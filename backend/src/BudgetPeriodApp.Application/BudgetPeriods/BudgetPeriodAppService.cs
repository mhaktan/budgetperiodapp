using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.BudgetPeriods.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.Budgets.Dto;
using BudgetPeriodApp.Approvals.Dto;
using BudgetPeriodApp.Authorization;
using BudgetPeriodApp.Flows;

namespace BudgetPeriodApp.BudgetPeriods
{
    public class BudgetPeriodAppService : AsyncCrudAppService<
        BudgetPeriod,
        BudgetPeriodDto,
        long,
        PagedBudgetPeriodResultRequestDto,
        CreateBudgetPeriodDto,
        BudgetPeriodDto>,
        IBudgetPeriodAppService
    {
        private readonly IFlowEngine _flowEngine;

        public BudgetPeriodAppService(IRepository<BudgetPeriod, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.BudgetPeriod_Read;
            GetAllPermissionName = PermissionNames.BudgetPeriod_Read;
            CreatePermissionName = PermissionNames.BudgetPeriod_Create;
            UpdatePermissionName = PermissionNames.BudgetPeriod_Update;
            DeletePermissionName = PermissionNames.BudgetPeriod_Delete;
        }

        protected override IQueryable<BudgetPeriod> CreateFilteredQuery(PagedBudgetPeriodResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Name != null && x.Name.Contains(input.Keyword)) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)))
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name != null && x.Name.Contains(input.Name))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(input.PeriodType.HasValue, x => x.PeriodType == (BudgetPeriodPeriodType)input.PeriodType.Value)
                .WhereIf(input.StartDate.HasValue, x => x.StartDate == input.StartDate.Value)
                .WhereIf(input.EndDate.HasValue, x => x.EndDate == input.EndDate.Value)
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value)
                .WhereIf(input.StartDateFrom.HasValue, x => x.StartDate >= input.StartDateFrom.Value)
                .WhereIf(input.StartDateTo.HasValue, x => x.StartDate <= input.StartDateTo.Value)
                .WhereIf(input.EndDateFrom.HasValue, x => x.EndDate >= input.EndDateFrom.Value)
                .WhereIf(input.EndDateTo.HasValue, x => x.EndDate <= input.EndDateTo.Value)
                .WhereIf(!input.PeriodTypeIn.IsNullOrWhiteSpace(), x => input.PeriodTypeIn
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(v => (BudgetPeriodPeriodType?)int.Parse(v.Trim()))
                    .Contains(x.PeriodType))
                .WhereIf(input.PeriodTypeNot.HasValue, x => x.PeriodType != (BudgetPeriodPeriodType)input.PeriodTypeNot.Value);
        }

        public override async Task<BudgetPeriodDto> CreateAsync(CreateBudgetPeriodDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "BudgetPeriod", result);
            return result;
        }

        public override async Task<BudgetPeriodDto> UpdateAsync(BudgetPeriodDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "BudgetPeriod", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "BudgetPeriod", new { Id = input.Id });
        }
        [Abp.Authorization.AbpAuthorize(PermissionNames.BudgetPeriod_Read)]
        public List<GroupCountDto> GetGroupedCount(BudgetPeriodGroupedCountInput input)
        {
            // Whitelist — istemciden gelen alan adı doğrudan sorguya girmez.
            var allowed = new[] { "PeriodType" };
            if (input.GroupBy == null || !allowed.Contains(input.GroupBy))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Gruplanabilir alan degil: {input.GroupBy}. Izin verilenler: {string.Join(", ", allowed)}");
            }

            var query = CreateFilteredQuery(input);

            switch (input.GroupBy)
            {
                case "PeriodType":
                    return query
                        .GroupBy(x => x.PeriodType)
                        .Select(g => new GroupCountDto
                        {
                            Key = ((int)g.Key).ToString(),
                            Label = g.Key.ToString(),
                            Count = g.Count(),
                        })
                        .ToList();
                default:
                    return new List<GroupCountDto>();
            }
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.BudgetPeriod_Read)]
        public decimal? GetStats(BudgetPeriodStatsInput input)
        {
            var query = CreateFilteredQuery(input);

            if (input.Aggregate == "avgDayDiff")
            {
                var allowedDates = new[] { "StartDate", "EndDate" };
                if (!allowedDates.Contains(input.FromField) || !allowedDates.Contains(input.ToField))
                {
                    throw new Abp.UI.UserFriendlyException("avgDayDiff icin gecerli iki tarih alani gerekli.");
                }
                switch (input.FromField + "|" + input.ToField)
                {
                    case "StartDate|EndDate":
                    {
                        var pairsStartDateEndDate = query
                            .Select(x => new { A = x.StartDate, B = x.EndDate })
                            .ToList();
                        if (pairsStartDateEndDate.Count == 0) return null;
                        return (decimal)pairsStartDateEndDate.Average(p => (p.B - p.A).TotalDays);
                    }
                    case "EndDate|StartDate":
                    {
                        var pairsEndDateStartDate = query
                            .Select(x => new { A = x.EndDate, B = x.StartDate })
                            .ToList();
                        if (pairsEndDateStartDate.Count == 0) return null;
                        return (decimal)pairsEndDateStartDate.Average(p => (p.B - p.A).TotalDays);
                    }
                    default: return null;
                }
            }

            var allowedNumeric = new string[0];
            if (!allowedNumeric.Contains(input.Field))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Toplanabilir alan degil: {input.Field}. Izin verilenler: {string.Join(", ", allowedNumeric)}");
            }
            switch (input.Field)
            {
                        default: return null;
            }
        }

        /// <summary>
        /// Rapor verisi — kok kayit ve alt koleksiyonlar TEK yanitta.
        /// PDF sablonu template basina tek apiBinding kullaniyor.
        /// </summary>
        [Abp.Authorization.AbpAuthorize(PermissionNames.BudgetPeriod_Read)]
        public async Task<BudgetPeriodReportDto> GetReportData(long id)
        {
            var root = await Repository.GetAll()
                .Include(x => x.Budgets)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (root == null)
                throw new Abp.UI.UserFriendlyException($"Kayit bulunamadi: {id}");

            return new BudgetPeriodReportDto
            {
                Data = ObjectMapper.Map<BudgetPeriodDto>(root),
                Budgets = ObjectMapper.Map<List<BudgetDto>>(
                    root.Budgets == null ? new List<Budget>() : root.Budgets.ToList()),
            };
        }

    }
}

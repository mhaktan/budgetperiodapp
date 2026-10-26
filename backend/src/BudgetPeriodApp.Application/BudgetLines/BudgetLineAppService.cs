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
using BudgetPeriodApp.BudgetLines.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.ExpenseRequests.Dto;
using BudgetPeriodApp.Approvals.Dto;
using BudgetPeriodApp.Authorization;
using BudgetPeriodApp.Flows;

namespace BudgetPeriodApp.BudgetLines
{
    public class BudgetLineAppService : AsyncCrudAppService<
        BudgetLine,
        BudgetLineDto,
        long,
        PagedBudgetLineResultRequestDto,
        CreateBudgetLineDto,
        BudgetLineDto>,
        IBudgetLineAppService
    {
        private readonly IFlowEngine _flowEngine;

        public BudgetLineAppService(IRepository<BudgetLine, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.BudgetLine_Read;
            GetAllPermissionName = PermissionNames.BudgetLine_Read;
            CreatePermissionName = PermissionNames.BudgetLine_Create;
            UpdatePermissionName = PermissionNames.BudgetLine_Update;
            DeletePermissionName = PermissionNames.BudgetLine_Delete;
        }

        protected override IQueryable<BudgetLine> CreateFilteredQuery(PagedBudgetLineResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)) ||
                    (x.Notes != null && x.Notes.Contains(input.Keyword)))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes != null && x.Notes.Contains(input.Notes))
                .WhereIf(input.PlannedAmount.HasValue, x => x.PlannedAmount == input.PlannedAmount.Value)
                .WhereIf(input.PlannedAmountFrom.HasValue, x => x.PlannedAmount >= input.PlannedAmountFrom.Value)
                .WhereIf(input.PlannedAmountTo.HasValue, x => x.PlannedAmount <= input.PlannedAmountTo.Value)
                .WhereIf(input.BudgetId.HasValue, x => x.BudgetId == input.BudgetId.Value)
                .WhereIf(input.ExpenseCategoryId.HasValue, x => x.ExpenseCategoryId == input.ExpenseCategoryId.Value);
        }

        public override async Task<BudgetLineDto> CreateAsync(CreateBudgetLineDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "BudgetLine", result);
            return result;
        }

        public override async Task<BudgetLineDto> UpdateAsync(BudgetLineDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "BudgetLine", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "BudgetLine", new { Id = input.Id });
        }
        [Abp.Authorization.AbpAuthorize(PermissionNames.BudgetLine_Read)]
        public List<GroupCountDto> GetGroupedCount(BudgetLineGroupedCountInput input)
        {
            // Whitelist — istemciden gelen alan adı doğrudan sorguya girmez.
            var allowed = new[] { "BudgetId", "ExpenseCategoryId" };
            if (input.GroupBy == null || !allowed.Contains(input.GroupBy))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Gruplanabilir alan degil: {input.GroupBy}. Izin verilenler: {string.Join(", ", allowed)}");
            }

            var query = CreateFilteredQuery(input);

            switch (input.GroupBy)
            {
                case "BudgetId":
                    return query
                        .GroupBy(x => new { Key = x.BudgetId, Label = x.Budget == null ? null : x.Budget.Title })
                        .Select(g => new GroupCountDto
                        {
                            Key = g.Key.Key.ToString(),
                            Label = g.Key.Label ?? "(bos)",
                            Count = g.Count(),
                        })
                        .ToList();
                case "ExpenseCategoryId":
                    return query
                        .GroupBy(x => new { Key = x.ExpenseCategoryId, Label = x.ExpenseCategory == null ? null : x.ExpenseCategory.Name })
                        .Select(g => new GroupCountDto
                        {
                            Key = g.Key.Key.ToString(),
                            Label = g.Key.Label ?? "(bos)",
                            Count = g.Count(),
                        })
                        .ToList();
                default:
                    return new List<GroupCountDto>();
            }
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.BudgetLine_Read)]
        public decimal? GetStats(BudgetLineStatsInput input)
        {
            var query = CreateFilteredQuery(input);

            if (input.Aggregate == "avgDayDiff")
            {
                var allowedDates = new string[0];
                if (!allowedDates.Contains(input.FromField) || !allowedDates.Contains(input.ToField))
                {
                    throw new Abp.UI.UserFriendlyException("avgDayDiff icin gecerli iki tarih alani gerekli.");
                }
                switch (input.FromField + "|" + input.ToField)
                {
                    default: return null;
                }
            }

            var allowedNumeric = new[] { "PlannedAmount" };
            if (!allowedNumeric.Contains(input.Field))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Toplanabilir alan degil: {input.Field}. Izin verilenler: {string.Join(", ", allowedNumeric)}");
            }
            switch (input.Field)
            {
                        case "PlannedAmount": return input.Aggregate == "sum" ? query.Sum(x => (decimal?)x.PlannedAmount)
                            : input.Aggregate == "min" ? query.Min(x => (decimal?)x.PlannedAmount)
                            : input.Aggregate == "max" ? query.Max(x => (decimal?)x.PlannedAmount)
                            : query.Average(x => (decimal?)x.PlannedAmount);
                        default: return null;
            }
        }

        /// <summary>
        /// Rapor verisi — kok kayit ve alt koleksiyonlar TEK yanitta.
        /// PDF sablonu template basina tek apiBinding kullaniyor.
        /// </summary>
        [Abp.Authorization.AbpAuthorize(PermissionNames.BudgetLine_Read)]
        public async Task<BudgetLineReportDto> GetReportData(long id)
        {
            var root = await Repository.GetAll()
                .Include(x => x.ExpenseRequests)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (root == null)
                throw new Abp.UI.UserFriendlyException($"Kayit bulunamadi: {id}");

            return new BudgetLineReportDto
            {
                Data = ObjectMapper.Map<BudgetLineDto>(root),
                ExpenseRequests = ObjectMapper.Map<List<ExpenseRequestDto>>(
                    root.ExpenseRequests == null ? new List<ExpenseRequest>() : root.ExpenseRequests.ToList()),
            };
        }

    }
}

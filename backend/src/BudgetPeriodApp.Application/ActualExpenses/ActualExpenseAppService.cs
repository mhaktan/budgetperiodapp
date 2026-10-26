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
using BudgetPeriodApp.ActualExpenses.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.Authorization;
using BudgetPeriodApp.Flows;

namespace BudgetPeriodApp.ActualExpenses
{
    public class ActualExpenseAppService : AsyncCrudAppService<
        ActualExpense,
        ActualExpenseDto,
        long,
        PagedActualExpenseResultRequestDto,
        CreateActualExpenseDto,
        ActualExpenseDto>,
        IActualExpenseAppService
    {
        private readonly IFlowEngine _flowEngine;

        public ActualExpenseAppService(IRepository<ActualExpense, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.ActualExpense_Read;
            GetAllPermissionName = PermissionNames.ActualExpense_Read;
            CreatePermissionName = PermissionNames.ActualExpense_Create;
            UpdatePermissionName = PermissionNames.ActualExpense_Update;
            DeletePermissionName = PermissionNames.ActualExpense_Delete;
        }

        protected override IQueryable<ActualExpense> CreateFilteredQuery(PagedActualExpenseResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)) ||
                    (x.Notes != null && x.Notes.Contains(input.Keyword)))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes != null && x.Notes.Contains(input.Notes))
                .WhereIf(input.Amount.HasValue, x => x.Amount == input.Amount.Value)
                .WhereIf(input.ExpenseDate.HasValue, x => x.ExpenseDate == input.ExpenseDate.Value)
                .WhereIf(input.AmountFrom.HasValue, x => x.Amount >= input.AmountFrom.Value)
                .WhereIf(input.AmountTo.HasValue, x => x.Amount <= input.AmountTo.Value)
                .WhereIf(input.ExpenseDateFrom.HasValue, x => x.ExpenseDate >= input.ExpenseDateFrom.Value)
                .WhereIf(input.ExpenseDateTo.HasValue, x => x.ExpenseDate <= input.ExpenseDateTo.Value)
                .WhereIf(input.ExpenseRequestId.HasValue, x => x.ExpenseRequestId == input.ExpenseRequestId.Value)
                .WhereIf(input.ExpenseCategoryId.HasValue, x => x.ExpenseCategoryId == input.ExpenseCategoryId.Value);
        }

        public override async Task<ActualExpenseDto> CreateAsync(CreateActualExpenseDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "ActualExpense", result);
            return result;
        }

        public override async Task<ActualExpenseDto> UpdateAsync(ActualExpenseDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "ActualExpense", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "ActualExpense", new { Id = input.Id });
        }
        [Abp.Authorization.AbpAuthorize(PermissionNames.ActualExpense_Read)]
        public List<GroupCountDto> GetGroupedCount(ActualExpenseGroupedCountInput input)
        {
            // Whitelist — istemciden gelen alan adı doğrudan sorguya girmez.
            var allowed = new[] { "ExpenseRequestId", "ExpenseCategoryId" };
            if (input.GroupBy == null || !allowed.Contains(input.GroupBy))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Gruplanabilir alan degil: {input.GroupBy}. Izin verilenler: {string.Join(", ", allowed)}");
            }

            var query = CreateFilteredQuery(input);

            switch (input.GroupBy)
            {
                case "ExpenseRequestId":
                    return query
                        .GroupBy(x => new { Key = x.ExpenseRequestId, Label = x.ExpenseRequest == null ? null : x.ExpenseRequest.Title })
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

        [Abp.Authorization.AbpAuthorize(PermissionNames.ActualExpense_Read)]
        public decimal? GetStats(ActualExpenseStatsInput input)
        {
            var query = CreateFilteredQuery(input);

            if (input.Aggregate == "avgDayDiff")
            {
                var allowedDates = new[] { "ExpenseDate" };
                if (!allowedDates.Contains(input.FromField) || !allowedDates.Contains(input.ToField))
                {
                    throw new Abp.UI.UserFriendlyException("avgDayDiff icin gecerli iki tarih alani gerekli.");
                }
                switch (input.FromField + "|" + input.ToField)
                {
                    default: return null;
                }
            }

            var allowedNumeric = new[] { "Amount" };
            if (!allowedNumeric.Contains(input.Field))
            {
                throw new Abp.UI.UserFriendlyException(
                    $"Toplanabilir alan degil: {input.Field}. Izin verilenler: {string.Join(", ", allowedNumeric)}");
            }
            switch (input.Field)
            {
                        case "Amount": return input.Aggregate == "sum" ? query.Sum(x => (decimal?)x.Amount)
                            : input.Aggregate == "min" ? query.Min(x => (decimal?)x.Amount)
                            : input.Aggregate == "max" ? query.Max(x => (decimal?)x.Amount)
                            : query.Average(x => (decimal?)x.Amount);
                        default: return null;
            }
        }

    }
}

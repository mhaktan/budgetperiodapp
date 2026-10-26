using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.ActualExpenses.Dto;

namespace BudgetPeriodApp.ActualExpenses
{
    public interface IActualExpenseAppService : IAsyncCrudAppService<
        ActualExpenseDto,
        long,
        PagedActualExpenseResultRequestDto,
        CreateActualExpenseDto,
        ActualExpenseDto>
    {
        List<GroupCountDto> GetGroupedCount(ActualExpenseGroupedCountInput input);
        decimal? GetStats(ActualExpenseStatsInput input);
    }
}

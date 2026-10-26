using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.StateMachine.Dto;
using BudgetPeriodApp.Budgets.Dto;

namespace BudgetPeriodApp.Budgets
{
    public interface IBudgetAppService : IAsyncCrudAppService<
        BudgetDto,
        long,
        PagedBudgetResultRequestDto,
        CreateBudgetDto,
        BudgetDto>
    {
        Task<BudgetDto> ChangeStatusAsync(long id, ChangeStatusInput input);

        // Yalniz ApprovalAppService cagirir — REST'e acik degil
        [Abp.Application.Services.RemoteService(false)]
        Task<BudgetDto> ApplyApprovalTransitionAsync(long id, ChangeStatusInput input);
        List<GroupCountDto> GetGroupedCount(BudgetGroupedCountInput input);
        decimal? GetStats(BudgetStatsInput input);
        Task<BudgetReportDto> GetReportData(long id);
    }
}

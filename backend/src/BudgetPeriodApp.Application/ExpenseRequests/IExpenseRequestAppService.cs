using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.StateMachine.Dto;
using BudgetPeriodApp.ExpenseRequests.Dto;

namespace BudgetPeriodApp.ExpenseRequests
{
    public interface IExpenseRequestAppService : IAsyncCrudAppService<
        ExpenseRequestDto,
        long,
        PagedExpenseRequestResultRequestDto,
        CreateExpenseRequestDto,
        ExpenseRequestDto>
    {
        Task<ExpenseRequestDto> ChangeStatusAsync(long id, ChangeStatusInput input);

        // Yalniz ApprovalAppService cagirir — REST'e acik degil
        [Abp.Application.Services.RemoteService(false)]
        Task<ExpenseRequestDto> ApplyApprovalTransitionAsync(long id, ChangeStatusInput input);
        List<GroupCountDto> GetGroupedCount(ExpenseRequestGroupedCountInput input);
        decimal? GetStats(ExpenseRequestStatsInput input);
        Task<ExpenseRequestReportDto> GetReportData(long id);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.BudgetLines.Dto;

namespace BudgetPeriodApp.BudgetLines
{
    public interface IBudgetLineAppService : IAsyncCrudAppService<
        BudgetLineDto,
        long,
        PagedBudgetLineResultRequestDto,
        CreateBudgetLineDto,
        BudgetLineDto>
    {
        List<GroupCountDto> GetGroupedCount(BudgetLineGroupedCountInput input);
        decimal? GetStats(BudgetLineStatsInput input);
        Task<BudgetLineReportDto> GetReportData(long id);
    }
}

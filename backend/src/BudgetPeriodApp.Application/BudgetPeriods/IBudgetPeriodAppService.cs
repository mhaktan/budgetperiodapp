using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.BudgetPeriods.Dto;

namespace BudgetPeriodApp.BudgetPeriods
{
    public interface IBudgetPeriodAppService : IAsyncCrudAppService<
        BudgetPeriodDto,
        long,
        PagedBudgetPeriodResultRequestDto,
        CreateBudgetPeriodDto,
        BudgetPeriodDto>
    {
        List<GroupCountDto> GetGroupedCount(BudgetPeriodGroupedCountInput input);
        decimal? GetStats(BudgetPeriodStatsInput input);
        Task<BudgetPeriodReportDto> GetReportData(long id);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.ExpenseCategorys.Dto;

namespace BudgetPeriodApp.ExpenseCategorys
{
    public interface IExpenseCategoryAppService : IAsyncCrudAppService<
        ExpenseCategoryDto,
        long,
        PagedExpenseCategoryResultRequestDto,
        CreateExpenseCategoryDto,
        ExpenseCategoryDto>
    {
        Task<ExpenseCategoryReportDto> GetReportData(long id);
    }
}

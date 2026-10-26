using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using BudgetPeriodApp.Analytics.Dto;
using BudgetPeriodApp.Departments.Dto;

namespace BudgetPeriodApp.Departments
{
    public interface IDepartmentAppService : IAsyncCrudAppService<
        DepartmentDto,
        long,
        PagedDepartmentResultRequestDto,
        CreateDepartmentDto,
        DepartmentDto>
    {
        Task<DepartmentReportDto> GetReportData(long id);
    }
}

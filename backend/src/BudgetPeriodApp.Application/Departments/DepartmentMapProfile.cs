using AutoMapper;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.Departments.Dto;

namespace BudgetPeriodApp.Departments
{
    public class DepartmentMapProfile : Profile
    {
        public DepartmentMapProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<DepartmentDto, Department>();
        }
    }
}

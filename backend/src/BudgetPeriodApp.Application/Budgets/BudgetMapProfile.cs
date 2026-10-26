using AutoMapper;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.Budgets.Dto;

namespace BudgetPeriodApp.Budgets
{
    public class BudgetMapProfile : Profile
    {
        public BudgetMapProfile()
        {
            CreateMap<Budget, BudgetDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateBudgetDto, Budget>();
            CreateMap<BudgetDto, Budget>();
        }
    }
}

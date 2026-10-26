using AutoMapper;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.BudgetPeriods.Dto;

namespace BudgetPeriodApp.BudgetPeriods
{
    public class BudgetPeriodMapProfile : Profile
    {
        public BudgetPeriodMapProfile()
        {
            CreateMap<BudgetPeriod, BudgetPeriodDto>();
            CreateMap<CreateBudgetPeriodDto, BudgetPeriod>();
            CreateMap<BudgetPeriodDto, BudgetPeriod>();
        }
    }
}

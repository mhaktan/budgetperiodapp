using AutoMapper;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.BudgetLines.Dto;

namespace BudgetPeriodApp.BudgetLines
{
    public class BudgetLineMapProfile : Profile
    {
        public BudgetLineMapProfile()
        {
            CreateMap<BudgetLine, BudgetLineDto>();
            CreateMap<CreateBudgetLineDto, BudgetLine>();
            CreateMap<BudgetLineDto, BudgetLine>();
        }
    }
}

using AutoMapper;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.ExpenseRequests.Dto;

namespace BudgetPeriodApp.ExpenseRequests
{
    public class ExpenseRequestMapProfile : Profile
    {
        public ExpenseRequestMapProfile()
        {
            CreateMap<ExpenseRequest, ExpenseRequestDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateExpenseRequestDto, ExpenseRequest>();
            CreateMap<ExpenseRequestDto, ExpenseRequest>();
        }
    }
}

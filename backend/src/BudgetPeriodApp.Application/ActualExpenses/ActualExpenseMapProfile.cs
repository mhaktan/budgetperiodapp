using AutoMapper;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.ActualExpenses.Dto;

namespace BudgetPeriodApp.ActualExpenses
{
    public class ActualExpenseMapProfile : Profile
    {
        public ActualExpenseMapProfile()
        {
            CreateMap<ActualExpense, ActualExpenseDto>();
            CreateMap<CreateActualExpenseDto, ActualExpense>();
            CreateMap<ActualExpenseDto, ActualExpense>();
        }
    }
}

using AutoMapper;
using BudgetPeriodApp.Entities;
using BudgetPeriodApp.ExpenseCategorys.Dto;

namespace BudgetPeriodApp.ExpenseCategorys
{
    public class ExpenseCategoryMapProfile : Profile
    {
        public ExpenseCategoryMapProfile()
        {
            CreateMap<ExpenseCategory, ExpenseCategoryDto>();
            CreateMap<CreateExpenseCategoryDto, ExpenseCategory>();
            CreateMap<ExpenseCategoryDto, ExpenseCategory>();
        }
    }
}

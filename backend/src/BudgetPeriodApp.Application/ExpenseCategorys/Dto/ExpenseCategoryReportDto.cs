using System;
using System.Collections.Generic;
using BudgetPeriodApp.BudgetLines.Dto;
using BudgetPeriodApp.ExpenseRequests.Dto;
using BudgetPeriodApp.ActualExpenses.Dto;

namespace BudgetPeriodApp.ExpenseCategorys.Dto
{
    /// <summary>
    /// Rapor verisi — kok kayit ve alt koleksiyonlar tek yanitta.
    /// PDF sablonu tek apiBinding kullandigi icin nested donuyoruz.
    /// </summary>
    public class ExpenseCategoryReportDto
    {
        public ExpenseCategoryDto Data { get; set; }
        public List<BudgetLineDto> BudgetLines { get; set; }
        public List<ExpenseRequestDto> ExpenseRequests { get; set; }
        public List<ActualExpenseDto> ActualExpenses { get; set; }
    }
}

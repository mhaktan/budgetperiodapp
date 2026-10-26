using System;
using Abp.Application.Services.Dto;

namespace BudgetPeriodApp.ActualExpenses.Dto
{
    public class PagedActualExpenseResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? ExpenseRequestId { get; set; }
        public long? ExpenseCategoryId { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public string Notes { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public DateTime? ExpenseDateFrom { get; set; }
        public DateTime? ExpenseDateTo { get; set; }
    }
}

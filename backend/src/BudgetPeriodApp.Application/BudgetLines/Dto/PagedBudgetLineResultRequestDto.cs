using System;
using Abp.Application.Services.Dto;

namespace BudgetPeriodApp.BudgetLines.Dto
{
    public class PagedBudgetLineResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? BudgetId { get; set; }
        public long? ExpenseCategoryId { get; set; }
        public string Description { get; set; }
        public decimal? PlannedAmount { get; set; }
        public string Notes { get; set; }
        public decimal? PlannedAmountFrom { get; set; }
        public decimal? PlannedAmountTo { get; set; }
    }
}

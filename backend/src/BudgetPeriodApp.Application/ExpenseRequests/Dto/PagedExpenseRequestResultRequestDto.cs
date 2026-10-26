using System;
using Abp.Application.Services.Dto;

namespace BudgetPeriodApp.ExpenseRequests.Dto
{
    public class PagedExpenseRequestResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? BudgetLineId { get; set; }
        public long? ExpenseCategoryId { get; set; }
        public long? DepartmentId { get; set; }
        public string Title { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? RequestDate { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public string RevisionNote { get; set; }
        public decimal? AmountFrom { get; set; }
        public decimal? AmountTo { get; set; }
        public DateTime? RequestDateFrom { get; set; }
        public DateTime? RequestDateTo { get; set; }
        /// <summary>Virgülle ayrılmış enum indeksleri — ör. "0,2"</summary>
        public string StatusIn { get; set; }
        public int? StatusNot { get; set; }
    }
}

using System;
using Abp.Application.Services.Dto;

namespace BudgetPeriodApp.Budgets.Dto
{
    public class PagedBudgetResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? BudgetPeriodId { get; set; }
        public long? DepartmentId { get; set; }
        public string Title { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public string RevisionNote { get; set; }
        public decimal? TotalAmountFrom { get; set; }
        public decimal? TotalAmountTo { get; set; }
        /// <summary>Virgülle ayrılmış enum indeksleri — ör. "0,2"</summary>
        public string StatusIn { get; set; }
        public int? StatusNot { get; set; }
    }
}

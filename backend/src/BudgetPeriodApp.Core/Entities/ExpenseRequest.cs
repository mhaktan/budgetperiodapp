using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace BudgetPeriodApp.Entities
{
    // State Machine: status — Draft → PendingManagerApproval → PendingFinanceApproval → Approved → Revision → Rejected → Cancelled
    // Initial: Draft | Transitions: Draft→PendingManagerApproval[Submit], PendingManagerApproval→PendingFinanceApproval[Approve], PendingManagerApproval→Revision[Revise], PendingManagerApproval→Rejected[Reject], PendingFinanceApproval→Approved[Approve], PendingFinanceApproval→Revision[Revise], PendingFinanceApproval→Rejected[Reject], Revision→PendingManagerApproval[Resubmit], *→Cancelled[Cancel]
    [Table("ExpenseRequests")]
    public class ExpenseRequest : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public decimal Amount { get; set; }

        public DateTime RequestDate { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public ExpenseRequestStatus Status { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public long BudgetLineId { get; set; }

        [ForeignKey(nameof(BudgetLineId))]
        public virtual BudgetLine BudgetLine { get; set; }

        public long ExpenseCategoryId { get; set; }

        [ForeignKey(nameof(ExpenseCategoryId))]
        public virtual ExpenseCategory ExpenseCategory { get; set; }

        public long DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        public virtual ICollection<ActualExpense> ActualExpenses { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace BudgetPeriodApp.Entities
{
    // State Machine: status — Draft → PendingApproval → Approved → Revision → Cancelled
    // Initial: Draft | Transitions: Draft→PendingApproval[Submit], PendingApproval→Approved[Approve], PendingApproval→Revision[Revise], Revision→PendingApproval[Resubmit], *→Cancelled[Cancel]
    [Table("Budgets")]
    public class Budget : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public decimal TotalAmount { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public BudgetStatus Status { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public long BudgetPeriodId { get; set; }

        [ForeignKey(nameof(BudgetPeriodId))]
        public virtual BudgetPeriod BudgetPeriod { get; set; }

        public long DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        public virtual ICollection<BudgetLine> BudgetLines { get; set; }

    }
}
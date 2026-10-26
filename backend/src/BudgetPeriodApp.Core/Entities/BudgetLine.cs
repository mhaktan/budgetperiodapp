using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace BudgetPeriodApp.Entities
{
    [Table("BudgetLines")]
    public class BudgetLine : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public decimal PlannedAmount { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public long BudgetId { get; set; }

        [ForeignKey(nameof(BudgetId))]
        public virtual Budget Budget { get; set; }

        public long ExpenseCategoryId { get; set; }

        [ForeignKey(nameof(ExpenseCategoryId))]
        public virtual ExpenseCategory ExpenseCategory { get; set; }

        public virtual ICollection<ExpenseRequest> ExpenseRequests { get; set; }

    }
}
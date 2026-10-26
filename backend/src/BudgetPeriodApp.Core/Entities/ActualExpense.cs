using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace BudgetPeriodApp.Entities
{
    [Table("ActualExpenses")]
    public class ActualExpense : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public long ExpenseRequestId { get; set; }

        [ForeignKey(nameof(ExpenseRequestId))]
        public virtual ExpenseRequest ExpenseRequest { get; set; }

        public long ExpenseCategoryId { get; set; }

        [ForeignKey(nameof(ExpenseCategoryId))]
        public virtual ExpenseCategory ExpenseCategory { get; set; }

    }
}
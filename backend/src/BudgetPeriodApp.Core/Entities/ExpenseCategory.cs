using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace BudgetPeriodApp.Entities
{
    [Table("ExpenseCategorys")]
    public class ExpenseCategory : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<BudgetLine> BudgetLines { get; set; }

        public virtual ICollection<ExpenseRequest> ExpenseRequests { get; set; }

        public virtual ICollection<ActualExpense> ActualExpenses { get; set; }

    }
}
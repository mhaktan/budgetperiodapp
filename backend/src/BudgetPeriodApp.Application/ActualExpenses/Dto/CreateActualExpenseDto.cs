using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace BudgetPeriodApp.ActualExpenses.Dto
{
    [AutoMapTo(typeof(Entities.ActualExpense))]
    public class CreateActualExpenseDto
    {
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public long ExpenseRequestId { get; set; }

        public long ExpenseCategoryId { get; set; }

    }
}
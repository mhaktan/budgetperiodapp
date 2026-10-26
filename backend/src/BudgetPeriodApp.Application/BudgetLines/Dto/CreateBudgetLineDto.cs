using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace BudgetPeriodApp.BudgetLines.Dto
{
    [AutoMapTo(typeof(Entities.BudgetLine))]
    public class CreateBudgetLineDto
    {
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        public decimal PlannedAmount { get; set; }

        [MaxLength(1000)]
        public string Notes { get; set; }

        public long BudgetId { get; set; }

        public long ExpenseCategoryId { get; set; }

    }
}
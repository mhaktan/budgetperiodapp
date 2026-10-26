using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace BudgetPeriodApp.ExpenseCategorys.Dto
{
    [AutoMapTo(typeof(Entities.ExpenseCategory))]
    public class CreateExpenseCategoryDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

    }
}
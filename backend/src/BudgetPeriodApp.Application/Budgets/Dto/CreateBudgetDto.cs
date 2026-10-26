using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace BudgetPeriodApp.Budgets.Dto
{
    [AutoMapTo(typeof(Entities.Budget))]
    public class CreateBudgetDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public decimal TotalAmount { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int Status { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public long BudgetPeriodId { get; set; }

        public long DepartmentId { get; set; }

    }
}
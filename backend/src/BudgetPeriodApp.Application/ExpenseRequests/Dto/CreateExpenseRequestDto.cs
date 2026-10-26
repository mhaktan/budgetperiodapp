using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace BudgetPeriodApp.ExpenseRequests.Dto
{
    [AutoMapTo(typeof(Entities.ExpenseRequest))]
    public class CreateExpenseRequestDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public decimal Amount { get; set; }

        public DateTime RequestDate { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public int Status { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public long BudgetLineId { get; set; }

        public long ExpenseCategoryId { get; set; }

        public long DepartmentId { get; set; }

    }
}
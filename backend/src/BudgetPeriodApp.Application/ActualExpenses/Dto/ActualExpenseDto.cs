using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace BudgetPeriodApp.ActualExpenses.Dto
{
    [AutoMapFrom(typeof(Entities.ActualExpense))]
    public class ActualExpenseDto : EntityDto<long>
    {
        public string Description { get; set; }

        public decimal Amount { get; set; }

        public DateTime ExpenseDate { get; set; }

        public string Notes { get; set; }

        public long ExpenseRequestId { get; set; }

        public long ExpenseCategoryId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}
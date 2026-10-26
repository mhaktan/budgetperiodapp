using System;
using BudgetPeriodApp.Analytics.Dto;

namespace BudgetPeriodApp.ExpenseRequests.Dto
{
    /// <summary>GetAll ile ayni filtreleri kabul eder, ustune GroupBy alir.</summary>
    public class ExpenseRequestGroupedCountInput : PagedExpenseRequestResultRequestDto
    {
        public string GroupBy { get; set; }
    }

    public class ExpenseRequestStatsInput : PagedExpenseRequestResultRequestDto
    {
        /// <summary>avg | sum | min | max | avgDayDiff</summary>
        public string Aggregate { get; set; }
        public string Field { get; set; }
        public string FromField { get; set; }
        public string ToField { get; set; }
    }
}

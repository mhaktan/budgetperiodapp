using System;
using BudgetPeriodApp.Analytics.Dto;

namespace BudgetPeriodApp.ActualExpenses.Dto
{
    /// <summary>GetAll ile ayni filtreleri kabul eder, ustune GroupBy alir.</summary>
    public class ActualExpenseGroupedCountInput : PagedActualExpenseResultRequestDto
    {
        public string GroupBy { get; set; }
    }

    public class ActualExpenseStatsInput : PagedActualExpenseResultRequestDto
    {
        /// <summary>avg | sum | min | max | avgDayDiff</summary>
        public string Aggregate { get; set; }
        public string Field { get; set; }
        public string FromField { get; set; }
        public string ToField { get; set; }
    }
}

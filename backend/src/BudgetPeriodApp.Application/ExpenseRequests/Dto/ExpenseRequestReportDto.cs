using System;
using System.Collections.Generic;
using BudgetPeriodApp.ActualExpenses.Dto;
using BudgetPeriodApp.Approvals.Dto;

namespace BudgetPeriodApp.ExpenseRequests.Dto
{
    /// <summary>
    /// Rapor verisi — kok kayit ve alt koleksiyonlar tek yanitta.
    /// PDF sablonu tek apiBinding kullandigi icin nested donuyoruz.
    /// </summary>
    public class ExpenseRequestReportDto
    {
        public ExpenseRequestDto Data { get; set; }
        public List<ActualExpenseDto> ActualExpenses { get; set; }
        public List<ApprovalRecordDto> ApprovalHistory { get; set; }
    }
}

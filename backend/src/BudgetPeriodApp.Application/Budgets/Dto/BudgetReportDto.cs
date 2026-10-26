using System;
using System.Collections.Generic;
using BudgetPeriodApp.BudgetLines.Dto;
using BudgetPeriodApp.Approvals.Dto;

namespace BudgetPeriodApp.Budgets.Dto
{
    /// <summary>
    /// Rapor verisi — kok kayit ve alt koleksiyonlar tek yanitta.
    /// PDF sablonu tek apiBinding kullandigi icin nested donuyoruz.
    /// </summary>
    public class BudgetReportDto
    {
        public BudgetDto Data { get; set; }
        public List<BudgetLineDto> BudgetLines { get; set; }
        public List<ApprovalRecordDto> ApprovalHistory { get; set; }
    }
}

using System;
using System.Collections.Generic;
using BudgetPeriodApp.ExpenseRequests.Dto;

namespace BudgetPeriodApp.BudgetLines.Dto
{
    /// <summary>
    /// Rapor verisi — kok kayit ve alt koleksiyonlar tek yanitta.
    /// PDF sablonu tek apiBinding kullandigi icin nested donuyoruz.
    /// </summary>
    public class BudgetLineReportDto
    {
        public BudgetLineDto Data { get; set; }
        public List<ExpenseRequestDto> ExpenseRequests { get; set; }
    }
}

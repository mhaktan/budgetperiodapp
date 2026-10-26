using System;
using System.Collections.Generic;
using BudgetPeriodApp.Budgets.Dto;

namespace BudgetPeriodApp.BudgetPeriods.Dto
{
    /// <summary>
    /// Rapor verisi — kok kayit ve alt koleksiyonlar tek yanitta.
    /// PDF sablonu tek apiBinding kullandigi icin nested donuyoruz.
    /// </summary>
    public class BudgetPeriodReportDto
    {
        public BudgetPeriodDto Data { get; set; }
        public List<BudgetDto> Budgets { get; set; }
    }
}

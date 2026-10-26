using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace BudgetPeriodApp.BudgetLines.Dto
{
    [AutoMapFrom(typeof(Entities.BudgetLine))]
    public class BudgetLineDto : EntityDto<long>
    {
        public string Description { get; set; }

        public decimal PlannedAmount { get; set; }

        public string Notes { get; set; }

        public long BudgetId { get; set; }

        public long ExpenseCategoryId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}
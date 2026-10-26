using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace BudgetPeriodApp.Budgets.Dto
{
    [AutoMapFrom(typeof(Entities.Budget))]
    public class BudgetDto : EntityDto<long>
    {
        public string Title { get; set; }

        public decimal TotalAmount { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public string RevisionNote { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long BudgetPeriodId { get; set; }

        public long DepartmentId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}
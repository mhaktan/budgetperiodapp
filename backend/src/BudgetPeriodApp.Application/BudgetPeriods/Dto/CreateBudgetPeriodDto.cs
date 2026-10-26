using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace BudgetPeriodApp.BudgetPeriods.Dto
{
    [AutoMapTo(typeof(Entities.BudgetPeriod))]
    public class CreateBudgetPeriodDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public int PeriodType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

    }
}
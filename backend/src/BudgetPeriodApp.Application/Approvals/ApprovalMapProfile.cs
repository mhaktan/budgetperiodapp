using AutoMapper;
using BudgetPeriodApp.Approvals.Dto;
using BudgetPeriodApp.Entities;

namespace BudgetPeriodApp.Approvals
{
    public class ApprovalMapProfile : Profile
    {
        public ApprovalMapProfile()
        {
            CreateMap<ApprovalRecord, ApprovalRecordDto>();
            CreateMap<StatusChangeLog, StatusChangeLogDto>();
        }
    }
}

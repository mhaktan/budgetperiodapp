using Abp.Authorization;
using Abp.Localization;

namespace BudgetPeriodApp.Authorization
{
    public class BudgetPeriodAppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var pages = context.GetPermissionOrNull("Pages") ?? context.CreatePermission("Pages", L("Pages"));

            // BudgetPeriod
            pages.CreateChildPermission(PermissionNames.BudgetPeriod_Read, L("BudgetPeriod.Read"));
            pages.CreateChildPermission(PermissionNames.BudgetPeriod_Create, L("BudgetPeriod.Create"));
            pages.CreateChildPermission(PermissionNames.BudgetPeriod_Update, L("BudgetPeriod.Update"));
            pages.CreateChildPermission(PermissionNames.BudgetPeriod_Delete, L("BudgetPeriod.Delete"));

            // Department
            pages.CreateChildPermission(PermissionNames.Department_Read, L("Department.Read"));
            pages.CreateChildPermission(PermissionNames.Department_Create, L("Department.Create"));
            pages.CreateChildPermission(PermissionNames.Department_Update, L("Department.Update"));
            pages.CreateChildPermission(PermissionNames.Department_Delete, L("Department.Delete"));

            // ExpenseCategory
            pages.CreateChildPermission(PermissionNames.ExpenseCategory_Read, L("ExpenseCategory.Read"));
            pages.CreateChildPermission(PermissionNames.ExpenseCategory_Create, L("ExpenseCategory.Create"));
            pages.CreateChildPermission(PermissionNames.ExpenseCategory_Update, L("ExpenseCategory.Update"));
            pages.CreateChildPermission(PermissionNames.ExpenseCategory_Delete, L("ExpenseCategory.Delete"));

            // Budget
            pages.CreateChildPermission(PermissionNames.Budget_Read, L("Budget.Read"));
            pages.CreateChildPermission(PermissionNames.Budget_Create, L("Budget.Create"));
            pages.CreateChildPermission(PermissionNames.Budget_Update, L("Budget.Update"));
            pages.CreateChildPermission(PermissionNames.Budget_Delete, L("Budget.Delete"));
            pages.CreateChildPermission(PermissionNames.Budget_ChangeStatus, L("Budget.ChangeStatus"));

            // BudgetLine
            pages.CreateChildPermission(PermissionNames.BudgetLine_Read, L("BudgetLine.Read"));
            pages.CreateChildPermission(PermissionNames.BudgetLine_Create, L("BudgetLine.Create"));
            pages.CreateChildPermission(PermissionNames.BudgetLine_Update, L("BudgetLine.Update"));
            pages.CreateChildPermission(PermissionNames.BudgetLine_Delete, L("BudgetLine.Delete"));

            // ExpenseRequest
            pages.CreateChildPermission(PermissionNames.ExpenseRequest_Read, L("ExpenseRequest.Read"));
            pages.CreateChildPermission(PermissionNames.ExpenseRequest_Create, L("ExpenseRequest.Create"));
            pages.CreateChildPermission(PermissionNames.ExpenseRequest_Update, L("ExpenseRequest.Update"));
            pages.CreateChildPermission(PermissionNames.ExpenseRequest_Delete, L("ExpenseRequest.Delete"));
            pages.CreateChildPermission(PermissionNames.ExpenseRequest_ChangeStatus, L("ExpenseRequest.ChangeStatus"));

            // ActualExpense
            pages.CreateChildPermission(PermissionNames.ActualExpense_Read, L("ActualExpense.Read"));
            pages.CreateChildPermission(PermissionNames.ActualExpense_Create, L("ActualExpense.Create"));
            pages.CreateChildPermission(PermissionNames.ActualExpense_Update, L("ActualExpense.Update"));
            pages.CreateChildPermission(PermissionNames.ActualExpense_Delete, L("ActualExpense.Delete"));

            // RBAC
            pages.CreateChildPermission(PermissionNames.AppUser_Read, L("AppUser.Read"));
            pages.CreateChildPermission(PermissionNames.AppRole_Read, L("AppRole.Read"));
            pages.CreateChildPermission(PermissionNames.AppUser_Create, L("AppUser.Create"));
            pages.CreateChildPermission(PermissionNames.AppRole_Create, L("AppRole.Create"));
            pages.CreateChildPermission(PermissionNames.AppUser_Update, L("AppUser.Update"));
            pages.CreateChildPermission(PermissionNames.AppRole_Update, L("AppRole.Update"));
            pages.CreateChildPermission(PermissionNames.AppUser_Delete, L("AppUser.Delete"));
            pages.CreateChildPermission(PermissionNames.AppRole_Delete, L("AppRole.Delete"));
            pages.CreateChildPermission(PermissionNames.AppRole_AssignPermissions, L("AppRole.AssignPermissions"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BudgetPeriodAppConsts.LocalizationSourceName);
        }
    }
}

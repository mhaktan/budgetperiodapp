namespace BudgetPeriodApp.Authorization
{
    public static class PermissionNames
    {
        public const string Pages = "Pages";

        // BudgetPeriod
        public const string BudgetPeriod_Read = "BudgetPeriod.Read";
        public const string BudgetPeriod_Create = "BudgetPeriod.Create";
        public const string BudgetPeriod_Update = "BudgetPeriod.Update";
        public const string BudgetPeriod_Delete = "BudgetPeriod.Delete";

        // Department
        public const string Department_Read = "Department.Read";
        public const string Department_Create = "Department.Create";
        public const string Department_Update = "Department.Update";
        public const string Department_Delete = "Department.Delete";

        // ExpenseCategory
        public const string ExpenseCategory_Read = "ExpenseCategory.Read";
        public const string ExpenseCategory_Create = "ExpenseCategory.Create";
        public const string ExpenseCategory_Update = "ExpenseCategory.Update";
        public const string ExpenseCategory_Delete = "ExpenseCategory.Delete";

        // Budget
        public const string Budget_Read = "Budget.Read";
        public const string Budget_Create = "Budget.Create";
        public const string Budget_Update = "Budget.Update";
        public const string Budget_Delete = "Budget.Delete";
        public const string Budget_ChangeStatus = "Budget.ChangeStatus";

        // BudgetLine
        public const string BudgetLine_Read = "BudgetLine.Read";
        public const string BudgetLine_Create = "BudgetLine.Create";
        public const string BudgetLine_Update = "BudgetLine.Update";
        public const string BudgetLine_Delete = "BudgetLine.Delete";

        // ExpenseRequest
        public const string ExpenseRequest_Read = "ExpenseRequest.Read";
        public const string ExpenseRequest_Create = "ExpenseRequest.Create";
        public const string ExpenseRequest_Update = "ExpenseRequest.Update";
        public const string ExpenseRequest_Delete = "ExpenseRequest.Delete";
        public const string ExpenseRequest_ChangeStatus = "ExpenseRequest.ChangeStatus";

        // ActualExpense
        public const string ActualExpense_Read = "ActualExpense.Read";
        public const string ActualExpense_Create = "ActualExpense.Create";
        public const string ActualExpense_Update = "ActualExpense.Update";
        public const string ActualExpense_Delete = "ActualExpense.Delete";

        // RBAC management
        public const string AppUser_Read = "AppUser.Read";
        public const string AppRole_Read = "AppRole.Read";
        public const string AppUser_Create = "AppUser.Create";
        public const string AppRole_Create = "AppRole.Create";
        public const string AppUser_Update = "AppUser.Update";
        public const string AppRole_Update = "AppRole.Update";
        public const string AppUser_Delete = "AppUser.Delete";
        public const string AppRole_Delete = "AppRole.Delete";
        public const string AppRole_AssignPermissions = "AppRole.AssignPermissions";

    }
}

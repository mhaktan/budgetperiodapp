using System.Collections.Generic;
using Abp.Dependency;

namespace BudgetPeriodApp.Authorization
{
    /// <summary>Single permission descriptor — name, group (entity), description.</summary>
    public class PermissionInfo
    {
        public string Name { get; }
        public string Group { get; }
        public string Description { get; }
        public bool IsRbac { get; }

        public PermissionInfo(string name, string group, string description, bool isRbac)
        {
            Name = name; Group = group; Description = description; IsRbac = isRbac;
        }
    }

    public interface IPermissionRegistry
    {
        IReadOnlyList<PermissionInfo> All { get; }
    }

    public class PermissionRegistry : IPermissionRegistry, ISingletonDependency
    {
        public IReadOnlyList<PermissionInfo> All { get; } = new List<PermissionInfo>
        {
            new PermissionInfo("BudgetPeriod.Read", "BudgetPeriod", "Read BudgetPeriod", false),
            new PermissionInfo("BudgetPeriod.Create", "BudgetPeriod", "Create BudgetPeriod", false),
            new PermissionInfo("BudgetPeriod.Update", "BudgetPeriod", "Update BudgetPeriod", false),
            new PermissionInfo("BudgetPeriod.Delete", "BudgetPeriod", "Delete BudgetPeriod", false),
            new PermissionInfo("Department.Read", "Department", "Read Department", false),
            new PermissionInfo("Department.Create", "Department", "Create Department", false),
            new PermissionInfo("Department.Update", "Department", "Update Department", false),
            new PermissionInfo("Department.Delete", "Department", "Delete Department", false),
            new PermissionInfo("ExpenseCategory.Read", "ExpenseCategory", "Read ExpenseCategory", false),
            new PermissionInfo("ExpenseCategory.Create", "ExpenseCategory", "Create ExpenseCategory", false),
            new PermissionInfo("ExpenseCategory.Update", "ExpenseCategory", "Update ExpenseCategory", false),
            new PermissionInfo("ExpenseCategory.Delete", "ExpenseCategory", "Delete ExpenseCategory", false),
            new PermissionInfo("Budget.Read", "Budget", "Read Budget", false),
            new PermissionInfo("Budget.Create", "Budget", "Create Budget", false),
            new PermissionInfo("Budget.Update", "Budget", "Update Budget", false),
            new PermissionInfo("Budget.Delete", "Budget", "Delete Budget", false),
            new PermissionInfo("Budget.ChangeStatus", "Budget", "Change Budget status", false),
            new PermissionInfo("BudgetLine.Read", "BudgetLine", "Read BudgetLine", false),
            new PermissionInfo("BudgetLine.Create", "BudgetLine", "Create BudgetLine", false),
            new PermissionInfo("BudgetLine.Update", "BudgetLine", "Update BudgetLine", false),
            new PermissionInfo("BudgetLine.Delete", "BudgetLine", "Delete BudgetLine", false),
            new PermissionInfo("ExpenseRequest.Read", "ExpenseRequest", "Read ExpenseRequest", false),
            new PermissionInfo("ExpenseRequest.Create", "ExpenseRequest", "Create ExpenseRequest", false),
            new PermissionInfo("ExpenseRequest.Update", "ExpenseRequest", "Update ExpenseRequest", false),
            new PermissionInfo("ExpenseRequest.Delete", "ExpenseRequest", "Delete ExpenseRequest", false),
            new PermissionInfo("ExpenseRequest.ChangeStatus", "ExpenseRequest", "Change ExpenseRequest status", false),
            new PermissionInfo("ActualExpense.Read", "ActualExpense", "Read ActualExpense", false),
            new PermissionInfo("ActualExpense.Create", "ActualExpense", "Create ActualExpense", false),
            new PermissionInfo("ActualExpense.Update", "ActualExpense", "Update ActualExpense", false),
            new PermissionInfo("ActualExpense.Delete", "ActualExpense", "Delete ActualExpense", false),
            new PermissionInfo("AppUser.Read", "AppUser", "Read users", true),
            new PermissionInfo("AppRole.Read", "AppRole", "Read roles", true),
            new PermissionInfo("AppUser.Create", "AppUser", "Create users", true),
            new PermissionInfo("AppRole.Create", "AppRole", "Create roles", true),
            new PermissionInfo("AppUser.Update", "AppUser", "Update users", true),
            new PermissionInfo("AppRole.Update", "AppRole", "Update roles", true),
            new PermissionInfo("AppUser.Delete", "AppUser", "Delete users", true),
            new PermissionInfo("AppRole.Delete", "AppRole", "Delete roles", true),
            new PermissionInfo("AppRole.AssignPermissions", "AppRole", "Assign permissions to roles", true),
        };
    }
}

// ---------------------------------------------------------------------------
// Enum definitions — auto-generated from ER model
// Maps integer values to display labels for enum fields
// ---------------------------------------------------------------------------

// BudgetPeriod.periodType
export const BudgetPeriodPeriodTypeMap: Record<string, string> = {
  "0": "Monthly",
  "1": "Annual"
};
export const BudgetPeriodPeriodTypeOptions = [
  { label: "Monthly", value: "0" },
  { label: "Annual", value: "1" }
];

// Budget.status
export const BudgetStatusMap: Record<string, string> = {
  "0": "Draft",
  "1": "PendingApproval",
  "2": "Approved",
  "3": "Revision",
  "4": "Cancelled"
};
export const BudgetStatusOptions = [
  { label: "Draft", value: "0" },
  { label: "PendingApproval", value: "1" },
  { label: "Approved", value: "2" },
  { label: "Revision", value: "3" },
  { label: "Cancelled", value: "4" }
];

// ExpenseRequest.status
export const ExpenseRequestStatusMap: Record<string, string> = {
  "0": "Draft",
  "1": "PendingManagerApproval",
  "2": "PendingFinanceApproval",
  "3": "Approved",
  "4": "Revision",
  "5": "Rejected",
  "6": "Cancelled"
};
export const ExpenseRequestStatusOptions = [
  { label: "Draft", value: "0" },
  { label: "PendingManagerApproval", value: "1" },
  { label: "PendingFinanceApproval", value: "2" },
  { label: "Approved", value: "3" },
  { label: "Revision", value: "4" },
  { label: "Rejected", value: "5" },
  { label: "Cancelled", value: "6" }
];

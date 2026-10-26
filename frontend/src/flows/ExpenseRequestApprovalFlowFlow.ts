// Auto-generated flow: ExpenseRequest Approval Flow
// Auto-generated approval flow for ExpenseRequest. Customize email templates and add conditions as needed.
// Resource: ExpenseRequest
// Enabled: true
//
// Nodes:
  // trigger: On ExpenseRequest Submit
  // condition: Status = PendingManagerApproval?
  // approval: ExpenseRequest Approval
  // action: Send Approval Email
  // trigger: On ExpenseRequest Approved
  // action: Send Completion Email
//
// Edges:
  // On ExpenseRequest Submit → Status = PendingManagerApproval?
  // Status = PendingManagerApproval? → ExpenseRequest Approval (true)
  // ExpenseRequest Approval → Send Approval Email
  // On ExpenseRequest Approved → Send Completion Email
//
// This file is for documentation purposes.
// Flow execution is handled by FlowEngine.ts using flowDefinitions.json.

export const FLOW_EXPENSEREQUEST_APPROVAL_FLOW_ID = 'flow-ExpenseRequest-approval';

import React, { useState } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { BrowserRouter, Route, Routes, Navigate } from 'react-router-dom';
import { isAuthenticated, clearAuth } from './dataProvider';
import { LoginPage } from './screens/LoginPage';
import { GlobalMenu } from './components/GlobalMenu';
import { FlowProvider } from './flows/FlowProvider';
import { BudgetPeriodList } from './screens/BudgetPeriod/BudgetPeriodList';
import { DepartmentList } from './screens/Department/DepartmentList';
import { ExpenseCategoryList } from './screens/ExpenseCategory/ExpenseCategoryList';
import { BudgetList } from './screens/Budget/BudgetList';
import { BudgetLineList } from './screens/BudgetLine/BudgetLineList';
import { ExpenseRequestList } from './screens/ExpenseRequest/ExpenseRequestList';
import { ActualExpenseList } from './screens/ActualExpense/ActualExpenseList';
import { DashboardScreen } from './screens/dashboard/DashboardScreen';
import TaskInboxScreen from './screens/tasks/TaskInboxScreen';
import UserListScreen from './admin/UserListScreen';
import RoleListScreen from './admin/RoleListScreen';

const queryClient = new QueryClient({
  defaultOptions: { queries: { retry: 1, staleTime: 0 } },
});

export const App: React.FC = () => {
  const [authenticated, setAuthenticated] = useState(isAuthenticated());


  const handleLogout = () => {
    clearAuth();
    setAuthenticated(false);
    queryClient.clear();
  };

  return (
    <QueryClientProvider client={queryClient}>
      <FlowProvider>
      <BrowserRouter>
        <Routes>
          <Route path="*" element={
            authenticated
              ? (
                <GlobalMenu>
                  <Routes>
                    <Route path="/" element={<Navigate to="/dashboard" replace />} />
          <Route path="/dashboard" element={<DashboardScreen />} />
          <Route path="/BudgetPeriod" element={<BudgetPeriodList />} />
          <Route path="/Department" element={<DepartmentList />} />
          <Route path="/ExpenseCategory" element={<ExpenseCategoryList />} />
          <Route path="/Budget" element={<BudgetList />} />
          <Route path="/BudgetLine" element={<BudgetLineList />} />
          <Route path="/ExpenseRequest" element={<ExpenseRequestList />} />
          <Route path="/ActualExpense" element={<ActualExpenseList />} />
          <Route path="/tasks" element={<TaskInboxScreen />} />
          <Route path="/users" element={<UserListScreen />} />
          <Route path="/roles" element={<RoleListScreen />} />
                  </Routes>
                </GlobalMenu>
              )
              : <LoginPage onLogin={() => setAuthenticated(true)} />
          } />
        </Routes>
      </BrowserRouter>
        </FlowProvider>
    </QueryClientProvider>
  );
};

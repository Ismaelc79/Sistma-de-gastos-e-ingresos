import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { LoginPage } from '../../features/auth/pages/LoginPage';
import { RegisterPage } from '../../features/auth/pages/RegisterPage';
import { DashboardPage } from '../../features/dashboard/pages/DashboardPage';
import { TransactionsPage } from '../../features/transactions/pages/TransactionsPage';
import { ReportsPage } from '../../features/reports/pages/ReportsPage';
import { ProfilePage } from '../../features/profile/pages/ProfilePage';

import { MainLayout } from '../../shared/layouts/MainLayout';
import { ProtectedRoute } from './ProtectedRoute';
import { CategoriesPages } from '../../features/categories/pages/CategoriesPages';

export const AppRoutes = () => {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        {/* Protected routes */}
        <Route
          path="/dashboard"
          element={
            <ProtectedRoute>
              <MainLayout>
                <DashboardPage />
              </MainLayout>
            </ProtectedRoute>
          }
        />

         <Route
          path="/categories"
          element={
            <ProtectedRoute>
              <MainLayout>
                <CategoriesPages />
              </MainLayout>
            </ProtectedRoute>
          }
        />

        <Route
          path="/transactions"
          element={
            <ProtectedRoute>
              <MainLayout>
                <TransactionsPage />
              </MainLayout>
            </ProtectedRoute>
          }
        />

        <Route
          path="/reports"
          element={
            <ProtectedRoute>
              <MainLayout>
                <ReportsPage />
              </MainLayout>
            </ProtectedRoute>
          }
        />

        <Route
          path="/profile"
          element={
            <ProtectedRoute>
              <MainLayout>
                <ProfilePage />
              </MainLayout>
            </ProtectedRoute>
          }
        />

        {/* Default redirect */}
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  );
};

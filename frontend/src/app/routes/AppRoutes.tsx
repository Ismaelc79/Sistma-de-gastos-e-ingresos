import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { LoginPage } from '../../features/auth/pages/LoginPage';
import { RegisterPage } from '../../features/auth/pages/RegisterPage';
import { DashboardPage } from '../../features/dashboard/pages/DashboardPage';
import { TransactionsPage } from '../../features/transactions/pages/TransactionsPage';

import { MainLayout } from '../../shared/layouts/MainLayout';
import { ProtectedRoute } from './ProtectedRoute';

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
          path="/transactions"
          element={
            <ProtectedRoute>
              <MainLayout>
                <div className="text-center py-12">
                  <h1 className="text-2xl font-bold text-dark-900">Falta dinero para completar esto</h1>
                  <p className="text-dark-600 mt-2">Coming soon...</p>
                </div>
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
                <div className="text-center py-12">
                  <h1 className="text-2xl font-bold text-dark-900">Reports</h1>
                  <p className="text-dark-600 mt-2">Coming soon...</p>
                </div>
              </MainLayout>
            </ProtectedRoute>
          }
        />

        <Route
          path="/profile"
          element={
            <ProtectedRoute>
              <MainLayout>
                <div className="text-center py-12">
                  <h1 className="text-2xl font-bold text-dark-900">Profile</h1>
                  <p className="text-dark-600 mt-2">Coming soon...</p>
                </div>
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

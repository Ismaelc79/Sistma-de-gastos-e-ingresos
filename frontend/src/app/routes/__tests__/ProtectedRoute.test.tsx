import { render, screen } from '@testing-library/react';
import { MemoryRouter, Route, Routes } from 'react-router-dom';
import { ProtectedRoute } from '../ProtectedRoute';

const mockUseAuthStore = vi.fn();
const originalBypass = (import.meta.env as Record<string, string | undefined>).VITE_BYPASS_AUTH;

vi.mock('../../../shared/stores/authStore', () => ({
  useAuthStore: () => mockUseAuthStore(),
}));

describe('ProtectedRoute', () => {
  const renderWithRouter = () =>
    render(
      <MemoryRouter initialEntries={['/dashboard']}>
        <Routes>
          <Route
            path="/dashboard"
            element={
              <ProtectedRoute>
                <div>Private dashboard</div>
              </ProtectedRoute>
            }
          />
          <Route path="/login" element={<div>Login Page</div>} />
        </Routes>
      </MemoryRouter>
    );

  afterEach(() => {
    if (originalBypass === undefined) {
      delete (import.meta.env as Record<string, string>).VITE_BYPASS_AUTH;
    } else {
      (import.meta.env as Record<string, string>).VITE_BYPASS_AUTH = originalBypass;
    }
  });

  it('redirects to login when user is not authenticated', () => {
    mockUseAuthStore.mockReturnValue({ isAuthenticated: false });
    (import.meta.env as Record<string, string>).VITE_BYPASS_AUTH = 'false';

    renderWithRouter();

    expect(screen.getByText(/login page/i)).toBeInTheDocument();
    expect(screen.queryByText(/private dashboard/i)).not.toBeInTheDocument();
  });

  it('renders children when user is authenticated', () => {
    mockUseAuthStore.mockReturnValue({ isAuthenticated: true });
    (import.meta.env as Record<string, string>).VITE_BYPASS_AUTH = 'false';

    renderWithRouter();

    expect(screen.getByText(/private dashboard/i)).toBeInTheDocument();
  });

  it('bypasses auth when env flag is enabled', () => {
    mockUseAuthStore.mockReturnValue({ isAuthenticated: false });
    (import.meta.env as Record<string, string>).VITE_BYPASS_AUTH = 'true';

    renderWithRouter();

    expect(screen.getByText(/private dashboard/i)).toBeInTheDocument();
  });
});

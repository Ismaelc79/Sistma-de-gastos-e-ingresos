import { render, screen } from '@testing-library/react';
import App from './App';

const mockUseAuthStore = vi.fn();

vi.mock('./shared/stores/authStore', () => ({
  useAuthStore: () => mockUseAuthStore(),
}));

vi.mock('./app/routes/AppRoutes', () => ({
  AppRoutes: () => <div data-testid="routes">Routes mounted</div>,
}));

describe('App', () => {
  beforeEach(() => {
    document.documentElement.classList.remove('theme-dark');
    localStorage.clear();
  });

  afterEach(() => {
    mockUseAuthStore.mockReset();
  });

  it('applies user preferred dark theme', () => {
    mockUseAuthStore.mockReturnValue({ user: { theme: 'dark' } });

    render(<App />);

    expect(document.documentElement).toHaveClass('theme-dark');
    expect(screen.getByTestId('routes')).toBeInTheDocument();
  });

  it('removes dark theme when preference is light', () => {
    document.documentElement.classList.add('theme-dark');
    mockUseAuthStore.mockReturnValue({ user: { theme: 'light' } });

    render(<App />);

    expect(document.documentElement).not.toHaveClass('theme-dark');
  });

  it('falls back to localStorage theme when user theme is missing', () => {
    localStorage.setItem('ui-theme', 'dark');
    mockUseAuthStore.mockReturnValue({ user: null });

    render(<App />);

    expect(document.documentElement).toHaveClass('theme-dark');
  });
});

import { useEffect } from 'react';
import { AppRoutes } from './app/routes/AppRoutes';
import { useAuthStore } from './shared/stores/authStore';

function App() {
  const { user } = useAuthStore();

  useEffect(() => {
    const theme = String((user as any)?.theme ?? localStorage.getItem('ui-theme') ?? 'light').toLowerCase();
    const root = document.documentElement;
    if (theme === 'dark') root.classList.add('theme-dark');
    else root.classList.remove('theme-dark');
  }, [user?.theme]);

  return <AppRoutes />;
}

export default App;

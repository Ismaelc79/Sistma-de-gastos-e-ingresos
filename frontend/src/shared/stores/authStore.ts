import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import axios from '../lib/axios';
import type { AuthState, LoginRequest, RegisterRequest, AuthResponse } from '../types/auth.types';

interface AuthStore extends AuthState {
  login: (credentials: LoginRequest) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  logout: () => Promise<void>;
  setUser: (user: AuthState['user']) => void;
  clearError: () => void;
}

export const useAuthStore = create<AuthStore>()(
  persist(
    (set) => ({
      user: null,
      accessToken: null,
      refreshToken: null,
      isAuthenticated: false,
      isLoading: false,
      error: null,

      login: async (credentials: LoginRequest) => {
        set({ isLoading: true, error: null });
        try {
          const response = await axios.post<AuthResponse>(import.meta.env.VITE_API_URL + '/auth/login', credentials);
          const { user, accessToken, refreshToken } = response.data;

          // Store tokens in localStorage
          localStorage.setItem('accessToken', accessToken);
          localStorage.setItem('refreshToken', refreshToken);

          set({
            user,
            accessToken,
            refreshToken,
            isAuthenticated: true,
            isLoading: false,
            error: null,
          });
        } catch (error: any) {
       
          const errorMessage = error.response?.data?.message || 'Login failed';
          set({ isLoading: false, error: errorMessage });
          console.log(error);
          
          throw error;
        }
      },

      register: async (data: RegisterRequest) => {
        set({ isLoading: true, error: null });
        try {
          const response = await axios.post<AuthResponse>('/auth/register', data);
          const { user, accessToken, refreshToken } = response.data;

          // Store tokens in localStorage
          localStorage.setItem('accessToken', accessToken);
          localStorage.setItem('refreshToken', refreshToken);

          set({
            user,
            accessToken,
            refreshToken,
            isAuthenticated: true,
            isLoading: false,
            error: null,
          });
        } catch (error: any) {
          const errorMessage = error.response?.data?.message || 'Registration failed';
          set({ isLoading: false, error: errorMessage });
          throw error;
        }
      },

      logout: async () => {
        try {
          await axios.post('/auth/logout');
        } catch (error) {
          console.error('Logout error:', error);
        } finally {
          // Clear tokens from localStorage
          localStorage.removeItem('accessToken');
          localStorage.removeItem('refreshToken');

          set({
            user: null,
            accessToken: null,
            refreshToken: null,
            isAuthenticated: false,
            error: null,
          });
        }
      },

      setUser: (user) => set({ user }),

      clearError: () => set({ error: null }),
    }),
    {
      name: 'auth-storage',
      partialize: (state) => ({
        user: state.user,
        accessToken: state.accessToken,
        refreshToken: state.refreshToken,
        isAuthenticated: state.isAuthenticated,
      }),
    }
  )
);

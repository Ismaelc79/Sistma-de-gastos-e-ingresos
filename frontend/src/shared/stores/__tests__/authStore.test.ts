import { act } from '@testing-library/react';
import { useAuthStore } from '../authStore';

const postMock = vi.fn();

vi.mock('../../lib/axios', () => ({
  __esModule: true,
  default: {
    post: (...args: any[]) => postMock(...args),
  },
}));

const baseState = {
  user: null,
  accessToken: null,
  refreshToken: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,
};

const resetStore = () => {
  const { login, register, logout, setUser, clearError } = useAuthStore.getState();
  useAuthStore.setState({
    ...baseState,
    login,
    register,
    logout,
    setUser,
    clearError,
  });
};

describe('useAuthStore', () => {
  beforeEach(() => {
    postMock.mockReset();
    localStorage.clear();
    resetStore();
  });

  it('logs in and persists tokens on success', async () => {
    const authResponse = {
      user: { id: 1, name: 'Ada' },
      accessToken: 'access-123',
      refreshToken: 'refresh-456',
    };
    postMock.mockResolvedValue({ data: authResponse });

    await act(async () => {
      await useAuthStore.getState().login({ email: 'ada@example.com', password: 'secret' });
    });

    expect(postMock).toHaveBeenCalledWith('/auth/login', { email: 'ada@example.com', password: 'secret' });
    const state = useAuthStore.getState();
    expect(state.user).toEqual(authResponse.user);
    expect(state.isAuthenticated).toBe(true);
    expect(localStorage.getItem('accessToken')).toBe('access-123');
    expect(localStorage.getItem('refreshToken')).toBe('refresh-456');
  });

  it('captures error message and rethrows on login failure', async () => {
    const error = { response: { data: { message: 'Invalid credentials' } } };
    postMock.mockRejectedValue(error);

    await expect(useAuthStore.getState().login({ email: 'bad', password: 'bad' })).rejects.toEqual(error);

    const state = useAuthStore.getState();
    expect(state.error).toBe('Invalid credentials');
    expect(state.isLoading).toBe(false);
  });

  it('registers user using API and stores credentials', async () => {
    const payload = {
      user: { id: 2, name: 'Grace' },
      accessToken: 'token',
      refreshToken: 'refresh',
    };
    postMock.mockResolvedValue({ data: payload });

    await act(async () => {
      await useAuthStore.getState().register({
        name: 'Grace',
        email: 'g@example.com',
        password: '123456',
        confirmPassword: '123456',
      });
    });

    expect(postMock).toHaveBeenCalledWith('/auth/register', {
      name: 'Grace',
      email: 'g@example.com',
      password: '123456',
      confirmPassword: '123456',
    });
    expect(useAuthStore.getState().isAuthenticated).toBe(true);
  });

  it('logs out and clears tokens even if API fails', async () => {
    localStorage.setItem('accessToken', 'persisted');
    localStorage.setItem('refreshToken', 'persisted');
    useAuthStore.setState({
      ...useAuthStore.getState(),
      user: { id: 1, name: 'Test' } as any,
      accessToken: 'persisted',
      refreshToken: 'persisted',
      isAuthenticated: true,
    });

    postMock.mockRejectedValueOnce(new Error('network'));

    await act(async () => {
      await useAuthStore.getState().logout();
    });

    expect(postMock).toHaveBeenCalledWith('/auth/logout');
    const state = useAuthStore.getState();
    expect(state.user).toBeNull();
    expect(state.isAuthenticated).toBe(false);
    expect(localStorage.getItem('accessToken')).toBeNull();
    expect(localStorage.getItem('refreshToken')).toBeNull();
  });

  it('setUser and clearError helpers update state', () => {
    act(() => {
      useAuthStore.getState().setUser({ id: 10, name: 'Neo' } as any);
    });
    expect(useAuthStore.getState().user?.name).toBe('Neo');

    useAuthStore.setState({ ...useAuthStore.getState(), error: 'boom' });
    act(() => {
      useAuthStore.getState().clearError();
    });
    expect(useAuthStore.getState().error).toBeNull();
  });
});

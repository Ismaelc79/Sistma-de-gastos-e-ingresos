import { updateUser } from './profile.api';

const axiosMock = vi.hoisted(() => ({
  patch: vi.fn(),
}));

vi.mock('../../../shared/lib/axios', () => ({
  __esModule: true,
  default: axiosMock,
}));

describe('profile api', () => {
  beforeEach(() => {
    axiosMock.patch.mockReset();
  });

  it('updates the profile with provided payload', async () => {
    const payload = { name: 'Updated', theme: 'dark' };
    const response = { id: 1, ...payload };
    axiosMock.patch.mockResolvedValue({ data: response });

    const result = await updateUser(payload as any);

    expect(axiosMock.patch).toHaveBeenCalledWith('/users/me', payload);
    expect(result).toEqual(response);
  });
});

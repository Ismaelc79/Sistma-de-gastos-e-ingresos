import { delay, readJSON, writeJSON } from '../mock';

describe('mock helpers', () => {
  it('resolves delay after the provided time', async () => {
    vi.useFakeTimers();
    const promise = delay(500);

    vi.advanceTimersByTime(500);
    await expect(promise).resolves.toBeUndefined();
  });

  it('reads json and falls back when parsing fails', () => {
    localStorage.setItem('valid', JSON.stringify({ value: 1 }));
    expect(readJSON('valid', { value: 0 })).toEqual({ value: 1 });

    localStorage.setItem('invalid', '{not json');
    expect(readJSON('invalid', { fallback: true })).toEqual({ fallback: true });
  });

  it('writes json to localStorage', () => {
    writeJSON('user', { id: 1, name: 'Ada' });
    expect(localStorage.getItem('user')).toBe(JSON.stringify({ id: 1, name: 'Ada' }));
  });
});

export const delay = (ms: number) => new Promise((res) => setTimeout(res, ms));

export function readJSON<T>(key: string, fallback: T): T {
  try {
    const raw = localStorage.getItem(key);
    return raw ? (JSON.parse(raw) as T) : fallback;
  } catch {
    return fallback;
  }
}

export function writeJSON<T>(key: string, value: T) {
  localStorage.setItem(key, JSON.stringify(value));
}

import { useEffect, useState } from 'react';
import { Button, Card, Input } from '../../../shared/ui';
import type { Profile, UpdateProfileInput } from '../../../shared/types/profile.types';
import { delay, readJSON, writeJSON } from '../../../shared/lib/mock';

const USE_MOCKS = import.meta.env.VITE_USE_MOCKS === 'true';
const PROFILE_KEY = 'mock-profile';

async function getProfile(): Promise<Profile> {
  if (USE_MOCKS) {
    await delay(150);
    const fallback: Profile = {
      id: 'me',
      name: 'Guest User',
      email: 'guest@example.com',
      preferredCurrency: 'USD',
      language: 'en',
      theme: 'light',
    };
    return readJSON(PROFILE_KEY, fallback);
  }
  // TODO: replace with axios when backend ready
  throw new Error('Not implemented');
}

async function updateProfile(input: UpdateProfileInput): Promise<Profile> {
  if (USE_MOCKS) {
    await delay(150);
    const current = await getProfile();
    const next = { ...current, ...input } as Profile;
    writeJSON(PROFILE_KEY, next);
    return next;
  }
  throw new Error('Not implemented');
}

export const ProfilePage = () => {
  const [profile, setProfile] = useState<Profile | null>(null);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    getProfile().then(setProfile);
  }, []);

  const handleChange = (field: keyof UpdateProfileInput) => (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setProfile((prev) => prev ? { ...prev, [field]: e.target.value } as Profile : prev);
  };

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!profile) return;
    setSaving(true);
    const updated = await updateProfile({
      name: profile.name,
      phone: profile.phone,
      preferredCurrency: profile.preferredCurrency,
      language: profile.language,
      theme: profile.theme,
      avatarUrl: profile.avatarUrl,
    });
    setProfile(updated);
    setSaving(false);
  };

  if (!profile) return <p className="text-dark-600">Loading...</p>;

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold text-dark-900">Profile</h1>
        <p className="text-dark-600">Manage your account settings</p>
      </div>

      <Card>
        <form onSubmit={handleSave} className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <Input label="Full Name" value={profile.name} onChange={handleChange('name')} required />
          <Input label="Email" value={profile.email} disabled />
          <Input label="Phone" value={profile.phone || ''} onChange={handleChange('phone')} />

          <div>
            <label className="block text-sm font-medium text-dark-700 mb-1">Preferred Currency</label>
            <select className="w-full px-3 py-2 border border-dark-300 rounded-lg" value={profile.preferredCurrency} onChange={handleChange('preferredCurrency')}>
              <option>USD</option>
              <option>EUR</option>
              <option>PEN</option>
              <option>MXN</option>
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-dark-700 mb-1">Language</label>
            <select className="w-full px-3 py-2 border border-dark-300 rounded-lg" value={profile.language} onChange={handleChange('language')}>
              <option value="en">English</option>
              <option value="es">Español</option>
            </select>
          </div>

          <div>
            <label className="block text-sm font-medium text-dark-700 mb-1">Theme</label>
            <select className="w-full px-3 py-2 border border-dark-300 rounded-lg" value={profile.theme} onChange={handleChange('theme')}>
              <option value="light">Light</option>
              <option value="dark">Dark</option>
            </select>
          </div>

          <div className="md:col-span-2 flex items-center gap-3">
            <Button type="submit" isLoading={saving}>Save Changes</Button>
          </div>
        </form>
      </Card>

      <Card>
        <h2 className="text-lg font-semibold text-dark-900 mb-2">Security</h2>
        <p className="text-sm text-dark-600 mb-4">Change your password and manage two-factor authentication</p>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <Input label="New Password" type="password" placeholder="••••••••" />
          <Input label="Confirm Password" type="password" placeholder="••••••••" />
          <div>
            <Button>Update Password</Button>
          </div>
        </div>
      </Card>
    </div>
  );
};

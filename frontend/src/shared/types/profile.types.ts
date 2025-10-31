export interface Profile {
  id: string;
  name: string;
  email: string;
  phone?: string;
  preferredCurrency?: string;
  language?: 'es' | 'en';
  theme?: 'light' | 'dark';
  avatarUrl?: string;
  emailVerified?: boolean;
  phoneVerified?: boolean;
}

export interface UpdateProfileInput {
  name?: string;
  phone?: string;
  preferredCurrency?: string;
  language?: 'es' | 'en';
  theme?: 'light' | 'dark';
  avatarUrl?: string;
}

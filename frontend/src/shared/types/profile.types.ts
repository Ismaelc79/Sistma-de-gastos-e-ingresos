export interface Profile {
  id: string;
  name: string;
  email: string;
  phone?: string;
  currency?: string;
  language?: 'es' | 'en';
  theme?: 'light' | 'dark';
  avatar?: string;
  isVerified?: boolean;
  phoneVerified?: boolean;
  createdAt: string,
  updateAt: string
}

export interface UpdateProfileInput {
  name?: string;
  phone?: string;
  preferredCurrency?: string;
  language?: 'es' | 'en';
  theme?: 'light' | 'dark';
  avatarUrl?: string;
}

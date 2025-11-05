export interface Profile {
  id: string;
  name: string;
  email: string;
  phone?: string;
  currency?: string;
  language?: "es" | "en";
  theme?: "light" | "dark";
  avatar?: string;
  isVerified?: boolean;
  phoneVerified?: boolean;
  createdAt: string;
  updateAt: string;
  password: string,
  newpassword: string,

}


export interface Profile2 {
  avatar: string;
  createdAt: string;
  currency: "DO";
  email: "string";
  id: string;
  isVerified: boolean;
  language: "English" | "Espanish";
  name: string;
  phone: string;
  updatedAt: string;
}

export interface UpdateProfileInput {
  name?: string;
  phone?: string;
  currency?: string;
  language?: "es" | "en" | string;
  theme?: "light" | "dark";
  avatar?: string;
}

export interface ProfileInput {
  name: string;
  phone: string;
  currency: string;
  language: string;
  avatar: string;
  password: string,
  newpassword: string,

}

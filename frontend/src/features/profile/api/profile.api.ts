
import axiosInstance from "../../../shared/lib/axios";
import type { Profile, UpdateProfileInput } from "../../../shared/types/profile.types";




export async function updateUser(input: UpdateProfileInput): Promise<Profile> {
  const { data } = await axiosInstance.patch("/users/me", input);
  return data;
}

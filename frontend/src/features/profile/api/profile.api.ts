
import axiosInstance from "../../../shared/lib/axios";
import type {
  Profile,
  ProfileInput,
} from "../../../shared/types/profile.types";




export async function updateUser(input: ProfileInput): Promise<Profile> {
  const { data } = await axiosInstance.patch("/users/me", input);
  return data;
}

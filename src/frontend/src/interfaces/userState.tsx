import { UserInfo } from "./userInfo";

export interface UserState {
  loading?: boolean;
  users: Array<UserInfo>;
  index?: number;
}

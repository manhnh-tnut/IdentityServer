import { Action, Reducer } from "redux";
import * as actionTypes from "../actions/type";
import { UserState } from "../interfaces/userState";

type INCOMING_ACTION =
  | actionTypes.FETCH_USER
  | actionTypes.FETCH_USER_SUCCESS;

export const userReducer: Reducer<UserState> = (
  state: UserState | undefined,
  incomingAction: Action
): UserState => {
  if (state === undefined) {
    return { users: [], loading: false };
  }

  const action = incomingAction as INCOMING_ACTION;
  switch (action.type) {
    case "fetch_user":
      return {
        index: state.index,
        loading: true,
        users: [],
      };

    case "fetch_user_success":
      return {
        index: state.index,
        loading: false,
        users: action.payload,
      };

    default:
      return state;
  }
};

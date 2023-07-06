import { Action, Reducer } from "redux";
import * as actionTypes from "../actions/type";

type INCOMING_ACTION =
  | actionTypes.FETCH_USER
  | actionTypes.FETCH_USER_SUCCESS
  | actionTypes.CANCEL_FETCHING_USER;

export const loadingReducer: Reducer<boolean> = (
  state: boolean | undefined,
  incomingAction: Action
): boolean => {
  if (state === undefined) {
    return false;
  }
  const action = incomingAction as INCOMING_ACTION;
  switch (action.type) {
    case "fetch_user":
      return true;

    case "fetch_user_success":
    case "cancel_fetching_user":
      return false;
    default:
      return state;
  }
};

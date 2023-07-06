import * as actionTypes from "./type";

export const counterActionCreators = {
  increment: () => ({ type: "increase" } as actionTypes.INCREMENT),
  decrement: () => ({ type: "decrease" } as actionTypes.DECREASE),
  stop: () => ({ type: "stop_counter" } as actionTypes.STOP_COUNTER),
};

export const fetchUsersActionCreators = {
  fetchUsers: (index: number) =>
    ({ type: "fetch_user" } as actionTypes.FETCH_USER),
  cancelRequest: () =>
    ({ type: "cancel_fetching_user" } as actionTypes.CANCEL_FETCHING_USER),
};

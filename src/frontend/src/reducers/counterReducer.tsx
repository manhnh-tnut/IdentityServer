import { Action, Reducer } from "redux";
import * as actionTypes from "../actions/type";
import { CounterState } from "../interfaces/counterState";

type INCOMING_ACTION =
  | actionTypes.INCREMENT
  | actionTypes.DECREASE
  | actionTypes.INCREASE_DONE
  | actionTypes.STOP_COUNTER;

export const counterReducer: Reducer<CounterState> = (
  state: CounterState | undefined,
  incomingAction: Action
): CounterState => {
  if (state === undefined) {
    return { count: 0 };
  }
  const action = incomingAction as INCOMING_ACTION;
  switch (action.type) {
    case "increase_done":
      return { count: state.count + 1 };

    case "decrease":
      return { count: state.count - 1 };

    default:
      return state;
  }
};

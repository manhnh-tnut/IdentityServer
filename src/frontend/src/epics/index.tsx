import { combineEpics } from "redux-observable";
import { counterEpic } from "./counterEpic";
import { fetchUsersEpic } from "./fetchUsersEpic";

export default combineEpics(counterEpic, fetchUsersEpic);

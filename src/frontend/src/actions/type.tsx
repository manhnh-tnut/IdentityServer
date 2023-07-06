export interface INCREMENT {
  type: "increase";
}
export interface DECREASE {
  type: "decrease";
}
export interface INCREASE_DONE {
  type: "increase_done";
}
export interface STOP_COUNTER {
  type: "stop_counter";
}
export interface FETCH_USER {
  type: "fetch_user";
}
export interface FETCH_USER_SUCCESS {
  payload: any[];
  type: "fetch_user_success";
}
export interface SHOW_LOADING_ICON {
  type: "show_loading_icon";
}
export interface HIDE_LOADING_ICON {
  type: "hide_loading_icon";
}
export interface CANCEL_FETCHING_USER {
  type: "cancel_fetching_user";
}

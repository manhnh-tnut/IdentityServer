import { userReducer } from "./userReducer";
import { counterReducer } from "./counterReducer";
import { loadingReducer } from "./loadingReducer";

export default {
  user: userReducer,
  counter: counterReducer,
  loading: loadingReducer,
};

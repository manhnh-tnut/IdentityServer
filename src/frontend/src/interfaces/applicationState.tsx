import { CounterState } from "./counterState";
import { UserState } from "./userState";

export interface ApplicationState {
    counter: CounterState | undefined;
    user: UserState | undefined;
}
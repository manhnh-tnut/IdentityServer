import * as actionTypes from "../actions/type";
import { timer } from "rxjs";
import { map, switchMap, takeUntil } from "rxjs/operators";
import { ofType } from "redux-observable";

export const counterEpic = (action$: any) =>
  action$.pipe(
    ofType(({ type: "increase" } as actionTypes.INCREMENT).type),
    switchMap((action) => {
      console.log("counterEpic", action);
      return timer(0, 100).pipe(
        takeUntil(timer(1000)),
        map((response: any) => ({
          type: ({ type: "increase_done" } as actionTypes.INCREASE_DONE).type,
        })),
        takeUntil(
          action$.pipe(
            ofType(({ type: "stop_counter" } as actionTypes.STOP_COUNTER).type)
          )
        )
      );
    })
  );

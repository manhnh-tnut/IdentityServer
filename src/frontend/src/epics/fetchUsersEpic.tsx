import * as actionTypes from "../actions/type";
import { of, map } from "rxjs";
import { catchError, delay, mergeMap, takeUntil } from "rxjs/operators";
import { ofType } from "redux-observable";

const fakeApi = (time: number) =>
  of([
    {
      userId: 1,
      name: "chris",
      position: "Front-end",
      email: "chris.ho@innovatube.com",
    },
  ]).pipe(delay(time));

export const fetchUsersEpic = (action$: any) =>
  action$.pipe(
    ofType(({ type: "fetch_user" } as actionTypes.FETCH_USER).type),
    mergeMap((action) =>
      fakeApi(2000).pipe(
        map((response: any) => ({
          type: (
            { type: "fetch_user_success" } as actionTypes.FETCH_USER_SUCCESS
          ).type,
          payload: response,
        })),
        takeUntil(
          action$.pipe(
            ofType(
              (
                {
                  type: "cancel_fetching_user",
                } as actionTypes.CANCEL_FETCHING_USER
              ).type
            )
          )
        ),
        catchError((error) =>
          of({
            type: (
              {
                type: "cancel_fetching_user",
              } as actionTypes.CANCEL_FETCHING_USER
            ).type,
            payload: error.xhr.response,
            error: true,
          })
        )
      )
    )
  );

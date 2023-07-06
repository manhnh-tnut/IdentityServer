import rootEpic from "../epics";
import reducers from "../reducers";
import { createStore, applyMiddleware, compose, combineReducers } from "redux";
import { createEpicMiddleware } from "redux-observable";
import { composeWithDevTools } from "redux-devtools-extension";
import { connectRouter, routerMiddleware } from "connected-react-router";
import { History } from "history";
import { ApplicationState } from "../interfaces/applicationState";

// import createSagaMiddleware from 'redux-saga';
// import rootSaga from '../sagas';

// const sagaMiddleware = createSagaMiddleware();
// const store = createStore(
//     reducers,
//     applyMiddleware(sagaMiddleware)
// );
// sagaMiddleware.run(rootSaga);

// const epicMiddleware = createEpicMiddleware();

// const store = createStore(
//   rootReducers,
//   composeWithDevTools(applyMiddleware(epicMiddleware))
// );
// epicMiddleware.run(rootEpic);

export default function configureStore(history: History, initialState?: ApplicationState) {
  const epicMiddleware = createEpicMiddleware();
  const middleware = [epicMiddleware, routerMiddleware(history)];
  const rootReducer = combineReducers({
    ...reducers,
    router: connectRouter(history),
  });

  const enhancers = [];
  const windowIfDefined =
    typeof window === "undefined" ? null : (window as any); // eslint-disable-line @typescript-eslint/no-explicit-any
  if (windowIfDefined && windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__) {
    enhancers.push(windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__());
  }

  const store = createStore(
    rootReducer,
    initialState,
    compose(composeWithDevTools(applyMiddleware(...middleware)), ...enhancers)
  );
  epicMiddleware.run(rootEpic);
  return store;
}

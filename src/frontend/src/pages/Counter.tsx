import * as React from "react";
import { connect } from "react-redux";

import * as actions from "../actions";
import { ApplicationState } from "../interfaces/applicationState";
import { CounterState } from "../interfaces/counterState";

type CounterProps = CounterState & typeof actions.counterActionCreators;

class Counter extends React.PureComponent<CounterProps> {
  public render() {
    return (
      <React.Fragment>
        <h1>Counter</h1>

        <p>This is a simple example of a React component.</p>

        <p aria-live="polite">
          Current count: <strong>{this.props.count}</strong>
        </p>

        <button
          type="button"
          className="btn btn-primary btn-lg"
          onClick={() => {
            this.props.increment();
          }}
        >
          Increment
        </button>
        <button
          type="button"
          className="btn btn-primary btn-lg"
          onClick={() => {
            this.props.decrement();
          }}
        >
          Decrement
        </button>
      </React.Fragment>
    );
  }
}

const mapStateToProps = (state: ApplicationState) => state.counter;
export default connect(mapStateToProps, actions.counterActionCreators)(Counter);

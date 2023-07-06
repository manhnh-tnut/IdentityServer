import * as React from "react";
import { connect } from "react-redux";
import { Link } from "react-router-dom";

import * as actions from "../actions";
import { UserState } from "../interfaces/userState";
import { UserInfo } from "../interfaces/userInfo";
import { ApplicationState } from "../interfaces/applicationState";

type FetchDataProps = UserState & typeof actions.fetchUsersActionCreators;

class FetchData extends React.PureComponent<FetchDataProps> {
  fetchUsers = (index: number) => this.props.fetchUsers(index);
  cancelRequest = () => this.props.cancelRequest();
  // This method is called when the component is first added to the document
  public componentDidMount() {
    this.ensureDataFetched();
  }

  public render() {
    return (
      <React.Fragment>
        <h1 id="tabelLabel">Users</h1>
        <p>
          This component demonstrates fetching data from the server and working
          with URL parameters.
        </p>
        {this.renderTable()}
        {this.renderPagination()}
      </React.Fragment>
    );
  }

  private ensureDataFetched() {
    this.fetchUsers(this.props.index || 0);
  }

  private renderTable() {
    return (
      <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Email</th>
            <th>Position</th>
          </tr>
        </thead>
        <tbody>
          {this.props.users.map((user: UserInfo) => (
            <tr key={user.userId}>
              <td>{user.name}</td>
              <td>{user.email}</td>
              <td>{user.position}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  }

  private renderPagination() {
    const previndex = (this.props.index || 0) - 5;
    const nextindex = (this.props.index || 0) + 5;

    return (
      <div className="d-flex justify-content-between">
        <Link
          className="btn btn-outline-secondary btn-sm"
          to={`/fetch-data/${previndex}`}
        >
          Previous
        </Link>
        {this.props.loading && <span>Loading...</span>}
        <Link
          className="btn btn-outline-secondary btn-sm"
          to={`/fetch-data/${nextindex}`}
        >
          Next
        </Link>
      </div>
    );
  }
}

const mapStateToProps = (state: ApplicationState) => state.user;

export default connect(
  mapStateToProps,
  actions.fetchUsersActionCreators
)(FetchData);

import React from "react";
import { Route, Routes } from "react-router-dom";
import Counter from "../pages/Counter";
import FetchData from "../pages/FetchData";
import Home from "../pages/Home";

export default class RootRoutes extends React.PureComponent<{}, {}> {
  public render() {
    return (
      <React.Fragment>
        <Routes>
          <Route path="/" element={Home} />
          <Route path="/counter" element={Counter} />
          <Route path="/fetch-data/:index?" element={FetchData} />
        </Routes>
      </React.Fragment>
    );
  }
}

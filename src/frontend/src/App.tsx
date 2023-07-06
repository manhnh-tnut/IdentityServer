import * as React from "react";
import { Route } from "react-router";
import Layout from "./components/Layout";
import Home from "./pages/Home";
import Counter from "./pages/Counter";
import FetchData from "./pages/FetchData";

import "./custom.css";

export default () => (
  <Layout>
    <Route path="/" element={Home} />
    <Route path="/counter" element={Counter} />
    <Route path="/fetch-data/:index?" element={FetchData} />
  </Layout>
);

import React, { useState, useContext, useMemo } from "react";
import { Router, Route, Switch, NavLink } from "react-router-dom";
import * as createHistory from "history";
import Login from "../components/Login";
import Layout from "../components/SharedLayout";
import Frontpage from "../components/Frontpage";
import DomainForm from "../components/DomainForm";
import { UserContext } from "../components/UserContext";

/**
 * Authors: Ali Asbai, Anna Mann, Clara Hällgren, Gabriel Vega, Olivia Höft
 * 
 * Instead of BrowserRouter, we use the regular router,
 * but we pass in a customer history to it.
 * */

export const history = createHistory.createBrowserHistory();

function AppRouter(){
    const [user, setUser] = useState(sessionStorage.getItem('User'));
    const providerValue = useMemo(() => ({ user, setUser }), [user, setUser]);

    return (
        <UserContext.Provider value={providerValue}>
          <Router history={history}>
              <div className="App">

                  <Layout />

                  <Switch>
                      <Route exact path="/" component={Login} />
                      <Route path="/Princess" component={Frontpage} />
                      <Route path="/Domain" component={DomainForm} />
                  </Switch>

              </div>
            </Router>
        </UserContext.Provider>
    );
}

export default AppRouter;

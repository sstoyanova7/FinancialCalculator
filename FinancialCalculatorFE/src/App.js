import React from "react";
//importing components

import WelcomePage from "./pages/WelcomePage"
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import LeasCalcPage from './pages/LeasingCalculator'
import {
  BrowserRouter as Router,
  Switch,
  Route
} from "react-router-dom";

function App() {
  return (
      <Router>
        <Switch>

        <Route exact path="/" component={WelcomePage} />
        <Route path="/login" component={LoginPage} />
        <Route path="/register" component={RegisterPage} />
        <Route path="/leas" component={LeasCalcPage} />
       
        </Switch>
      </Router>
  );
}

export default App;

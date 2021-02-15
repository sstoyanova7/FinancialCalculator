import React from 'react';
import { HashRouter as Router, Switch, Route } from 'react-router-dom';
import Navbar from "./Navbar";
import CalculatorCredit from "./CalculatorCredit";
import CalculatorLizing from "./CalculatorLizing";
import CalculatorRefinance from "./CalculatorRefinance";
import CreditHistory from "./CreditHistory";
import LeasingHistory from "./LeasingHistory";
import RefinanceHistory from "./RefinanceHistory";
import AboutUs from "./AboutUs";
import Home from "./Home";
import Calculators from "./Calculators";
import axios from 'axios';
import Logout from './Logout';
import History from './History'

class App extends React.Component {
  constructor(props) {
    super(props);
      this.state = {
          cookie: "",
          user: ""
    }
  }

    loadCookie = (user) => {
        this.setState({
            cookie: document.cookie,
            user: user
        })
    }

    onLogoutSubmit = () => {
      axios({
          method: "post",
          url: "http://localhost:5000/api/Authentication/logout"
      }).then(res => {
        this.loadCookie();
        
      })
  }

    componentDidMount() {
        if (document.cookie) {
            this.setState({
                cookie: document.cookie
            })
        } else {
            this.setState({
                cookie: ""
            })
        }
    }

    render() {
    return (
      <div>
        <Router>
        <Route
              path="/" 
              render={(props) => (
                  <Navbar {...props}  user={this.state.user} cookie={this.state.cookie}/>
              )}
            />
          {/* <Route path="/" component={Navbar} cookie={this.state.cookie}/> */}
          <Switch>
          <Route
              path="/history/credit" exact
              render={(props) => (
                  <CreditHistory {...props} cookie={this.state.cookie}/>
              )}
            />
            <Route
              path="/history/leasing" exact
              render={(props) => (
                  <LeasingHistory {...props} cookie={this.state.cookie}/>
              )}
            />
            <Route
              path="/history/refinance" exact
              render={(props) => (
                  <RefinanceHistory {...props} cookie={this.state.cookie}/>
              )}
            />
            <Route path="/calculators/credit" cookie={this.state.cookie} exact component={CalculatorCredit} />
            <Route path="/calculators/lizing" cookie={this.state.cookie} exact component={CalculatorLizing} />
            <Route path="/calculators/refinance" cookie={this.state.cookie} exact component={CalculatorRefinance} />
            
           
            <Route
              path="/about-us" exact
              render={(props) => (
                  <AboutUs {...props} cookie={this.state.cookie} userName={this.state.user} logout={this.onLogoutSubmit}/>
              )}
            />
             <Route
              path="/history" exact
              render={(props) => (
                  <History {...props} cookie={this.state.cookie}/>
              )}
            />
             <Route
              path="/calculators" exact
              render={(props) => (
                  <Calculators {...props}/>
              )}
            />
            <Route
            
              path="/logout" exact
              render={(props) => (
                  <Logout {...props} cookie={this.state.cookie} logout={this.onLogoutSubmit}/>
              )}
            />
            {/* <Route path="/about-us" exact component={AboutUs} /> */}
            {/* <Route path="/" exact component={Home userInfo={5}} /> */}
            <Route
              path="/" exact
              render={(props) => (
                  <Home {...props}  loadCookie={this.loadCookie }/>
              )}
            />
          </Switch>
        </Router>
      </div>
    );
  }
}

export default App;

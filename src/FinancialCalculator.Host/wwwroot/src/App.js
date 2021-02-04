import React from 'react';
import { HashRouter as Router, Switch, Route } from 'react-router-dom';
import Navbar from "./Navbar";
import CalculatorCredit from "./CalculatorCredit";
import CalculatorLizing from "./CalculatorLizing";
import CalculatorRefinance from "./CalculatorRefinance";
import StatisticsOne from "./StatisticsOne";
import StatisticsTwo from "./StatisticsTwo";
import StatisticsThree from "./StatisticsThree";
import AboutUs from "./AboutUs";
import Home from "./Home";
import Calculators from "./Calculators";

class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      userData: {
        email: "",
        profileName: "Ekaterina",
        password: ""
      }
    }

  }

  render() {
    return (
      <div>
        <Router>
          <Route path="/" component={Navbar} />
          <Switch>
            <Route path="/calculators/credit" exact component={CalculatorCredit} />
            <Route path="/calculators/lizing" exact component={CalculatorLizing} />
            <Route path="/calculators/refinance" exact component={CalculatorRefinance} />
            <Route path="/calculators" exact component={Calculators} />
            <Route path="/statistics/one" exact component={StatisticsOne} />
            <Route path="/statistics/two" exact component={StatisticsTwo} />
            <Route path="/statistics/three" exact component={StatisticsThree} />
            <Route path="/about-us" exact component={AboutUs} />
            {/* <Route path="/" exact component={Home userInfo={5}} /> */}
            <Route
              path="/" exact
              render={(props) => (
                <Home {...props} userInfo={{profileName: this.state.userData.profileName}} />
              )}
            />
          </Switch>
        </Router>
      </div>
    );
  }
}

export default App;

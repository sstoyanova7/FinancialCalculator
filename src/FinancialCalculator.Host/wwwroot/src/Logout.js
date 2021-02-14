import React from 'react';
import './Pages.css';

class Logout extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            message: ""
        }
    }

    componentDidMount() {
        if (this.props.cookie !== "") {
            this.setState({ message: "До нови срещи!" })
            setTimeout( () => {
                    this.setState({ message: "" });
                    this.props.history.push(('/calculators'));
                },
                3000
            );
            
        }
        this.props.logout();
    }

    render() {
        return (
            <div>
                <h1>{this.state.message}</h1>
            </div>
        );
    }
}

export default Logout;

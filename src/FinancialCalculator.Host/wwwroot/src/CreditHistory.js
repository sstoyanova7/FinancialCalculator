import React from 'react';
import './Pages.css';
import axios from 'axios';

class CreditHistory extends React.Component {
    constructor(props) {
        super(props);
    }


    componentDidMount() {
      if(this.props.cookie !== "") {
        axios({
            method: 'get',
            url: 'http://localhost:5000/api/RequestHistory/?type=credit'
        }).then(res => {
            console.log(res);
        }).catch(err => {
            console.log(err);
        })
      } 
    }

    render() {
        if(this.props.cookie !== "") {
            return (
                <div className = "page">
                    <h1>кредитно търсене</h1>
                </div>
            );
        } else {
            return <div><p>not credit</p></div>;
        }
    }
}

export default CreditHistory;

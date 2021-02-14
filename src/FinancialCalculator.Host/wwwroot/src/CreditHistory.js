import React from 'react';
import './Pages.css';
import axios from 'axios';

class CreditHistory extends React.Component {
    constructor(props) {
        super(props);
    }


    componentDidMount() {
        axios({
            method: 'post',
            url: 'http://localhost:5000/api/RequestHistory/?type=credit'
        }).then(res => {
            console.log(res);
        }).catch(err => {
            console.log(err);
        })
    }

    render() {
        return (
            <div className = "page">
                <h1>кредитно търсене</h1>
            </div>
        );
    }
}

export default CreditHistory;
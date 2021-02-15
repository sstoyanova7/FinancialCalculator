import React from 'react';
import './Pages.css';
import axios from 'axios';

class LeasingHistory extends React.Component {
    constructor(props) {
        super(props);
    }

    componentDidMount() {
       if(this.props.cookie !== "") {
        axios({
            method: 'get',
            url: 'http://localhost:5000/api/RequestHistory/?type=leasing'
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
                <h1>Лизинг</h1>
            </div>
        );
       } else {
           return <div><p>not leasing</p></div>
       }
    }
}

export default LeasingHistory;

import React from 'react';
import './Pages.css';
import axios from 'axios';

class LeasingHistory extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            result: []
        };
    }

    componentDidMount() {
        if (this.props.cookie !== "") {
            axios({
                method: 'get',
                url: 'http://localhost:5000/api/RequestHistory/?type=leasing'
            }).then(res => {
                const resArrayData = [...res.data];
                const stateArray = [];
                console.log(res.data);
                resArrayData.forEach(data => {
                    let dataLength = data['calculation_Result'].length;
                    let posResult = data['calculation_Result'].indexOf('Result:');
                    let posInput = data['calculation_Result'].indexOf('Input:');
                    let subStringResult = data['calculation_Result'].substring(posResult + 'Result:'.length , dataLength);
                    let subStringInput = data['calculation_Result'].substring(posInput + 'Input:'.length , posResult);
                    stateArray.push({
                        ['id']: data['id'],
                        ['data']: subStringResult,
                        ['input']: subStringInput

                    })
                })
                this.setState({
                    result: stateArray
                })
                console.log(this.state.result);

            }).catch(err => {
                console.log(err);
            })
        }
    }

    componentDidUpdate() {
        if (this.props.cookie !== "") {
            axios({
                method: 'get',
                url: 'http://localhost:5000/api/RequestHistory/?type=leasing'
            }).then(res => {
                const resArrayData = [...res.data];
                const stateArray = [];
                resArrayData.forEach(data => {
                    let dataLength = data['calculation_Result'].length;
                    let posResult = data['calculation_Result'].indexOf('Result:');
                    let posInput = data['calculation_Result'].indexOf('Input:');
                    let subStringResult = data['calculation_Result'].substring(posResult + 'Result:'.length , dataLength);
                    let subStringInput = data['calculation_Result'].substring(posInput + 'Input:'.length , posResult);
                    stateArray.push({
                        ['id']: data['id'],
                        ['data']: subStringResult,
                        ['input']: subStringInput

                    })
                })
                this.setState({
                    result: stateArray
                })
                console.log(this.state.result);

            }).catch(err => {
                console.log(err);
            })
        }
    }
    render() {
        if (this.props.cookie !== "") {
            return (
                <div className="container">
                    <div className="data-wrapper">
                        {this.state.result.map(data => {
                            return (
                                <div className="data">
                                    <div className="data-info">
                                        <p className="data-text"><span className="heading-m">id:</span>  {data.id}</p>
                                    </div>
                                    <div className="data-info">
                                        <p className="data-text"><span className="heading-m">input:</span> <br />{data.input}</p>
                                    </div>
                                    <div className="data-info">
                                        <p className="data-text"><span className="heading-m">result:</span> <br />{data.data}</p>
                                    </div>
                                </div>
                            )
                        })}
                    </div>
                </div>
            );
        } else {
            return <div><p>not leasing</p></div>
        }
    }
}

export default LeasingHistory;

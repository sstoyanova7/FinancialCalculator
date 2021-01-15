import React, { Component } from 'react';
import LeasingCalculator from '../components/LeasingCalculator/LeasingCalculator'
import { canSubmit, checkIfNumber, isEmpty } from '../exceptions/validations'
import axios from 'axios';
class LeasCalcPage extends Component {

    constructor() {
        super()
        this.isNumber = false

        this.state = {
            ProductPrice: "",
            StartingInstallment: "",
            Period: "",
            MonthlyInstallment: "",
            StartingFeeValue: "",
            StartingFeeValueType: 0,
            StartingFeeType: "",
            anualPercentExpense: "300",
            totalPaidWithFees: "100",
            totalFees: "200"
        }
    }
    handleChange = (event) => {
        const { name, value, style } = event.target
        this.isNumber = checkIfNumber(value)
        this.setState({
            [name]: value,
        })
        style.borderColor = this.isNumber ? '' : 'red'

    }

    handleSubmit = (event) => {

        event.preventDefault();
        if (!canSubmit(event) && !isEmpty(event)) {
            let information = {
                'UserAgent': "Mozilla",
                'ProductPrice': parseFloat(this.state.ProductPrice),
                'StartingInstallment': parseFloat(this.state.StartingInstallment),
                'Period': parseFloat(this.state.Period),
                'MonthlyInstallment': parseFloat(this.state.MonthlyInstallment),
                'StartingFee': {
                    'Type': 1,
                    'Value': parseFloat(this.state.StartingFeeValue),
                    'ValueType': this.state.StartingFeeValueType
                }
            }
            axios({
                method: 'post',
                url: '/FinancialCalculator/api/calculateLeasingLoan',
                data: {
                     ...information 
                }   
            }).then(res => { console.log(res.data) }).catch(err => { console.log(err) })
        } else {
            console.log('err')
        }
    }

    render() {


        return (

            <LeasingCalculator
                handleChange={this.handleChange}
                handleSubmit={this.handleSubmit}
                {...this.state}
            />
        )
    }
}

export default LeasCalcPage;
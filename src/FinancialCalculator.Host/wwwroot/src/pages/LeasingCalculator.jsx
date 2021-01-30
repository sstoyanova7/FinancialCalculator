import React, { Component } from 'react';
import LeasingCalculator from '../components/LeasingCalculator/LeasingCalculator'
import { validate } from '../exceptions/validations'
import axios from 'axios';
class LeasCalcPage extends Component {

    constructor() {
        super()


        this.state = {
            errors: [],
            isVisible: "none",
            ProductPrice: "",
            StartingInstallment: "",
            Period: "",
            MonthlyInstallment: "",
            StartingFeeValue: "",
            StartingFeeValueType: 0,
            StartingFeeType: "",

            responseInformation: {
                statusCodeText: "",
                statusCode: "",
                anualPercentExpense: "",
                totalCost: "",
                totalFees: ""
            }

        }
    }
    handleChange = (event) => {
        const { name, value, style } = event.target


        this.setState({
            [name]: value,
        })
        style.border = ''



    }



    handleSubmit = (event) => {

        event.preventDefault();

        const errors = validate(event);

        if (errors.length > 0) {
            this.setState({ errors, isVisible: '' })
        } else {
            this.setState({ isVisible: 'none' })
            const information = {
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
            }).then(res => {

                console.log(res.data)
                this.setState({
                    ...{
                        responseInformation: {
                            statusCodeTest: res.statusText,
                            statusCode: res.data['status'],
                            anualPercentExpense: res.data['annualPercentCost'],
                            totalCost: res.data['totalCost'],
                            totalFees: res.data['totalFees']
                        }
                    }
                })

            }).catch(err => { console.log(err) })
        }
        console.log(errors)


        //if (!canSubmit(event) && !isEmpty(event)) {


        //} else {
        //    console.log('err')
        //}
    }


    render() {


        return (

            <LeasingCalculator
                handleChange={this.handleChange}
                handleSubmit={this.handleSubmit}
                handleClick={this.handleClick}
                isVisible={this.state.isVisible}
                errorMessages={this.state.errors}
                {...this.state.responseInformation}
                {...this.state}
            />
        )
    }
}

export default LeasCalcPage;
import React, {Component} from 'react';
import LeasingCalculator from '../components/LeasingCalculator/LeasingCalculator'
import {canSubmit, checkIfNumber, isEmpty} from '../exceptions/validations'
import axios from 'axios';
class LeasCalcPage extends Component {

    constructor() {
        super()
        this.isNumber = false

        this.state = {
            valGoods: "",
            downPayment: "",    
            leaseTerm: "",
            monthlyPayment: "",
            initProcessingFee: "",
            anualPercentExpense: "300",
            totalPaidWithFees: "100",
            totalFees: "200"
        }
    }
    handleChange = (event) => {
        const {name, value, style} = event.target
        this.isNumber = checkIfNumber(value)  
        this.setState({
            [name] : value,
        })
        style.borderColor = this.isNumber ? '' : 'red'

    }

    handleSubmit = (event) => {
        const leasCalcInformation = {
            anualPercentExpense: this.state.anualPercentExpense,
            totalPaidWithFees: this.state.totalPaidWithFees,
            totalFees: this.state.totalFees
        }
        event.preventDefault();
        axios.get("/FinancialCalculator/api/test", {headers: {Hi: "Georgi"}});
        if(!canSubmit(event) && !isEmpty(event)) {
            console.log({...this.state})
            console.log(leasCalcInformation)
        }else {
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
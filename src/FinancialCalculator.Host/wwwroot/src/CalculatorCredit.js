import React from 'react';
import './Pages.css';
import "@ui5/webcomponents/dist/Card";
import "@ui5/webcomponents/dist/List.js";
import "@ui5/webcomponents/dist/StandardListItem.js";
import "@ui5/webcomponents/dist/CustomListItem.js";
import "@ui5/webcomponents/dist/Input.js";
import "@ui5/webcomponents/dist/Button";
import "@ui5/webcomponents/dist/Table.js";
import "@ui5/webcomponents/dist/TableColumn.js";
import "@ui5/webcomponents/dist/TableRow.js";
import "@ui5/webcomponents/dist/TableCell.js";
import "@ui5/webcomponents/dist/Title";
import "@ui5/webcomponents/dist/Select";
import "@ui5/webcomponents/dist/Option";
import axios from 'axios';
import Notes from './Notes';

class CalculatorCredit extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loanAmount: "",
            period: "",
            installmentType: "0",
            interest: "",
            // Promo Period
            promoPeriod: "",
            promoInterest: "",
            gracePeriod: "",
            // Fees
            feeType: "",
            // StartingFees
            аpplicationFee: "",
            applicationFeeValueType: "1",
            processingFee: "",
            processingFeeValueType: "1",
            otherFees: "",
            otherFeesValueType: "1",
            // Annual Fees
            annualManagementFee: "",
            annualManagementFeeValueType: "1",
            otherAnnualFees: "",
            otherAnnualFeesValueType: "1",
            // Monthly Fees
            monthlyManagementFee: "",
            monthlyManagementFeeValueType: "1",
            otherMonthlyFees: "",
            otherMonthlyFeesValueType: "1"
        }
        this.calculateCreditButtonRef = React.createRef();
    }

    inputsOnChange = (event) => {
        const { id, value } = event.target;
        this.setState({
            [id]: value
        })
    }

    onSelectChange = (event) => {
        const { value } = event.detail.selectedOption
        this.setState({
            installmentType: value
        })
    }


    onCalculateCredit = (event) => {
        //Функция за Валидация на state
        //Input полетата да се маркират червени с грешката
        //research -> как да се имплементират съобщенията за грешни input и... дали с масив от хардкоднати грешки.
        //Функция за добавяне на грешките в input ите
        let userAgent = navigator.userAgent;
        const feesValue = ['аpplicationFee', 'processingFee', 'otherFees', 'annualManagementFee',
            'otherAnnualFees', 'monthlyManagementFee', 'otherMonthlyFees'];
        const feeValueTypes = [
            'applicationFeeValueType', 'processingFeeValueType', 'otherFeesValueType', 'annualManagementFeeValueType',
            'otherAnnualFeesValueType', 'monthlyManagementFeeValueType', 'otherMonthlyFeesValueType'];
        const promoPeriod = ["promoPeriod", "promoInterest", "gracePeriod"];
        let postInformation = {
            "userAgent": userAgent,
            "loanAmount": parseFloat(this.state.loanAmount),
            "period": parseInt(this.state.period),
            "interest": parseFloat(this.state.interest),
            "installmentType": parseInt(this.state.installmentType)
        };
        let fees = [];
        for (let i = 0; i < promoPeriod.length; i++) {
            if (this.state[promoPeriod[i]]) {
                postInformation[promoPeriod[i]] = parseFloat(this.state[promoPeriod[i]]);
            }
        }
        for (let i = 0; i < feesValue.length; i++) {
            if (this.state[feesValue[i]]) {
                fees.push({
                    ["type"]: parseInt(i),
                    ["value"]: parseFloat(this.state[feesValue[i]]),
                    ["valueType"]: parseInt(this.state[feeValueTypes[i]])
                })
            }
        }
        if (fees.length !== 0) {
            postInformation['fees'] = fees
        }
       
        axios({
            method: 'post',
            url: '/FinancialCalculator/api/calculateNewLoan',
            data: {
                ...postInformation
            }
        }).then(res => {
            
        }).then(err => {
            console.log(err);
        })
        

    }

    onFeeTypeValueChange = (event) => {
        const name = event.target.id;
        const feeValue = event.target.value;
        this.setState({
            [name]: feeValue
        })
    }

    addEventListeners() {
        this.calculateCreditButtonRef.current.addEventListener("click", this.onCalculateCredit);
        const inputs = document.querySelectorAll("#annual-percentage-of-fees, #promo-period, #fees");
        const inputsArray = [...inputs];
        inputsArray.forEach(input => {
            input.addEventListener("input", this.inputsOnChange);
        })
        const selectInstallment = document.getElementById("select-installment");
        selectInstallment.addEventListener("change", this.onSelectChange);
        const selectFeeTypeValue = document.querySelectorAll(
            "#applicationFeeValueType, #processingFeeValueType, \
            #otherFeesValueType, #annualManagementFeeValueType, \
            #otherAnnualFeesValueType, #monthlyManagementFeeValueType, #otherMonthlyFeesValueType "
        );
        const feeTypeValueArray = [...selectFeeTypeValue];
        feeTypeValueArray.forEach(valueType => {
            valueType.addEventListener("change", this.onFeeTypeValueChange);
        });
    }

    componentDidMount() {
        this.addEventListeners();
    }

    render() {
        return (
            <div>
                <div className="page">
                    <div className="sub-page">
                        <ui5-card heading="Кредитен калкулатор" subheading="Пресметнете месечни вноски и ГПР (годишен процент на разходите)" class="small">
                            <div id="annual-percentage-of-fees">
                                <div className="calculator-input-row">
                                    <div className="calculator-input-pair">
                                        <ui5-input id="loanAmount" placeholder="Размер на кредита*" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="period" placeholder="Срок (месеци)*" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-select id="select-installment" change={this.onSelectChange} class="select">
                                            <ui5-option value="0" icon="accounting-document-verification">Анюитетни вноски</ui5-option>
                                            <ui5-option value="1" icon="trend-down">Намаляващи вноски</ui5-option>
                                        </ui5-select>
                                    </div>
                                </div>
                                <div className="calculator-input-row">
                                    <div className="calculator-end-row">
                                        <div className="calculator-input-pair">
                                            <ui5-input id="interest" placeholder="Лихва (%)*" required></ui5-input>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ui5-card>
                        <div>
                            <ui5-card subheading="Промоционален период (месеци)" class="small">
                                <div id="promo-period">
                                    <div className="calculator-end-row">
                                        <div className="calculator-input-row">
                                            <div className="calculator-input-pair">
                                                <ui5-input id="promoPeriod" placeholder="Промо период (месеци)" required></ui5-input>
                                            </div>
                                            <div className="calculator-input-pair">
                                                <ui5-input id="promoInterest" placeholder="Промо лихва (%)" required></ui5-input>
                                            </div>
                                            <div className="calculator-input-pair">
                                                <ui5-input id="gracePeriod" placeholder="Гратисен период (месеци)" required></ui5-input>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ui5-card>
                        </div>
                        <div>
                            <ui5-card subheading="Такси" class="small">
                                <div id="fees">
                                    <div className="calculator-end-row">
                                        <div className="calculator-input-row">
                                            <div className="calculator-input-pair">
                                                <h4>Първоначални Такси</h4>
                                                <div className="starting-fees">
                                                    <div className="starting-fees-input">
                                                        <ui5-input id="аpplicationFee" placeholder="Такса кандидатстване" ></ui5-input>
                                                        <div id="fee-select">
                                                            <select id="applicationFeeValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input id="processingFee" placeholder="Такса обработка"></ui5-input>
                                                        <div id="fee-select">
                                                            <select id="processingFeeValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input id="otherFees" placeholder="Други такси" ></ui5-input>
                                                        <div id="fee-select">
                                                            <select id="otherFeesValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="calculator-input-row">
                                            <div className="calculator-input-pair">
                                                <h4>Годишни Такси</h4>
                                                <div className="annual-fees">
                                                    <div className="annual-fees-input">
                                                        <ui5-input id="annualManagementFee" placeholder="Год. такса управление" ></ui5-input>
                                                        <div id="fee-select">
                                                            <select id="annualManagementFeeValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input id="otherAnnualFees" placeholder="Други годишни такси" ></ui5-input>
                                                        <div id="fee-select">
                                                            <select id="otherAnnualFeesValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="calculator-input-row">
                                            <div className="calculator-input-pair">
                                                <h4>Месечни Такси</h4>
                                                <div className="monthly-fees">
                                                    <div className="monthly-fees-input">
                                                        <ui5-input id="monthlyManagementFee" placeholder="М. такса управление" ></ui5-input>
                                                        <div id="fee-select">
                                                            <select id="monthlyManagementFeeValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input id="otherMonthlyFees" placeholder="Други месечни такси" ></ui5-input>
                                                        <div id="fee-select">
                                                            <select id="otherMonthlyFeesValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                {/* Да се оправи ..., дали да се махне тази карта или не ? action да е надясно */}
                                <div className="calculator-calculation-credit-end">
                                    <div className="input-button">
                                        <div className="calculator-input-pair">
                                            <ui5-button ref={this.calculateCreditButtonRef} design="Emphasized">ИЗЧИСЛИ</ui5-button>
                                        </div>
                                    </div>
                                </div>
                            </ui5-card>
                        </div>
                    </div>
                </div>
                <Notes />
            </div>
        );
    }
}

export default CalculatorCredit;


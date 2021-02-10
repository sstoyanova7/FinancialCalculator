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
import axios from 'axios';
import Notes from './Notes';

class CalculatorRefinance extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loanAmount: "",
            interest: "",
            period: "",
            countOfPaidInstallments: "",
            earlyInstallmentsFee: "",
            newInterest: "",
            startingFeesPercent: "",
            startingFeesCurrency: "",
            calculated: false,
            //result
            currentLoan: {
                earlyInstallmentsFee: "",
                interest: "",
                monthlyInstallment: "",
                period: "",
                total: ""
            },
            newLoan: {
                earlyInstallmentsFee: "",
                interest: "",
                monthlyInstallment: "",
                period: "",
                total: ""
            },
            totalSavings: "",
            monthlySavings: ""
        }
        this.calculateRefinanceButtonRef = React.createRef();
    }

    inputsOnChange = (event) => {
        const { id, value } = event.target;
        this.setState({
            [id]: value
        })
    }

    onCalculatingRefinance = (event) => {
        //Функция за Валидация на state
        //Input полетата да се маркират червени с грешката
        //research -> как да се имплементират съобщенията за грешни input и... дали с масив от хардкоднати грешки.
        //Функция за добавяне на грешките в input ите
        let userAgent = navigator.userAgent;
        let postInformation = {
            "userAgent": userAgent,
            "loanAmount": parseFloat(this.state.loanAmount),
            "period": parseInt(this.state.period),
            "interest": parseFloat(this.state.interest),
            "countOfPaidInstallments": parseInt(this.state.countOfPaidInstallments),
            "earlyInstallmentsFee": parseFloat(this.state.earlyInstallmentsFee),
            "newInterest": parseFloat(this.state.newInterest),
            "startingFeesCurrency": parseFloat(this.state.startingFeesCurrency),
            "startingFeesPercent": parseFloat(this.state.startingFeesPercent)
        };
        axios({
            method: 'post',
            url: '/FinancialCalculator/api/calculateRefinancingLoan',
            data: {
                ...postInformation
            }
        }).then(res => {
            console.log(res.data);
            let currentLoan = res.data['currentLoan'];
            let newLoan = res.data['newLoan'];
            this.setState({
                calculated: true,
                totalSavings: res.data['totalSavings'],
                    currentLoan: {
                        earlyInstallmentsFee: currentLoan['earlyInstallmentsFee'],
                        interest: currentLoan['interest'],
                        monthlyInstallment: currentLoan['monthlyInstallment'],
                        period: currentLoan['period'],
                        total: currentLoan['total']
                    },
                    newLoan: {
                        earlyInstallmentsFee: newLoan['earlyInstallmentsFee'],
                        interest: newLoan['interest'],
                        monthlyInstallment: newLoan['monthlyInstallment'],
                        period: newLoan['period'],
                        total: newLoan['total']
                    },
                    monthlySavings: res.data['monthlySavings'],
                    totalSavings: res.data['totalSavings']
            })
        })
        this.setState({
            calculated: false
        });
    }

    addEventListeners() {
        const inputs = document.querySelectorAll("#input-list-currentRefinance, #input-list-newRefinance");
        const inputsArray = [...inputs];
        inputsArray.forEach(input => {
            input.addEventListener("input", this.inputsOnChange);
        })
        this.calculateRefinanceButtonRef.current.addEventListener("click", this.onCalculatingRefinance);
    }

    componentDidMount() {
        this.addEventListeners();
    }

    render() {
        return (
            <div>
                <div className="page">
                    <div className="sub-page">
                        <ui5-card heading="Калкулатор за Рефинансиране на кредит" subheading="Настоящ кредит" class="small">
                            <div id="input-list-currentRefinance">
                                <div className="calculator-input-row">
                                    <div className="calculator-input-pair">
                                        <ui5-input id="loanAmount" placeholder="Размер на кредита*" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="interest" placeholder="Лихва (%)*" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="period" placeholder="Срок на кредита (месеци)*" required></ui5-input>
                                    </div>
                                </div>
                                <div className="calculator-end-row">
                                    <div className="calculator-input-row">
                                        <div className="calculator-input-pair">
                                            <ui5-input id="countOfPaidInstallments" placeholder="Брой направени вноски*" required></ui5-input>
                                        </div>
                                        <div className="calculator-input-pair">
                                            <ui5-input id="earlyInstallmentsFee" placeholder="Такса предсрочно пог. (%)*" required></ui5-input>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ui5-card>
                        <div>
                            <ui5-card subheading="Нов кредит" class="small">
                                <div id="input-list-newRefinance">
                                    <div className="calculator-input-row">
                                        <div className="calculator-input-pair">
                                            <ui5-input id="newInterest" placeholder="Лихва*" required></ui5-input>
                                        </div>
                                        <div className="calculator-input-pair">
                                            <ui5-input id="startingFeesPercent" placeholder="Първоначални такси (%)*" required></ui5-input>
                                        </div>
                                        <div className="calculator-input-pair">
                                            <ui5-input id="startingFeesCurrency" placeholder="Първоначални такси-валута*" required></ui5-input>
                                        </div>
                                    </div>
                                    <div className="calculator-end-row">
                                        <div className="calculator-input-row">
                                            <div className="input-button">
                                                {/* Да се оправи стилистично бутона- да се сложи вдясно и по-надолу */}
                                                <div className="calculator-input-pair">
                                                    <ui5-button ref={this.calculateRefinanceButtonRef} design="Emphasized">ИЗЧИСЛИ</ui5-button>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </ui5-card>
                        </div>
                        <div>
                        </div>
                    </div>
                    {/* Да се оправи таблицата и да се попълнят полетата*/}
                    {this.state.calculated === true ?
                        <div className="sub-page">
                            <ui5-table class="demo-table" id="table">
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Инфо</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Резултат</ui5-title>
                                </ui5-table-column>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Годишен процент разход</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i></i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Общо такси</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i></i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"> Общо платено</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i></i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                            </ui5-table>
                        </div>
                        : null
                    }
                </div>
                <Notes />
            </div>
        );
    }
}

export default CalculatorRefinance;

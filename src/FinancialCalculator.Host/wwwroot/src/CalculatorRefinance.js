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
            monthlySavings: "",
            inputStates: ["None", "None", "None", "None", "None", "None", "None", "None"],
            errorMessages: ["", "", "", "", "", "", "", ""],
            responseErrorMessage: ""
        }
        this.calculateRefinanceButtonRef = React.createRef();
    }

    validation = (input, index) => {
        const value = parseInt(input);
        const inputStates = this.state.inputStates.slice();
        const errorMessages = this.state.errorMessages.slice();
        let errormsg = "";
        let state = "Error";
        if (input === "") {
            errormsg = "Стойността не може да е празна!";
        }
        else if (value < 0) {
            errormsg = "Стойността не може да е негативна!";
        }
        else if (isNaN(input)) {
            errormsg = "Стойността не може да бъде текст!";
        }
        else {
            return true;
        }

        inputStates[index] = state;
        errorMessages[index] = errormsg;
        this.setState({ inputStates: inputStates, errorMessages: errorMessages });
        return false;
    }

    inputsOnChange = (event) => {
        const { id, value, } = event.target;
        const inputStates = this.state.inputStates.slice();
        const errorMessages = this.state.errorMessages.slice();
        inputStates[event.target.getAttribute("data-index")] = "None";
        errorMessages[event.target.getAttribute("data-index")] = "";
        this.setState({ inputStates: inputStates, errorMessages: errorMessages });
        this.setState({
            [id]: value
        })
    }

    onCalculatingRefinance = (event) => {
        const v0 = this.validation(this.state.loanAmount, 0);
        const v2 = this.validation(this.state.interest, 1);
        const v1 = this.validation(this.state.period, 2);
        const v3 = this.validation(this.state.countOfPaidInstallments, 3);
        const v4 = this.validation(this.state.earlyInstallmentsFee, 4);
        const v5 = this.validation(this.state.newInterest, 5);
        const v7 = this.validation(this.state.startingFeesPercent, 6);
        const v6 = this.validation(this.state.startingFeesCurrency, 7);
        if (v0 && v1 && v2 && v3 && v4 && v5 && v6 && v7) {
            let sBrowser, sUsrAg = navigator.userAgent;

            // The order matters here, and this may report false positives for unlisted browsers.

            if (sUsrAg.indexOf("Firefox") > -1) {
                sBrowser = "Mozilla Firefox";
                // "Mozilla/5.0 (X11; Ubuntu; Linux x86_64; rv:61.0) Gecko/20100101 Firefox/61.0"
            } else if (sUsrAg.indexOf("SamsungBrowser") > -1) {
                sBrowser = "Samsung Internet";
                // "Mozilla/5.0 (Linux; Android 9; SAMSUNG SM-G955F Build/PPR1.180610.011) AppleWebKit/537.36 (KHTML, like Gecko) SamsungBrowser/9.4 Chrome/67.0.3396.87 Mobile Safari/537.36
            } else if (sUsrAg.indexOf("Opera") > -1 || sUsrAg.indexOf("OPR") > -1) {
                sBrowser = "Opera";
                // "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.102 Safari/537.36 OPR/57.0.3098.106"
            } else if (sUsrAg.indexOf("Trident") > -1) {
                sBrowser = "Microsoft Internet Explorer";
                // "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; Zoom 3.6.0; wbx 1.0.0; rv:11.0) like Gecko"
            } else if (sUsrAg.indexOf("Edge") > -1) {
                sBrowser = "Microsoft Edge";
                // "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36 Edge/16.16299"
            } else if (sUsrAg.indexOf("Chrome") > -1) {
                sBrowser = "Google Chrome or Chromium";
                // "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Ubuntu Chromium/66.0.3359.181 Chrome/66.0.3359.181 Safari/537.36"
            } else if (sUsrAg.indexOf("Safari") > -1) {
                sBrowser = "Apple Safari";
                // "Mozilla/5.0 (iPhone; CPU iPhone OS 11_4 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/11.0 Mobile/15E148 Safari/604.1 980x1306"
            } else {
                sBrowser = "unknown";
            }
            let postInformation = {
                "userAgent": sBrowser,
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
                console.log(res);
                if (res.data.status !== 200) {
                        //
                } else {
                    let currentLoan = res.data['currentLoan'];
                    let newLoan = res.data['newLoan'];
                    this.setState({
                        responseErrorMessage: "",
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
                }
            }).catch(err => {
                this.setState({
                    responseErrorMessage: "Не може да изчислим рефинансирането поради невалидни данни. Опитайте отново.",
                    calculated: false
                })
            })
            this.setState({
                calculated: false
            });
        }
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
                    {this.state.responseErrorMessage !== "" ?
                        <ui5-messagestrip type="Negative" no-close-button>{this.state.responseErrorMessage}</ui5-messagestrip> :
                        null     
                }
                    <div className="sub-page">
                        <ui5-card heading="Калкулатор за Рефинансиране на кредит" subheading="Настоящ кредит" class="small">
                            <div id="input-list-currentRefinance">
                                <div className="calculator-input-row">
                                    <div className="calculator-input-pair">
                                        <ui5-input id="loanAmount" data-index={0} value-state={this.state.inputStates[0]} placeholder="Размер на кредита*" required>
                                            <div slot="valueStateMessage">{this.state.errorMessages[0]}</div>
                                        </ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="interest" data-index={1} value-state={this.state.inputStates[1]} placeholder="Лихва (%)*" required>
                                            <div slot="valueStateMessage">{this.state.errorMessages[1]}</div>
                                        </ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="period" data-index={2} value-state={this.state.inputStates[2]} placeholder="Срок на кредита (месеци)*" required>
                                            <div slot="valueStateMessage">{this.state.errorMessages[2]}</div>
                                        </ui5-input>
                                    </div>
                                </div>
                                <div className="calculator-end-row">
                                    <div className="calculator-input-row">
                                        <div className="calculator-input-pair">
                                            <ui5-input id="countOfPaidInstallments" data-index={3} value-state={this.state.inputStates[3]} placeholder="Брой направени вноски*" required>
                                                <div slot="valueStateMessage">{this.state.errorMessages[3]}</div>
                                            </ui5-input>
                                        </div>
                                        <div className="calculator-input-pair">
                                            <ui5-input id="earlyInstallmentsFee" data-index={4} value-state={this.state.inputStates[4]} placeholder="Такса предсрочно пог. (%)*" required>
                                                <div slot="valueStateMessage">{this.state.errorMessages[4]}</div>
                                            </ui5-input>
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
                                            <ui5-input id="newInterest" data-index={5} value-state={this.state.inputStates[5]} placeholder="Лихва*" required>
                                                <div slot="valueStateMessage">{this.state.errorMessages[5]}</div>
                                            </ui5-input>
                                        </div>
                                        <div className="calculator-input-pair">
                                            <ui5-input id="startingFeesPercent" data-index={6} value-state={this.state.inputStates[6]} placeholder="Първоначални такси (%)*" required>
                                                <div slot="valueStateMessage">{this.state.errorMessages[6]}</div>
                                            </ui5-input>
                                        </div>
                                        <div className="calculator-input-pair">
                                            <ui5-input id="startingFeesCurrency" data-index={7} value-state={this.state.inputStates[7]} placeholder="Първоначални такси-валута*" required>
                                                <div slot="valueStateMessage">{this.state.errorMessages[7]}</div>
                                            </ui5-input>
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
                                    <ui5-title level="H4">Текущ Кредит</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Нов Кредит</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">+/- Спестявания</ui5-title>
                                </ui5-table-column>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Лихва</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.currentLoan.interest}%</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.newLoan.interest}%</ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Срокове на кредитите</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.currentLoan.period}</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.newLoan.period}</ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Такса за предсрочно погасяване</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.currentLoan.earlyInstallmentsFee}</ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Месечна вноска</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.currentLoan.monthlyInstallment}</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.newLoan.monthlyInstallment}</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.monthlySavings}</ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Общо изплатени</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.currentLoan.total}</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.newLoan.total}</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">{this.state.totalSavings}</ui5-title>
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

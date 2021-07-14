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
            otherMonthlyFeesValueType: "1",
            calculated: false,
            result: {
                annualPercentCost: "",
                feesCost: "",
                installmentCost: "",
                interestCost: "",
                repaymentPlan: [],
                totalCost: ""
            },
            // inputStates: ["None", "None", "None", "None", "None", "None", "None", "None", "None", "None", "None", "None", "None"],
            // errorMessages: ["", "", "", "", "", "", "", "", "", "", "", "", ""],
            responseErrorMessage: []
        }
        this.calculateCreditButtonRef = React.createRef();
        this.calc = React.createRef()
        this.res = React.createRef()
        this.onNewCalculateCredit = this.onNewCalculateCredit.bind(this);
    }
    // validation = (input, index) => {
    //     const value = parseInt(input);
    //     const inputStates = this.state.inputStates.slice();
    //     const errorMessages = this.state.errorMessages.slice();
    //     let errormsg = "";
    //     let state = "Error";
    //     if (input === "") {
    //         errormsg = "Стойността не може да е празна!";
    //     }
    //     else if (value < 0) {
    //         errormsg = "Стойността не може да е негативна!";
    //     }
    //     else if (isNaN(input)) {
    //         errormsg = "Стойността не може да бъде текст!";
    //     }
    //     else {
    //         return true;
    //     }

    //     inputStates[index] = state;
    //     errorMessages[index] = errormsg;
    //     this.setState({ inputStates: inputStates, errorMessages: errorMessages });
    //     return false;
    // }

    inputsOnChange = (event) => {
        const {id , value} = event.target;
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
        // const v0 = this.validation(this.state.loanAmount, 0);
        // const v1 = this.validation(this.state.period, 1);
        // const v2 = this.validation(this.state.interest, 2);
        // const v3 = this.validation(this.state.promoPeriod, 3);
        // const v4 = this.validation(this.state.promoInterest, 4);
        // const v5 = this.validation(this.state.gracePeriod, 5);
        // const v6 = this.validation(this.state.аpplicationFee, 6);
        // const v7 = this.validation(this.state.processingFee, 7);
        // const v8 = this.validation(this.state.otherFees, 8);
        // const v9 = this.validation(this.state.annualManagementFee, 9);
        // const v10 = this.validation(this.state.otherAnnualFees, 10);
        // const v11 = this.validation(this.state.monthlyManagementFee, 11);
        // const v12 = this.validation(this.state.otherMonthlyFees, 12);
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
        const feesValue = ['аpplicationFee', 'processingFee', 'otherFees', 'annualManagementFee',
            'otherAnnualFees', 'monthlyManagementFee', 'otherMonthlyFees'];
        const feeValueTypes = [
            'applicationFeeValueType', 'processingFeeValueType', 'otherFeesValueType', 'annualManagementFeeValueType',
            'otherAnnualFeesValueType', 'monthlyManagementFeeValueType', 'otherMonthlyFeesValueType'];
        const promoPeriod = ["promoPeriod", "promoInterest", "gracePeriod"];
        let postInformation = {
            "userAgent": sBrowser,
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
            if (res.data.status !== 200) {
                const errorMessages = res.data.errorMessage.split('..');
                this.setState({
                    responseErrorMessage: errorMessages,
                    calculated: false
                })
            } else {
                const repaymentPlan = [...res.data.repaymentPlan];
                this.setState({
                    calculated: true,
                    responseErrorMessage: "",
                    result: {
                        annualPercentCost: res.data['annualPercentCost'],
                        installmentCost: res.data['installmentsCost'],
                        interestCost: res.data['interestsCost'],
                        feesCost: res.data['feesCost'],
                        repaymentPlan: repaymentPlan,
                        totalCost: res.data['totalCost']
                    }
                })
                window.scrollTo({
                    top: 900,
                    left: 100,
                    behavior: 'smooth'
                });
            }
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

    onNewCalculateCredit = (event) => {
        window.scrollTo({
            top: 0,
            left: 100,
            behavior: 'smooth'
        });
    }

    addEventListeners() {
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
                    {this.state.responseErrorMessage.length !== 0 ?
                        this.state.responseErrorMessage.map(error => {
                            return (
                                <ui5-messagestrip type="Negative" no-close-button>{error}</ui5-messagestrip>
                            )
                        }) :
                        null
                    }
                    <div className="sub-page">
                        <ui5-card ref={this.calc} heading="Кредитен калкулатор" subheading="Пресметнете месечни вноски и ГПР (годишен процент на разходите)" class="small">
                            <div id="annual-percentage-of-fees">
                                <div className="calculator-input-row">
                                    <div className="calculator-input-pair">
                                        <ui5-input  id="loanAmount" placeholder="Размер на кредита*" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input  id="period" placeholder="Срок (месеци)*" required></ui5-input>
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
                                            <ui5-input  id="interest" placeholder="Лихва (%)*" required>
                                            
                                            </ui5-input>
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
                                                <ui5-input  id="promoPeriod" placeholder="Промо период (месеци)" required>
                                               
                                                </ui5-input>
                                            </div>
                                            <div className="calculator-input-pair">
                                                <ui5-input  id="promoInterest" placeholder="Промо лихва (%)" required>
                                               
                                                </ui5-input>
                                            </div>
                                            <div className="calculator-input-pair">
                                                <ui5-input  id="gracePeriod" placeholder="Гратисен период (месеци)" required>
                                                
                                                </ui5-input>
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
                                                        <ui5-input  id="аpplicationFee" placeholder="Такса кандидатстване" >
                                                        
                                                        </ui5-input>
                                                        <div id="fee-select">
                                                            <select id="applicationFeeValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input id="processingFee" placeholder="Такса обработка">
                                                       
                                                        </ui5-input>
                                                        <div id="fee-select">
                                                            <select id="processingFeeValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input  id="otherFees" placeholder="Други такси" >
                                                        
                                                        </ui5-input>
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
                                                        <ui5-input id="annualManagementFee" placeholder="Год. такса управление" >
                                                        
                                                        </ui5-input>
                                                        <div id="fee-select">
                                                            <select id="annualManagementFeeValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input  id="otherAnnualFees" placeholder="Други годишни такси" >
                                                        
                                                        </ui5-input>
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
                                                        <ui5-input  id="monthlyManagementFee" placeholder="М. такса управление" >
                                                        
                                                        </ui5-input>
                                                        <div id="fee-select">
                                                            <select id="monthlyManagementFeeValueType">
                                                                <option value="1">лв</option>
                                                                <option value="0">%</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input  id="otherMonthlyFees" placeholder="Други месечни такси" >
                                                       
                                                        </ui5-input>
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
                                <div className="calculator-calculation-credit-end">
                                    <div className="input-button">
                                        <div className="calculator-input-pair">
                                            <ui5-button onClick={this.onCalculateCredit} design="Emphasized">ИЗЧИСЛИ</ui5-button>
                                        </div>
                                    </div>
                                </div>
                            </ui5-card>
                        </div>
                    </div>
                    <div className="tables-credit">
                        <div className="upper-table">
                            <ui5-table class="demo-table" id="table">
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Инфо</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Резултат</ui5-title>
                                </ui5-table-column>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Годишен процентен разход</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i>{this.state.result.annualPercentCost}</i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Погасени лихви и такси</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i>{this.state.result.totalCost}</i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Такси и комисионни</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i>{this.state.result.feesCost}</i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Лихви</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i>{this.state.result.interestCost}</i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5">Вноски</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i>{this.state.result.installmentCost}</i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                            </ui5-table>
                        </div>
                        <div className="repayment-plan">
                            <ui5-table class="demo-table" id="table">
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">№</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Дата</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Месечна вноска</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Вноска главница</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Вноска лихва</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Остатък главница</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Такси и комисионни</ui5-title>
                                </ui5-table-column>
                                <ui5-table-column slot="columns">
                                    <ui5-title level="H4">Паричен поток</ui5-title>
                                </ui5-table-column>
                                {this.state.result.repaymentPlan.map(plan => {
                                    return (
                                        <ui5-table-row id={plan.id}>
                                            <ui5-table-cell>
                                                <ui5-title level="H5">{plan.id}</ui5-title>
                                            </ui5-table-cell>
                                            <ui5-table-cell>
                                                <ui5-title level="H5">{plan.date.substring(0, plan.date.indexOf('T'))}</ui5-title>
                                            </ui5-table-cell>
                                            <ui5-table-cell>
                                                <ui5-title level="H5">{plan.monthlyInstallment}</ui5-title>
                                            </ui5-table-cell>
                                            <ui5-table-cell>
                                                <ui5-title level="H5">{plan.principalInstallment}</ui5-title>
                                            </ui5-table-cell>
                                            <ui5-table-cell>
                                                <ui5-title level="H5">{plan.interestInstallment}</ui5-title>
                                            </ui5-table-cell>
                                            <ui5-table-cell>
                                                <ui5-title level="H5">{plan.principalBalance}</ui5-title>
                                            </ui5-table-cell>
                                            <ui5-table-cell>
                                                <ui5-title level="H5">{plan.fees}</ui5-title>
                                            </ui5-table-cell>
                                            <ui5-table-cell>
                                                <ui5-title level="H5">{plan.cashFlow}</ui5-title>
                                            </ui5-table-cell>
                                        </ui5-table-row>
                                    )
                                })}
                            </ui5-table>
                        </div>
                        <div className="new-credit-calculation"><ui5-button onClick={this.onNewCalculateCredit} design="Emphasized">НОВО ИЗЧИСЛЕНИЕ</ui5-button></div>
                    </div>

                </div>

                <Notes />
            </div>
        );
    }
}

export default CalculatorCredit;


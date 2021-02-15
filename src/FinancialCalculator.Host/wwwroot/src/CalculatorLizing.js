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
import "@ui5/webcomponents/dist/MessageStrip";

class CalculatorLizing extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            userAgent: "",
            productPrice: "",
            startingInstallment: "",
            leasePeriod: "",
            monthlyInstallment: "",
            startingFee: "",
            feeValueType: "1",
            calculated: false,
            result: {
                anualPercentExpense: "",
                totalPaidWithFees: "",
                TotalFees: ""
            },
            inputStates: ["None", "None", "None", "None", "None"],
            errorMessages: ["", "", "", "", ""],
            responseErrorMessage: ""
        }
        this.calculateLeasingButtonRef = React.createRef();
    }

    validation = (input, index) => {
        const value = parseInt(input);
        // const inputStates = this.state.inputStates.slice();
        // const errorMessages = this.state.errorMessages.slice();
        let inputStates = [...this.state.inputStates];
        let errorMessages = [...this.state.errorMessages];
        let errormsg = "";
        let state = "Error";
        if (index === 4) {
            if (value < 0) {
                errormsg = "Стойността не може да е негативна!";
            }
            else if (isNaN(input)) {
                errormsg = "Стойността не може да бъде текст!";
            }
            else {
                return true;
            }
        } else {
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
        }

        inputStates[index] = state;
        errorMessages[index] = errormsg;
        // this.setState({ inputStates: inputStates, errorMessages: errorMessages });
        this.setState({ inputStates, errorMessages });
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


    onCalculateLeasing = (event) => {
        const v0 = this.validation(this.state.productPrice, 0);
        const v1 = this.validation(this.state.startingInstallment, 1);
        const v2 = this.validation(this.state.leasePeriod, 2);
        const v3 = this.validation(this.state.monthlyInstallment, 3);
        const v4 = this.validation(this.state.startingFee, 4);
        if (v0 && v1 && v2 && v3 && v4) {
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
            console.log(sBrowser);
            let postInformation = {};
            if (!this.state.startingFee) {
                postInformation = {
                    'UserAgent': sBrowser,
                    'ProductPrice': parseFloat(this.state.productPrice),
                    'StartingInstallment': parseFloat(this.state.startingInstallment),
                    'Period': parseFloat(this.state.leasePeriod),
                    'MonthlyInstallment': parseFloat(this.state.monthlyInstallment),
                }
            } else {
                postInformation = {
                    'UserAgent': sBrowser,
                    'ProductPrice': parseFloat(this.state.productPrice),
                    'StartingInstallment': parseFloat(this.state.startingInstallment),
                    'Period': parseFloat(this.state.leasePeriod),
                    'MonthlyInstallment': parseInt(this.state.monthlyInstallment),
                    'StartingFee': {
                        'Type': 1,
                        'Value': parseFloat(this.state.startingFee),
                        'ValueType': parseInt(this.state.feeValueType)
                    }
                }
            }
            console.log(postInformation);
            axios({
                method: 'post',
                url: '/FinancialCalculator/api/calculateLeasingLoan',
                data: {
                    ...postInformation
                }
            }).then(res => {
                if (res.data.status !== 200) {
                    this.setState({
                        responseErrorMessage: res.data.errorMessage,
                        calculated: false
                    })
                } else {
                    this.setState({
                        responseErrorMessage: "",
                        calculated: true,
                        ...{
                            result: {
                                anualPercentExpense: res.data['annualPercentCost'],
                                totalPaidWithFees: res.data['totalCost'],
                                TotalFees: res.data['totalFees']
                            }
                        }
                    })
                }
            }).catch(err => {
                console.log(err);
            })
        }
    }

    onChangeFeeType = (event) => {
        const name = event.target.id;
        const value = event.target.value;
        this.setState({
            [name]: value
        })

    }

    addEventListeners() {
        this.calculateLeasingButtonRef.current.addEventListener("click", this.onCalculateLeasing);
        const inputs = document.getElementById('input-list');
        inputs.addEventListener("input", this.inputsOnChange);
        const selectFeeType = document.getElementById("feeValueType");
        selectFeeType.addEventListener("change", this.onChangeFeeType);
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
                        <ui5-card heading="Лизингов калкулатор" subheading="ГПР лизинговият калкулатор изчислява цената на дадена кредитна оферта." class="small">
                            <div id="input-list">
                                <div className="calculator-input-row">
                                    <div className="calculator-input-pair">
                                        <ui5-input id="productPrice" data-index={0} value-state={this.state.inputStates[0]} placeholder="Въведете цена на стоката*" required>
                                            <div slot="valueStateMessage">{this.state.errorMessages[0]}</div>
                                        </ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="startingInstallment" data-index={1} value-state={this.state.inputStates[1]} placeholder="Първоначална вноска*" required>
                                            <div slot="valueStateMessage">{this.state.errorMessages[1]}</div>
                                        </ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="leasePeriod" data-index={2} value-state={this.state.inputStates[2]} placeholder="Срок на лизинга (месеци)*" required>
                                            <div slot="valueStateMessage">{this.state.errorMessages[2]}</div>
                                        </ui5-input>
                                    </div>
                                </div>
                                <div className="calculator-input-row">
                                    <div className="calculator-input-pair">
                                        <ui5-input id="monthlyInstallment" data-index={3} value-state={this.state.inputStates[3]} placeholder="Месечна вноска*" required>
                                            <div slot="valueStateMessage">{this.state.errorMessages[3]}</div>
                                        </ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">

                                        <ui5-input id="startingFee" data-index={4} value-state={this.state.inputStates[4]} placeholder="Първоначална такса" required>
                                            <div slot="valueStateMessage">{this.state.errorMessages[4]}</div>
                                        </ui5-input>
                                        <div id="fee-select">
                                            <select id="feeValueType">
                                                <option value="1">лв</option>
                                                <option value="0">%</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div className="calculator-calculation-end">
                                        <div className="calculator-input-pair">
                                            <ui5-button ref={this.calculateLeasingButtonRef} design="Emphasized">ИЗЧИСЛИ</ui5-button>
                                        </div>
                                    </div>
                                </div>
                                <div className="list-items">
                                </div>
                            </div>
                        </ui5-card>
                    </div>
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
                                        <ui5-title level="H5"><i>{this.state.result.anualPercentExpense}</i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"> Общо изплатено с такси</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i>{this.state.result.totalPaidWithFees}</i></ui5-title>
                                    </ui5-table-cell>
                                </ui5-table-row>
                                <ui5-table-row>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"> Общо такси</ui5-title>
                                    </ui5-table-cell>
                                    <ui5-table-cell>
                                        <ui5-title level="H5"><i>{this.state.result.TotalFees}</i></ui5-title>
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

export default CalculatorLizing;

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
            }
        }
        this.calculateLeasingButtonRef = React.createRef();
    }

    inputsOnChange = (event) => {
        const { id, value } = event.target;
        this.setState({
            [id]: value
        })
      
    }

    //Функция за Валидация на state
    //Input полетата да се маркират червени с грешката
    //research -> как да се имплементират съобщенията за грешни input и... дали с масив от хардкоднати грешки.
    //Функция за добавяне на грешките в input ите

    onCalculateLeasing = (event) => {
        // if validation === true => post/get 
        let userAgent = navigator.userAgent;
        let postInformation = {};
        if (!this.state.startingFee) {
            postInformation = {
                'UserAgent': userAgent,
                'ProductPrice': parseFloat(this.state.productPrice),
                'StartingInstallment': parseFloat(this.state.startingInstallment),
                'Period': parseFloat(this.state.leasePeriod),
                'MonthlyInstallment': parseFloat(this.state.monthlyInstallment),
            }
        } else {
            postInformation = {
                'UserAgent': userAgent,
                'ProductPrice': parseFloat(this.state.productPrice),
                'StartingInstallment': parseFloat(this.state.startingInstallment),
                'Period': parseFloat(this.state.leasePeriod),
                'MonthlyInstallment': parseInt(this.state.monthlyInstallment),
                'StartingFee': {
                    'Type': 1,
                    'Value': parseFloat(this.state.startingFee),
                    'ValueType': parseInt(this.state.feeValueType)
                    //Change valueType 
                }
            }
        }
    
        axios({
            method: 'post',
            url: '/FinancialCalculator/api/calculateLeasingLoan',
            data: {
                ...postInformation
            }
        }).then(res => {
            
            this.setState({
                calculated: true,
                ...{
                    result: {
                        anualPercentExpense: res.data['annualPercentCost'],
                        totalPaidWithFees: res.data['totalCost'],
                        TotalFees: res.data['totalFees']
                    }
                }
            })
        }).then(err => {
            console.log(err);
        })

        this.setState({
            calculated: false
            //result = response;
        });
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
                    <div className="sub-page">
                        <ui5-card heading="Лизингов калкулатор" subheading="ГПР лизинговият калкулатор изчислява цената на дадена кредитна оферта." class="small">
                            <div id="input-list">
                                <div className="calculator-input-row">
                                    <div className="calculator-input-pair">
                                        <ui5-input id="productPrice" placeholder="Въведете цена на стоката*" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="startingInstallment" placeholder="Първоначална вноска*" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="leasePeriod" placeholder="Срок на лизинга (месеци)*" required></ui5-input>
                                    </div>
                                </div>
                                <div className="calculator-input-row">
                                    <div className="calculator-input-pair">
                                        <ui5-input id="monthlyInstallment" placeholder="Месечна вноска*" required></ui5-input>
                                    </div>

                                    <div className="calculator-input-pair">

                                        <ui5-input id="startingFee" placeholder="Първоначална такса" required></ui5-input>
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

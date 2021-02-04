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
import Notes from './Notes';

class CalculatorCredit extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loanAmount: "",
            period: "",
            installmentType: "",
            interest: "",
            // Promo Period
            promoPeriod: "",
            promoInterest: "",
            gracePeriod: "",
            // Fees
            feeType: "",
            // StartingFees
            аpplicationFee: "",
            processingFee: "",
            otherFees: "",
            // Annual Fees
            annualManagementFee: "",
            otherAnnualFees: "",
            // Monthly Fees
            monthlyManagementFee: "",
            otherMonthlyFees: ""
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
    
    onFeeChange = (event) => {
        this.setState({
            feeType: event.target.value
        })
    }

    onCalculateCredit = (event) => {
        //Функция за Валидация на state
        //Input полетата да се маркират червени с грешката
        //research -> как да се имплементират съобщенията за грешни input и... дали с масив от хардкоднати грешки.
        //Функция за добавяне на грешките в input ите
    }

    addEventListeners() {
        this.calculateCreditButtonRef.current.addEventListener("click", this.onCalculateCredit);
        const inputs = document.querySelectorAll("#annual-percentage-of-fees, #promo-period, #fees");
        const inputsArray = [...inputs];
        inputsArray.forEach(input => {
            input.addEventListener("input", this.inputsOnChange);
        })
        const select = document.getElementById("select-installment");
        select.addEventListener("change", this.onSelectChange);
        const feeRadioButtons = document.getElementsByClassName("leasingCurrency") || [];
        Array.from(feeRadioButtons).forEach((box) => box.addEventListener("select", this.onFeeChange));
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
                                        <ui5-input id="loanAmount" placeholder="Размер на кредита" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="period" placeholder="Срок (месеци)" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-select id="select-installment" change={this.onSelectChange} class="select">
                                            <ui5-option value="martin" icon="accounting-document-verification">Анюитетни вноски</ui5-option>
                                            <ui5-option value="stoyan" icon="trend-down">Намаляващи вноски</ui5-option>
                                        </ui5-select>
                                    </div>
                                </div>
                                <div className="calculator-input-row">
                                    <div className="calculator-end-row">
                                        <div className="calculator-input-pair">
                                            <ui5-input id="interest" placeholder="Лихва %" required></ui5-input>
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
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input id="processingFee" placeholder="Такса обработка"></ui5-input>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input id="otherFees" placeholder="Други такси" ></ui5-input>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="calculator-input-row">
                                            <div className="calculator-input-pair">
                                                <h4>Годишни Такси</h4>
                                                <div className="annual-fees">
                                                    <div className="annual-fees-input">
                                                        <ui5-input id="annualManagementFee" placeholder="Годишна такса управление" ></ui5-input>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input id="otherAnnualFees" placeholder="Други годишни такси" ></ui5-input>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="calculator-input-row">
                                            <div className="calculator-input-pair">
                                                <h4>Месечни Такси</h4>
                                                <div className="monthly-fees">
                                                    <div className="monthly-fees-input">
                                                        <ui5-input id="monthlyManagementFee" placeholder="Месечна такса управление" ></ui5-input>
                                                    </div>
                                                    <div className="calculator-input-pair">
                                                        <ui5-input id="otherMonthlyFees" placeholder="Други месечни такси" ></ui5-input>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                {/* Да се оправи ..., дали да се махне тази карта или не ? action да е надясно */}
                                <ui5-card>
                                    <div className="calculator-calculation-credit-end">
                                        <div className="calculator-input-pair">
                                            <div id="radioGroup">
                                                <ui5-radiobutton text="Валута" value="Валута" selected name="Currency" class="leasingCurrency"></ui5-radiobutton>
                                                <ui5-radiobutton text="Проценти" value="Проценти" name="Currency" class="leasingCurrency"></ui5-radiobutton>
                                            </div>
                                        </div>
                                        <div className="input-button">
                                            <div className="calculator-input-pair">
                                                <ui5-button ref={this.calculateCreditButtonRef} design="Emphasized">ИЗЧИСЛИ</ui5-button>
                                            </div>
                                        </div>
                                    </div>

                                </ui5-card>
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


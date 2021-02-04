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
            startingFeesCurrency: ""
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
                        <ui5-card heading="Калкулатор за Рефинансиране на кредит" subheading="Настоящ креди" class="small">
                            <div id="input-list-currentRefinance">
                                <div className="calculator-input-row">
                                    <div className="calculator-input-pair">
                                        <ui5-input id="loanAmount" placeholder="Размер на кредита" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="interest" placeholder="Лихва (%)" required></ui5-input>
                                    </div>
                                    <div className="calculator-input-pair">
                                        <ui5-input id="period" placeholder="Срок (месеци)" required></ui5-input>
                                    </div>
                                </div>
                                <div className="calculator-end-row">
                                    <div className="calculator-input-row">
                                        <div className="calculator-input-pair">
                                            <ui5-input id="countOfPaidInstallments" placeholder="Брой направени вноски" required></ui5-input>
                                        </div>
                                        <div className="calculator-input-pair">
                                            <ui5-input id="earlyInstallmentsFee" placeholder="Такса предсрочно пог. (%)" required></ui5-input>
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
                                            <ui5-input id="newInterest" placeholder="Лихва" required></ui5-input>
                                        </div>
                                        <div className="calculator-input-pair">
                                            <ui5-input id="startingFeesPercent" placeholder="Първоначални такси (%)" required></ui5-input>
                                        </div>
                                        <div className="calculator-input-pair">
                                            <ui5-input id="startingFeesCurrency" placeholder="Първоначални такси-валута" required></ui5-input>
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
                </div>
                <Notes />
            </div>
        );
    }
}

export default CalculatorRefinance;

import React from 'react';
import './Calculators.css';

class History extends React.Component {
    constructor(props) {
        super(props);
    }

    onCreditHistoryClick = () => {
        this.props.history.push(('/history/credit'))
    }
    onRefinanceHistoryClick = () => {
        this.props.history.push(('/history/refinance'))
    }
    onLeaseHistoryClick = () => {
        this.props.history.push(('/history/leasing'))
    }

    render() {
        if (this.props.cookie !== "") {
            return (
                <div>
                    <div className="container">
                        <div className="card-wrapper">
                            <div className="card">
                                <h2 className="heading">Кредитна история</h2>
                                <p className="description">
                                    {/* какво да се напише в картичките */}
                                    Универсален кредитен калкулатор. Изчислява всички параметри на един кредит:
                                    месечна вноска, размер на кредита, годишна лихва, годишен процент на разходите, срок на кредита, максимален възможен размер на кредит.
                                </p>
                                <div className="button">
                                    <ui5-button onClick={this.onCreditHistoryClick} design="Emphasized">Към Кредитна История</ui5-button>
                                </div>
                            </div>
                            <div className="card">
                                <h2 className="heading">История на рефинансиране</h2>
                                <p className="description">
                                    C пoмoщтa нa ĸaлĸyлaтopa зa peфинaнcиpaнe нa ĸpeдити мoжeтe дa изчиcлитe ĸoлĸo биxтe cпecтили/изгyбили,
                                    aĸo peшитe дa peфинaнcиpaтe, т.e. дa изтeглитe нoв ĸpeдит, зa дa пoгacитe cтapo ĸpeдитнo зaдължeниe.
                                 </p>
                                <div className="button">
                                    <ui5-button onClick={this.onRefinanceHistoryClick} design="Emphasized">Към история на рефинансиране</ui5-button>
                                </div>
                            </div>
                            <div className="card">
                                <h2 className="heading">Лизингова история</h2>
                                <p className="description">
                                    Целта на ГПР Калкулатора за кредити е да Ви помогне да изчислите истинската цена на дадена кредитна оферта.
                                    Колкото по-изгодна е дадена оферта, толкова по-ниски са ГПР и Общо погасената сума.
                                  </p>
                                <div className="button">
                                    <ui5-button onClick={this.onLeaseHistoryClick} design="Emphasized">Към лизингова история</ui5-button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            );
        } else {
            return (null);
        }
    }
}

export default History;

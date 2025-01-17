import React from 'react';
import './Calculators.css';
import "@ui5/webcomponents/dist/Card";
class Calculators extends React.Component {
    constructor(props) {
        super(props);
    }

    onCreditClick = () => {
        this.props.history.push(('/calculators/credit'));
    }
    onLeaseClick = () => {
        this.props.history.push(('/calculators/lizing'));
    }
    onRefinanceClick = () => {
        this.props.history.push(('/calculators/refinance'));
    }
    
    render() {
        return (
            <div>
                <div className="container">
                    <div className="card-wrapper">
                        <div className="card">
                            <h2 className="heading">Кредитен калкулатор</h2>
                            <p className="description">
                                Универсален кредитен калкулатор. Изчислява всички параметри на един кредит:
                                месечна вноска, размер на кредита, годишна лихва, годишен процент на разходите, срок на кредита, максимален възможен размер на кредит.
                            </p>
                            <div className="button">
                            <ui5-button onClick={this.onCreditClick}design="Emphasized">Към калкулатора</ui5-button>
                            </div>
                        </div>
                        <div className="card">
                            <h2 className="heading">Калкулатор за рефинансиране</h2>
                            <p className="description">
                                C пoмoщтa нa ĸaлĸyлaтopa зa peфинaнcиpaнe нa ĸpeдити мoжeтe дa изчиcлитe ĸoлĸo биxтe cпecтили/изгyбили,
                                aĸo peшитe дa peфинaнcиpaтe, т.e. дa изтeглитe нoв ĸpeдит, зa дa пoгacитe cтapo ĸpeдитнo зaдължeниe.
                             </p>
                            <div className="button">
                            <ui5-button onClick={this.onRefinanceClick} design="Emphasized">Към калкулатора</ui5-button>
                            </div>
                        </div>
                        <div className="card">
                            <h2 className="heading">Лизингов калкулатор</h2>
                            <p className="description">
                                Целта на ГПР Калкулатора за кредити е да Ви помогне да изчислите истинската цена на дадена кредитна оферта.
                                Колкото по-изгодна е дадена оферта, толкова по-ниски са ГПР и Общо погасената сума. 
                              </p>
                              <div className="button">
                            <ui5-button onClick={this.onLeaseClick} design="Emphasized">Към калкулатора</ui5-button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default Calculators;
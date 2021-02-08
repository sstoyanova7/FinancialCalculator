import React from 'react';
import './Pages.css';

class Home extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <p>{"Здравей, " + this.props.userInfo.profileName + "!"}</p>
                <div className="container">
                    <div className="account-form">
                        <ui5-card heading="Вход" class="small">
                            <div class="account-input">
                                <div className="account-input-username">
                                    <ui5-input id="sign-userName" placeholder="Потребителско име" required></ui5-input>
                                </div>
                                <div className="account-input-password">
                                    <ui5-input id="sign-password" type="Password" placeholder="Парола" required></ui5-input>
                                    <div className="text-input">
                                        <a href="http://localhost:3000/#/">
                                            <i>Забравена парола?</i>
                                        </a>
                                    </div>
                                </div>
                                <div className="account-button-actions">
                                    <ui5-button ref={this.calculateLeasingButtonRef} design="Emphasized">Вход</ui5-button>
                                </div>
                            </div>
                        </ui5-card>
                    </div>
                    <div className="register-form">
                        <ui5-card heading="Регистрация" class="small">
                            <div className="register-input">
                                <div className="register-input-username">
                                    <ui5-input id="sign-username" placeholder="Потребителско име" required></ui5-input>
                                </div>
                                <div className="register-input-email">
                                    <ui5-input id="register-email" placeholder="Имейл адрес" required></ui5-input>
                                </div>
                                <div className="register-input-password">
                                    <ui5-input id="register-password" type="Password" placeholder="Парола" required></ui5-input>
                                </div>
                                <div className="register-input-confirm-password">
                                    <ui5-input id="register-confirm-password" type="Password" placeholder="Потвърдете паролата" required></ui5-input>
                                </div>
                                <div className="account-button-actions">
                                    <ui5-button ref={this.calculateLeasingButtonRef} design="Emphasized">Регистрация</ui5-button>
                                </div>
                            </div>
                        </ui5-card>
                    </div>
                </div>
            </div>
        );
    }
}

export default Home;

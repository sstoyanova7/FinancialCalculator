import React from 'react';
import './Pages.css';

class Home extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <p>{"Здравей, "+this.props.userInfo.profileName + "!"}</p>
                <div className="account-form">
                    <ui5-card heading="Вход" class="small">
                        <div className="account-input">
                            <ui5-input id="sign-userName" placeholder="Потребителско име" required></ui5-input>
                        </div>
                        <div className="account-input">
                            <ui5-input id="sign-password" type="Password" placeholder="Парола" required></ui5-input>
                        </div>
                        <div className="text-input"><a href="http://localhost:3000/#/"><i>Забравена парола?</i></a></div>
                        <div className="account-button-actions">
                        <ui5-button ref={this.calculateLeasingButtonRef} design="Transparent">Вход</ui5-button>
                        </div>
                    </ui5-card>
                </div>
                <div className="account-form">
                    <ui5-card heading="Регистрация" class="small">
                    <div className="account-input">
                            <ui5-input id="email" placeholder="Имейл" required></ui5-input>
                        </div>
                    <div className="account-input">
                            <ui5-input id="userName" placeholder="Потребителско име" required></ui5-input>
                        </div>
                        <div className="account-input">
                            <ui5-input id="password" type="Password" placeholder="Парола" required></ui5-input>
                        </div>
                        <div className="account-input">
                            <ui5-input id="confirm-password" type="Password" placeholder="Потвърдете паролата" required></ui5-input>
                        </div>
                        <div className="account-button-actions">
                        <ui5-button ref={this.calculateLeasingButtonRef} design="Transparent">Регистрация</ui5-button>
                        </div>
                    </ui5-card>
                </div>
            </div >
        );
    }
}

export default Home;

import React from 'react';
import './Pages.css';
import './Navbar.css';
import axios from 'axios';
import TitleCss from '@ui5/webcomponents/dist/generated/themes/Title.css';
import Calculators from './Calculators';
import "@ui5/webcomponents/dist/MessageStrip";
class Home extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            loginUsername: "",
            loginPassword: "",
            registerUsername: "",
            registerEmail: "",
            registerPassword: "",
            registerRepeatedPassword: "",
            cookie: "",
            hasCookie: false,
            responseErrorMessage: "",
            code: 2,
            registered: 0,
            messageBox: {
                '200': <ui5-messagestrip type="Positive" no-close-button>успешна регистрация</ui5-messagestrip>,
                '400': <ui5-messagestrip type="Negative" no-close-button>Невалидни полета</ui5-messagestrip>,
                '404': <ui5-messagestrip type="Negative" no-close-button>Несъществуващ потребител</ui5-messagestrip>,
                '409': <ui5-messagestrip type="Negative" no-close-button>Съществуващо такова потребителско име</ui5-messagestrip>
            },
            message: "",
            messageBoxLogin: {
                '400': <ui5-messagestrip type="Negative" no-close-button>Грешна парола или потребителско име</ui5-messagestrip>,
                '404': <ui5-messagestrip type="Negative" no-close-button>Несъществуващ потребител</ui5-messagestrip>,
                '1000': <ui5-messagestrip type="Negative" no-close-button>Потребителското име или парола не могат да бъдат празни</ui5-messagestrip>
            },
            messageLogin: ""

        };
        this.homeSubmitButton = React.createRef();
        this.homeLogout = React.createRef();
    }

    onLoginSubmit = () => {
        let code = '1000';
        const postInformation = {
            "username": `${this.state.loginUsername}`,
            "password": `${this.state.loginPassword}`
        }

        if (this.state.loginUsername === "" || this.state.loginPassword === "") {
            this.setState({
                messageLogin: this.state.messageBoxLogin[code]
            })
        } else {
            axios({
                method: "post",
                url: "http://localhost:5000/api/Authentication/sign-in",
                data: { ...postInformation }
            }).then(res => {
                this.setState({
                    cookie: document.cookie,
                    hasCookie: true,
                })
                this.props.loadCookie(this.state.loginUsername);
                this.props.history.push(('/calculators'));
            }).catch(err => {
               let errorCode = String(err).split(' ');
                this.setState({
                    cookie: "",
                    hasCookie: false,
                    messageLogin: this.state.messageBoxLogin[`${errorCode[6]}`]
                })
            })
        }
    }

    onRegisterSubmit = () => {
        let code = 400;
        const postInformation = {
            "Username": this.state.registerUsername,
            "Email": this.state.registerEmail,
            "Password": this.state.registerPassword,
            "Password2": this.state.registerRepeatedPassword
        }
        if (this.state.registerPassword === "" || this.state.registerUsername === "" || this.state.registerRepeatedPassword === "" || this.state.registerEmail === "") {
            this.setState({
                message: this.state.messageBox[code]
            })
        } else {
            axios({
                method: "post",
                url: "http://localhost:5000/api/Authentication/sign-up",
                data: { ...postInformation }
            }).then(res => {
                code = 200;
                this.setState({
                    code: code,
                    message: this.state.messageBox[code]
                })
            }).catch(err => {
                this.setState({
                    message: this.state.messageBox[409]
                })
            })
        }
    }

    onInputChange = (event) => {
        const { name, value, } = event.target;
        this.setState({
            [name]: value
        })
    }

    componentDidMount() {
        if (document.cookie) {
            this.setState({
                cookie: document.cookie,
                hasCookie: true
            })
            this.props.loadCookie();
            this.props.history.push(('/calculators'));

        } else {
            this.setState({
                cookie: "",
                hasCookie: false

            })
        }
    }

    render() {

        return (
            <div>
                <div className="container">
                    <div className="form-wrapper">
                        <div className="form">
                            <div>
                                {this.state.messageLogin}
                            </div>
                            <h3 className="entry">Вход</h3>
                            <form>
                                <div className="login-username">
                                    <input type="text" name="loginUsername" value={this.state.loginUsername} onChange={this.onInputChange} placeholder="Потребителско име" />
                                </div>
                                <div className="login-password">
                                    <input type="password" name="loginPassword" value={this.state.loginPassword} onChange={this.onInputChange} placeholder="Парола" />
                                    <div className="text-input"><a href="#"><i>Забравена парола?</i></a></div>
                                </div>
                                <ui5-button onClick={this.onLoginSubmit} design="Emphasized">Вход</ui5-button>
                            </form>
                        </div>
                        <div className="form">
                            <div>
                                {this.state.message}
                            </div>
                            <h3>Регистрация</h3>
                            <form>
                                <div className="register-username">
                                    <input type="text" name="registerUsername" value={this.state.registerUsername} onChange={this.onInputChange} placeholder="Потребителско име" />
                                </div>
                                <div className="register-email">
                                    <input type="email" name="registerEmail" value={this.state.registerEmail} onChange={this.onInputChange} placeholder="Имейл" />
                                </div>
                                <div className="register-password">
                                    <input type="password" name="registerPassword" value={this.state.registerPassword} onChange={this.onInputChange} placeholder="Парола" />
                                </div>
                                <div className="register-repeated-password">
                                    <input type="password" name="registerRepeatedPassword" value={this.state.registerRepeatedPassword} onChange={this.onInputChange} placeholder="Повторете паролата" />
                                </div>
                                <ui5-button onClick={this.onRegisterSubmit} design="Emphasized">Регистрация</ui5-button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default Home;

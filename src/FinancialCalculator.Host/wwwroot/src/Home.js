import React from 'react';
import './Pages.css';
import './Navbar.css';
import axios from 'axios';
import TitleCss from '@ui5/webcomponents/dist/generated/themes/Title.css';
import Calculators from './Calculators';
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
            inputStates: ["None", "None"],
            errorMessages: ["", ""],
            responseErrorMessage: ""
            
        };
        this.homeSubmitButton = React.createRef();
        this.homeLogout = React.createRef();
    }

    validation = (input, index) => {
        const value = parseInt(input);
        // const inputStates = this.state.inputStates.slice();
        // const errorMessages = this.state.errorMessages.slice();
        let inputStates = [...this.state.inputStates];
        let errorMessages = [...this.state.errorMessages];
        let errormsg = "";
        let state = "Error";
        if(input === "") {
            errormsg = "Стойността не може да е празна!";

        }
        else {
            return true;
        }
        inputStates[index] = state;
        errorMessages[index] = errormsg;
        this.setState({ inputStates, errorMessages});
        return false;
    }

    onLoginSubmit = () => {
        const postInformation = {
            "username": `${this.state.loginUsername}`,
            "password": `${this.state.loginPassword}`
        }

        const v0 = this.validation(this.state.loginUsername, 0);
        const v1 = this.validation(this.state.loginPassword, 1);
        if (v0 && v1) {
            axios({
                method: "post",
                url: "http://localhost:5000/api/Authentication/sign-in",
                data: { ...postInformation }
            }).then(res => {
                this.setState({
                    cookie: document.cookie,
                    hasCookie: true,
                    errorInput: ""
                })
                this.props.loadCookie(this.state.loginUsername);
                this.props.history.push(('/calculators'));
            }).catch(err => {
                this.setState({
                    cookie: "",
                    hasCookie: false
                })
            })
        }
    }

    onRegisterSubmit = () => {
        const postInformation = {
            "Username": this.state.registerUsername,
            "Email": this.state.registerEmail,
            "Password": this.state.registerPassword,
            "Password2": this.state.registerRepeatedPassword
        }
        axios({
            method: "post",
            url: "http://localhost:5000/api/Authentication/sign-up",
            data: {...postInformation}
        }).then(res => {

        }).catch(err => {
            console.log(err);
        })
    }

    onInputChange = (event) => {
        const { name, value, } = event.target;
        const inputStates = this.state.inputStates.slice();
        const errorMessages = this.state.errorMessages.slice();
        inputStates[event.target.getAttribute("data-index")] = "None";
        errorMessages[event.target.getAttribute("data-index")] = "";
        this.setState({ inputStates: inputStates, errorMessages: errorMessages});
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
                        <h3 className="entry">Вход</h3>
                            <form>   
                                <div className="login-username">
                                    <input data-index={0} value-state={this.state.inputStates[0]} type="text" name="loginUsername" value={this.state.loginUsername} onChange={this.onInputChange} placeholder="Потребителско име" />
                                    <div className="error-message"><p>{this.state.errorMessages[0]}</p></div>
                                </div>
                                <div className="login-password">
                                    <input data-index={1} value-state={this.state.inputStates[1]} type="password" name="loginPassword" value={this.state.loginPassword} onChange={this.onInputChange} placeholder="Парола" />
                                    <div className="error-message"><p>{this.state.errorMessages[1]}</p></div>
                                    <div className="text-input"><a href="#"><i>Забравена парола?</i></a></div>
                                </div>
                                <ui5-button onClick={this.onLoginSubmit} design="Emphasized">Вход</ui5-button>
                            </form>
                        </div>
                        
                        <div className="form">
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

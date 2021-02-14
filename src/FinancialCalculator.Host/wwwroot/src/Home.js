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
            hasCookie: false
        };
        this.homeSubmitButton = React.createRef();
        this.homeLogout = React.createRef();
    }

    onLoginSubmit = () => {
        const postInformation = {
            "username": `${this.state.loginUsername}`,
            "password": `${this.state.loginPassword}`
        }
        axios({
            method: "post",
            url: "http://localhost:5000/api/Authentication/sign-in",
            data: { ...postInformation }
        }).then(res => {
            this.setState({
                cookie: document.cookie,
                hasCookie: true
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

    onInputChange = (event) => {
        const { name, value } = event.target;
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
                                <ui5-button design="Emphasized">Регистрация</ui5-button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

export default Home;

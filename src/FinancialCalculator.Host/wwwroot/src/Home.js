import React from 'react';
import './Pages.css';
import './Navbar.css';
import axios from 'axios';
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
            cookie: ""
        };
        this.homeSubmitButton = React.createRef();
        this.homeLogout = React.createRef();
    }

    onLoginSubmit = () => {
        console.log("majkaMUSHEBADAEBAMUHATADAEBA")
        const postInformation = {
            "username": `${this.state.loginUsername}`,
            "password": `${this.state.loginPassword}`
        }
        console.log(postInformation);
        axios({
            method: "post",
            url: "http://localhost:5000/api/Authentication/sign-in",
            data: { ...postInformation}
        }).then(res => {
            

            this.setState({
                cookie: document.cookie
            })
            this.props.loadCookie();
        }).catch(err => {
          
            this.setState({
                cookie: ""
            })
        })
    }

    onInputChange = (event) => {
        const { id, value } = event.target;
        this.setState({
                [id]: value
        })

        console.log(this.state.loginUsername, this.state.loginPassword);
    }

    onLogoutSubmit = () => {
        axios({
            method: "post",
            url: "http://localhost:5000/api/Authentication/logout"
        }).then(res => {
            this.setState({
                cookie: ""
            })
        })


    }

    addEventListeners() {
        this.homeSubmitButton.current.addEventListener("click", this.onLoginSubmit);
        this.homeLogout.current.addEventListener("click", this.onLogoutSubmit);
        const inputs = document.querySelectorAll(".account-input", ".register-input");
        const inputsArray = [...inputs];
        
        inputsArray.forEach(input => {
            input.addEventListener("change", this.onInputChange);
        });
    }

    componentDidMount() {
        this.addEventListeners();
        if (document.cookie) {
            this.setState({
                cookie: document.cookie
            })
            this.props.loadCookie();
        } else {
            this.setState({
                cookie: ""
            })
            this.removeEventListeners();
            this.addEventListeners();
        }
    }
    removeEventListeners() {
        this.homeSubmitButton.current.removeEventListener("click", this.onLoginSubmit);
        this.homeLogout.current.removeEventListener("click", this.onLogoutSubmit);
        const inputs = document.querySelectorAll(".account-input", ".register-input");
        const inputsArray = [...inputs];

        inputsArray.forEach(input => {
            input.removeEventListener("change", this.onInputChange);
        });
    }
    componentDidUpdate() {
      
    }

    render() {
        return (
            <div>
                {this.state.cookie !== "" ?
                    <p>{"Здравей, " + this.state.trueUser + "!"}</p> :
 
                    <div className="container">
                        <div className="account-form">
                            <ui5-card heading="Вход" class="small">
                                <div class="account-input">
                                    <div className="account-input-username">
                                        <ui5-input id="loginUsername" placeholder="Потребителско име" required></ui5-input>
                                    </div>
                                    <div className="account-input-password">
                                        <ui5-input id="loginPassword" type="Password" placeholder="Парола" required></ui5-input>
                                        <div className="text-input">
                                            <a href="#">
                                                <i>Забравена парола?</i>
                                            </a>
                                        </div>
                                    </div>
                                    <div className="account-button-actions">
                                        <ui5-button ref={this.homeSubmitButton} design="Emphasized">Вход</ui5-button>

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
                    </div> }
                <ui5-button ref={this.homeLogout} design="Emphasized">Изход</ui5-button>
            </div>
        );
    }
}

export default Home;

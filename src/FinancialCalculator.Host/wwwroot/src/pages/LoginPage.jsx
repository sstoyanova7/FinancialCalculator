import React, {Component} from 'react';
import LoginForm from '../components/LoginComponent/LoginForm/LoginForm'

class LoginPage extends Component {
    constructor() {
        super()

        this.state = {
            username: "",
            password: ""
    
    }
    
    this.handleChange = this.handleChange.bind(this);
    this.onSubmit = this.onSubmit.bind(this);
}

onSubmit(event) {
       
    console.log(this.state.username, this.state.password) 
    event.preventDefault()
    
}

handleChange(event) {
    const {name, value} = event.target
    this.setState({
        [name] : value
    })
}
    render() {  
        return (
            <LoginForm 
                handleChange={this.handleChange}
                onSubmit={this.onSubmit}
                username={this.state.username}
                password={this.state.password}
            />
        )
    }
    
}

export default LoginPage;
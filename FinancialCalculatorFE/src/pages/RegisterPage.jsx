import React, {Component} from 'react';
import RegisterForm from '../components/RegisterComponent/RegisterForm'

class RegisterPage extends Component {
    constructor() {
        super()
        this.state = {
            newUsername: "",
            newPassword: "",
            confirmPassword: "",
            newEmailAddress: ""
        }
        this.handleChange = this.handleChange.bind(this);
        this.onSubmit = this.onSubmit.bind(this);
    }

    handleChange(event) {
        const {name, value} = event.target
        this.setState({
            [name] : value
        })
        
    }

    onSubmit(event) {
        event.preventDefault()  
        console.log(this.state.newUsername, this.state.newPassword, this.state.confirmPassword, this.state.newEmailAddress)

        
    }
    render() {
        return (
            <div>
                <RegisterForm 
                    handleChange={this.handleChange}
                    onSubmit={this.onSubmit}
                    newUsername={this.state.newUsername}
                    newPassword={this.state.newPassword}
                    confirmPassword={this.state.confirmPassword}
                    newEmailAddress={this.state.newEmailAddress}
                    
                />
            
            </div>
        )
    }
}

export default RegisterPage;
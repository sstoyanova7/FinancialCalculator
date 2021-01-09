import React from 'react';
import {Link} from 'react-router-dom'
import './RegisterForm.css'

function RegisterForm(props) {
   

    
        return(
            <div>
                <div className="header">
                    <nav className="navbar">
                        <h1>Financial Calculator</h1>
                        <ul>
                            <li> <Link to="/">Home</Link> </li>
                            <li> <Link to="/login">Login</Link> </li>
                        </ul>
                    </nav>  
                    <div className="container">
                        <div className="form-wrap">
                            <h1 style={{fontSize:40}}>Sign Up</h1>
                            <form onSubmit={props.onSubmit}>     
                                <div className="form-group">

                                    <label>Username</label>
                                    <input 
                                        type="text"
                                        value={props.newUsername}
                                        name="newUsername"
                                        onChange={props.handleChange}
                                    />
                                </div>
                                <div className="form-group">

                                    <label>Password</label>
                                    <input 
                                        type="text"
                                        value={props.newPassword}
                                        name="newPassword"       
                                        onChange={props.handleChange}  
                                    />

                                </div>
                                <div className="form-group">

                                    <label style={{display:"block"}}>Repeat Passowrd</label>
                                    <input 
                                        type="text"
                                        value={props.confirmPassword}
                                        name="confirmPassword" 
                                        onChange={props.handleChange}  
                                    />

                                </div>
                                
                                <div className="form-group">

                                    <label style={{display:"block"}} >Email Address</label>
                                    <input 
                                        type="text"
                                        value={props.newEmailAddress}
                                        name="newEmailAddress"
                                        onChange={props.handleChange}  
                                    />
                                    
                                </div>
                                
                                <button className="btn">Sign In</button>     
                            </form>
                            <p style={{color: "#fff", fontSize: 20}}>Already have an account?
                                <Link style={{color:" rgba(0,0,250,1)"}} to="/login"> Sign in here</Link>
                            </p>
                           
                        </div>
                    </div>
                    <div>
                        <svg className="waves"
                        viewBox="0 24 150 28" preserveAspectRatio="none" 
                        shapeRendering="auto">
                            <defs>
                                <path 
                                    id="gentle-wave" 
                                    d="M-160 44c30 0 58-18 88-18s 58 18 88 18 58-18 88-18 58 18 88 18 v44h-352z"
                                />
                            </defs>
                            <g className="parallax">
                                <use xlinkHref="#gentle-wave" x="48" y="0" fill="rgba(255,255,255,0.7" />
                                <use xlinkHref="#gentle-wave" x="48" y="3" fill="rgba(255,255,255,0.5)" />
                                <use xlinkHref="#gentle-wave" x="48" y="5" fill="rgba(255,255,255,0.3)" />
                                <use xlinkHref="#gentle-wave" x="48" y="7" fill="#fff" />
                            </g>
                        </svg>
                    </div>
                </div>
            </div>
        )
}


export default RegisterForm;
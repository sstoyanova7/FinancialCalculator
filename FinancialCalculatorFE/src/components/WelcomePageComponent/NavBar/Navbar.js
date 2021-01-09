import React from "react";
import "./Navbar.css"
import {Link} from 'react-router-dom'
function Navbar() {

    return(
            <nav className="navbar">
           <h1>Financial Calculator</h1>
           <ul>
               <li> <Link to="/login">Login</Link> </li>
               <li> <Link to="/register">Sign up</Link> </li>
           </ul>
       </nav>

       
       
    )
}

export default Navbar;
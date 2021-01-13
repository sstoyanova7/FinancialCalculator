import React from "react";
import "./Showcase.css"
import Navbar from '../NavBar/Navbar';
import {Link} from 'react-router-dom';

function Showcase() {

    return (
        <div>
            <div className="header">
                <Navbar />
                <div className="inner-header flex"> 
                    <div className="inner-header-content">
                    <h1 >Welcome to the most prestigious</h1>
                    <h1>Financial Calculator</h1>
                        <div className="inner-header-button">
                            <Link to="/register">Get Started</Link> 
                        </div>
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
        </div >
    )
}

export default Showcase;
import React from 'react';
import {Link} from 'react-router-dom';
import "./LeasingCalculator.css"

function LeasingCalculator(props) {

   
    const errors = props.errorMessages.map((err, index) => {
        const errorLength = props.errorMessages.length;
        if (errorLength === index + 1) {
            return (
                <div className="error-wrap" key={index}>
                    <p>{err}</p>
                </div>
                )
        } else {
            return (
                <div className="error-wrap" key={index}>
                    <p>{err}</p>
                    <hr />
                </div>
                )
        }
        
    })

    return(
        <div>
            <div className="header">
                <nav className="navbar">
                        <h1>Leas Calculator</h1>
                        <ul> 
                            <li> <Link to="/">Other Clac</Link> </li>
                            <li> <Link to="/login">Other Calc</Link> </li>
                            <li> <Link to="/login">Logout</Link> </li>
                        </ul>
                </nav>  
                <div className="container">
                    <div className="tooltip">
                        <p style={{ display: props.isVisible }}>Invalid Data. Hover me.</p>

                        <span className="tooltiptext">{errors}</span>
                        
                    </div>
                    <form className="calculator-form" onSubmit={props.handleSubmit}>
                       
                        
                        <label>Product Price</label>
                        <input  
                            type="text"
                            value={props.ProductPrice}
                            placeholder={props.placeholder}
                            name="ProductPrice"
                            onChange={props.handleChange}     
                        />

                         <label>Starting Installment</label>
                        <input 
                            type="text"
                            value={props.StartingInstallment}
                            name="StartingInstallment"
                            onChange={props.handleChange}
                        />
                         <label>Lease Period (In Months)</label>
                        <input 
                            type="text"
                            value={props.Period}
                            name="Period"
                            onChange={props.handleChange}
                            
                        />
                         <label>Monthly Installment</label>
                        <input 
                            type="text"
                            value={props.MonthlyInstallment}
                            name="MonthlyInstallment"
                            onChange={props.handleChange}
                        />
                         <label>Starting Fee</label>
                        <input 
                            type="text"
                            value={props.StartingFeeValue}
                            name="StartingFeeValue"
                            onChange={props.handleChange}
                        />
                        <button  className="btn" >Submit</button>
                    </form>
                    
                    <div className="leas-calc-information">
                        <h2>Total Fees: {props.responseInformation.totalFees}</h2>
                        <h2>Total Cost: {props.responseInformation.totalCost}</h2> 
                        <h2>Annual Percent Cost: {props.responseInformation.anualPercentExpense}</h2>
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

export default LeasingCalculator;
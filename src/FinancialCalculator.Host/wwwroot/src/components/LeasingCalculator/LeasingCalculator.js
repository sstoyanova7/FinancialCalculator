import React from 'react';
import {Link} from 'react-router-dom';
import "./LeasingCalculator.css"

function LeasingCalculator(props) {

    return(
        <div>
            <div className="header">
                <nav className="navbar">
                        <h1>Leas Calculator</h1>
                        <ul> {/* change Links to other 2 calculators  
                              valGoods downPayment leaseTerm monthlyPayment initProcessingFee

                              */}
                            <li> <Link to="/">Other Clac</Link> </li>
                            <li> <Link to="/login">Other Calc</Link> </li>
                            <li> <Link to="/login">Logout</Link> </li>
                        </ul>
                </nav>  
                <div className="container">
                    <form className="calculator-form" onSubmit={props.handleSubmit}>
                        <label>Price of goods</label>
                        <input 
                            type="text"
                            value={props.ProductPrice}
                            name="ProductPrice"
                            placeholder="Price of the Goods"
                            onChange={props.handleChange}
                            
                        />
                         <label>Down Payment</label>
                        <input 
                            type="text"
                            value={props.StartingInstallment}
                            name="StartingInstallment"
                            onChange={props.handleChange}
                        />
                         <label>Lease Term in Months</label>
                        <input 
                            type="text"
                            value={props.Period}
                            name="Period"
                            onChange={props.handleChange}
                        />
                         <label>Monthly Payment</label>
                        <input 
                            type="text"
                            value={props.MonthlyInstallment}
                            name="MonthlyInstallment"
                            onChange={props.handleChange}
                        />
                         <label>Initial Processing Fee</label>
                        <input 
                            type="text"
                            value={props.StartingFeeValue}
                            name="StartingFeeValue"
                            onChange={props.handleChange}
                        />
                        <button className="btn" >Submit</button>
                    </form>
                    <div className="leas-calc-information">
                            <h2>Total Fees: {props.totalFees}</h2>
                            <h2>Total Paid with Fees: {props.totalPaidWithFees}</h2> 
                            <h2>Anual Percentage Expenses: {props.anualPercentExpense}</h2>
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
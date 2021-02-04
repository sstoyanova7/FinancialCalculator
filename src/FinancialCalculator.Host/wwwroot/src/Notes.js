import React from 'react';
import './Notes.css';
import ReactApexChart from 'react-apexcharts'
import "@ui5/webcomponents/dist/Calendar";

class Notes extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            series: [45, 35, 20],
            options: {
                labels: ['Домашни електроуреди', 'Автомобили', 'Мебели'],
                chart: {
                    type: 'donut',

                },
                responsive: [{
                    breakpoint: 480,
                    options: {
                        chart: {
                            width: 1000
                        },
                        legend: {
                            position: 'bottom'
                        }
                    }
                }]
            },
        };
    }

    render() {
        return (
            <div className="notes">
                <div className="note">
                <ui5-calendar hide-week-numbers></ui5-calendar>
                </div>
                <div className="note">
                    <h4>Най-често търсена цена на стоки</h4>
                    <p className="note-text">*Според последни проучвания, едни от най-търсените стоки, желани от клиенти, са домашни електроуреди*</p>
                    <ReactApexChart options={this.state.options} series={this.state.series} type="donut" />
                </div>
            </div>
        );
    }
}

export default Notes;

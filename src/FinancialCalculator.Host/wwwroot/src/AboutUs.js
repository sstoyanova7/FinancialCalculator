import React from 'react';
import './Pages.css';
import './AboutUs.css';
import img1 from './images/stoyan.jpg';
import img2 from './images/martin.jpg';
import img3 from './images/simona.jpg';
import img4 from './images/radoslav.jpg';
import img5 from './images/yanica.jpg';
import img6 from './images/teodor.jpg';
import img7 from './images/angel.jpg';
import "@ui5/webcomponents/dist/Card";
import "@ui5/webcomponents/dist/List.js";
import "@ui5/webcomponents/dist/StandardListItem.js";
import "@ui5/webcomponents/dist/CustomListItem.js";

class AboutUs extends React.Component {

    render() {
        return (
		<div>	
            <div className = "page">
            
            	<div className="cards">

					<div className="card-item">
						<ui5-card heading="Стоян Паскалев" subheading="Frontend" class="small">
							<img src={img1} slot="avatar" alt="" />
                            	
							<ui5-list separators="Inner">
								<ui5-li>placeholder</ui5-li>
							</ui5-list>
						</ui5-card>
					</div>
					<div className="card-item">
						<ui5-card heading="Мартин Георгиев" subheading="Frontend" class="small">
							<img src={img2} slot="avatar" alt="" />
                            	
							<ui5-list separators="Inner">
								<ui5-li>F92010</ui5-li>
							</ui5-list>
						</ui5-card>
					</div>

					<div className="card-item">
						<ui5-card heading="Ангел Грънчаров" subheading="Frontend" class="small">
							<img src={img7} slot="avatar" alt="" />
	
							<ui5-list separators="Inner">
								<ui5-li>placeholder</ui5-li>
							</ui5-list>
						</ui5-card>
					</div>
			   
				</div>

			</div>

			<div className = "page">

				<div className="cards">

					<div className="card-item">
						<ui5-card heading="Симона Стоянова" subheading="Backend" class="small">
							<img src={img3} slot="avatar" alt="" />
	
							<ui5-list separators="Inner">
								<ui5-li>placeholder</ui5-li>
							</ui5-list>
						</ui5-card>
					</div>
			   
					<div className="card-item">
						<ui5-card heading="Радослав Костурков" subheading="Backend" class="small">
							<img src={img4} slot="avatar" alt="" />
	
	
							<ui5-list separators="Inner">
								<ui5-li>placeholder</ui5-li>
							</ui5-list>
						</ui5-card>
					</div>

					<div className="card-item">
						<ui5-card heading="Яница Бойковска" subheading="Backend" class="small">
							<img src={img5} slot="avatar" alt="" />
	
							<ui5-list separators="Inner">
								<ui5-li>placeholder</ui5-li>
							</ui5-list>
						</ui5-card>
					</div>
			   
				</div>

			</div>

			<div className = "page">

				<div className="cards">
			   
					<div className="card-item">
						<ui5-card heading="Teodor Petkov" subheading="Documentation" class="small">
							<img src={img6} slot="avatar" alt="" />
	
							<ui5-list separators="Inner">
								<ui5-li>placeholder</ui5-li>
							</ui5-list>
						</ui5-card>
					</div>
			   
				</div>

			</div>

        </div>
        );
    }
}

export default AboutUs;
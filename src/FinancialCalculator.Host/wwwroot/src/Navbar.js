import React from 'react';
import "@ui5/webcomponents-fiori/dist/ShellBar.js";
import "@ui5/webcomponents-fiori/dist/ShellBarItem.js";
import "@ui5/webcomponents-fiori/dist/SideNavigation.js";
import "@ui5/webcomponents-fiori/dist/SideNavigationItem.js";
import "@ui5/webcomponents-fiori/dist/SideNavigationSubItem.js";
import "@ui5/webcomponents-icons/dist/Assets.js";
import './Navbar.css';
import "@ui5/webcomponents/dist/Popover.js";
import "@ui5/webcomponents/dist/Dialog";

class Navbar extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            logged: false,
            userData: {
                profileName: "User"
            }
        }

        this.onNavbarSelect = this.onNavbarSelect.bind(this);
        this.onProfileSelect = this.onProfileSelect.bind(this);
    }

    onNavbarSelect(event) {
        const newRoute = event.detail.item.id;
        this.props.history.push(('/' + newRoute));
    }

    onProfileSelect(event) {
        var selectedItem = event.detail.selectedItems[0].textContent;
        const popover = document.getElementById('popover');
        if (selectedItem === "Изход") {
            this.setState({
                logged: false,
                userData: {
                    profileName: "User"
                }
            });
        }
        else if (selectedItem === "Вход") {
            this.setState({
                logged: true,
                userData: {
                    profileName: "Ekaterina"
                }
            });
            this.props.history.push(('/'));
            
        }
        popover.close();
    }

    addEventListeners() {
        const navigation = document.getElementById('side-navigation');
        navigation.addEventListener("selection-change", this.onNavbarSelect);
        const shellbar = document.getElementById('shellbar');
        shellbar.addEventListener("profile-click", function (event) {
            const popover = document.getElementById('popover');
            popover.openBy(shellbar);
        });
        const profileOptions = document.getElementById('profile-options');
        profileOptions.addEventListener("selection-change", this.onProfileSelect);
        const sideNavigation = document.querySelector("ui5-side-navigation");
	    document.querySelector("#startButton").addEventListener("click", () => sideNavigation.collapsed = !sideNavigation.collapsed);
    }

    componentDidMount() {
        this.addEventListeners();
    }

    render() {
        return (
            <div>
                <ui5-shellbar
                    primary-title="Финансов Калкулатор"
                    secondary-title="Проект за НБУ"
                    show-co-pilot
                    id="shellbar"
                >
                    <ui5-button icon="menu" slot="startButton" id="startButton"></ui5-button>
                    <ui5-avatar slot="profile" icon="customer" id="profile"></ui5-avatar>
                </ui5-shellbar>
                <ui5-popover id="popover" placement-type="Bottom" horizontal-align="Right">
                    <div class="popover-header">
                        <ui5-title>{this.state.userData.profileName}</ui5-title>
                    </div>
                    <div class="popover-content">
                        {this.state.logged == false ?
                            <ui5-list id="profile-options" mode="SingleSelect" separators="None">
                                <ui5-li icon="business-card">Вход</ui5-li>
                                <ui5-li icon="add-contact">Регистрация</ui5-li>
                                <ui5-li icon="sys-help">Информация</ui5-li>
                            </ui5-list>
                            :
                            <ui5-list id="profile-options" mode="SingleSelect" separators="None">
                                <ui5-li icon="settings">Настройки</ui5-li>
                                <ui5-li icon="sys-help">Информация</ui5-li>
                                <ui5-li icon="log">Изход</ui5-li>
                            </ui5-list>
                        }
                    </div>
                </ui5-popover>
                <div className="side">
                    <ui5-side-navigation  id="side-navigation">
                        <ui5-side-navigation-item text="Вход / Регистрация" icon="home-share" id=""></ui5-side-navigation-item>
                        <ui5-side-navigation-item text="Калкулатори" expanded icon="simulate" id="calculators">
                            <ui5-side-navigation-sub-item text="Кредитен" icon="step" id="calculators/credit"></ui5-side-navigation-sub-item>
                            <ui5-side-navigation-sub-item text="Лизингов" icon="step" id="calculators/lizing"></ui5-side-navigation-sub-item>
                            <ui5-side-navigation-sub-item text="Рефинансиране" icon="step" id="calculators/refinance"></ui5-side-navigation-sub-item>
                        </ui5-side-navigation-item>
                        <ui5-side-navigation-item text="История" expanded icon="work-history" id="history">
                        <ui5-side-navigation-sub-item text="Кредитно търсене" icon="history" id="statistics/one"></ui5-side-navigation-sub-item>
                        <ui5-side-navigation-sub-item text="Лизингово търсене" icon="history" id="statistics/two"></ui5-side-navigation-sub-item>
                        <ui5-side-navigation-sub-item text="Рефинансиране" icon="history" id="statistics/three"></ui5-side-navigation-sub-item>
                        </ui5-side-navigation-item>
                        <ui5-side-navigation-item slot="fixedItems" text="За Нас" icon="collaborate" id="about-us"></ui5-side-navigation-item>
                    </ui5-side-navigation>
                </div>
                <ui5-popover id="register-dialog" verticalAlign = "Stretch" horizontal-align="Center">
                    <div class="popover-header">
                        <ui5-title>Регистрация</ui5-title>
                    </div>
                    <div class="popover-content">
                        <section class="login-form">
                            <div>
                                <ui5-label for="email" type="Email" required>Email: </ui5-label>
                                <ui5-input id="email"></ui5-input>
                            </div>
                            <div>
                                <ui5-label for="username" required>Username: </ui5-label>
                                <ui5-input id="username"></ui5-input>
                            </div>
                            <div>
                                <ui5-label for="password" required>Password: </ui5-label>
                                <ui5-input id="password" type="Password" value-state="Error"></ui5-input>
                            </div>
                        </section>
                        <div slot="footer" class="dialog-footer">
                            <div></div>
                            <ui5-button id="closeDialogButton" design="Emphasized">Register</ui5-button>
                            <ui5-button id="closeDialogButton" design="Transparent">Cancel</ui5-button>
                        </div>
                    </div>
                </ui5-popover>
                <ui5-popover id="sign-in-dialog" placement-type="Bottom" horizontal-align="Right">
                    <div class="popover-header">
                        <ui5-title>Вход</ui5-title>
                    </div>
                    <div class="popover-content">
                        <section class="login-form">
                            <div>
                                <ui5-label for="username" required>Потребител: </ui5-label>
                                <ui5-input id="username"></ui5-input>
                            </div>
                            <div>
                                <ui5-label for="password" required>Име: </ui5-label>
                                <ui5-input id="password" type="Password" value-state="Error"></ui5-input>
                            </div>
                        </section>
                        <div slot="footer" class="dialog-footer">
                            <div></div>
                            <ui5-button id="closeDialogButton" design="Emphasized">Вход</ui5-button>
                            <ui5-button id="closeDialogButton" design="Transparent">Затвори</ui5-button>
                        </div>
                    </div>
                </ui5-popover>
            </div>
        );
    }
}

export default Navbar;

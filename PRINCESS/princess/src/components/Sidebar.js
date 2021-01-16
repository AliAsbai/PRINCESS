import React, { Component } from "react"
import "../style/Sidebar.css"
import { Menu, Search, Domain, Assignment } from '@material-ui/icons';
import { withRouter } from "react-router-dom";

/**
 * Authors: Ali Asbai, Clara Hällgren
 * 
 * 
 * */

class Sidebar extends Component {
    addActiveClass;

    constructor(props) {
        super(props);
        this.toggleClass = this.toggleClass.bind(this);
        this.state = {
            toggle: false,
        };
    }

    toggleClass(event) {
        event.preventDefault();
        this.setState({ toggle: !this.state.toggle });
    };

    routeSearch = () => {
        const {history} = this.props;
        history.push("/Princess");
    };

    routeDomain = () => {
        const { history } = this.props;
        history.push("/Domain");
    };

    render() {
        const { toggle } = this.state
        return (
            <div id={toggle === true ? 'open' : 'close'} className="sidebar">
                <button id={toggle === true ? 'open' : 'close'} className="row" onClick={this.toggleClass}>Menu <Menu style={{ fontSize: 40 }} /></button>
                <button id={toggle === true ? 'open' : 'close'} className="row" onClick={this.routeSearch}>Search <Search style={{ fontSize: 40 }} /></button>
                <button id={toggle === true ? 'open' : 'close'} className="row" onClick={this.routeDomain}>Domain <Domain style={{ fontSize: 40 }} /></button>
                <button id={toggle === true ? 'open' : 'close'} className="row">Edit <Assignment style={{ fontSize: 40 }} /></button>
            </div>
        )
    }
}

export default withRouter(Sidebar)
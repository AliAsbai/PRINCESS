import React, { useContext } from "react";
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import { Link, useHistory } from "react-router-dom";
import '../style/Header.css';
import { UserContext } from "../components/UserContext";

/**
 * Authors: Ali Asbai, Anna Mann, Clara Hällgren, Gabriel Vega
 * 
 * 
 * */

function Header() {
    const history = useHistory();                   // Redirect

    const { user, setUser } = useContext(UserContext);

    const handleLogout = (evt) => {
        evt.preventDefault();

        var xml = new XMLHttpRequest();
        xml.open('GET', 'https://localhost:44307/User/logout', true);
        xml.responseType = 'json';
        xml.setRequestHeader('Authorization', 'Bearer ' + sessionStorage.getItem('Token'));

        xml.onload = () => {
            if (xml.status >= 400) {
                alert(xml.response);
            }
            else {
                sessionStorage.clear();
                history.push("/");                  // Redirect
            }
        };
        xml.send();

        return (null)
    }

    return (
        <div id="header-wrapper">
            < nav className="navbar navbar-expand-lg navbar-light fixed-top" id="header-wrapper">
                <h2>PRINCESS</h2>
                <div className="container">
                    <div className="collapse navbar-collapse" id="navbarTogglerDemo02">
                        <ul className="navbar-nav ml-auto">
                            <li className="nav-item">
                                <Link className="nav-link" to={"/"}>
                                    {user ? (<button type="logout"
                                        onClick={async (evt) => {
                                            const user = await handleLogout(evt);
                                            setUser(user);
                                        }}>
                                        Logout
                                    </button>) :
                                        (<button
                                            type="login">Login
                                        </button>)}
                                </Link>
                            </li>
                            <li className="nav-item">
                            </li>
                        </ul>
                    </div>
                </div>
            </nav >
        </div>
    )
}

export default Header;

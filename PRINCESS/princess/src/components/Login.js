import React, { useState, useContext } from "react";
import { useHistory } from "react-router-dom";
import '../style/Login.css';
import { UserContext } from "../components/UserContext";

/**
 * Authors: Ali Asbai, Clara Hällgren, Gabriel Vega
 * 
 * This file returns what to be rendered for the login page and the website.
 * On submit it shows what email and password that was entered and redirects.
 * 
 * */

function Login() {

    const [email, setEmail] = useState("");         // Hook for email
    const [password, setPassword] = useState("");   // Hook for password
    const history = useHistory();                   // Redirect
    const { user, setUser } = useContext(UserContext); // Hook for user

    const handleSubmit = (evt) => {
        evt.preventDefault();


        var login = {
            Username: email,
            Password: password,
        };

        var xml = new XMLHttpRequest();
        xml.open('POST', 'https://localhost:44307/User/authenticate', true);
        xml.responseType = 'json';
        xml.setRequestHeader('Content-Type', 'application/json');



        xml.onload = () => {
            if (xml.status >= 400) {
                if (xml.response.message != null) {
                    alert(xml.response.message);
                } else {
                    alert(JSON.stringify(xml.response.errors));
                }

            }
            else {
                setUser({
                    Username: xml.response.value.email,
                    Name: xml.response.value.name
                });
                sessionStorage.setItem('User', JSON.stringify(xml.response.value));
                sessionStorage.setItem('Token', xml.response.value.token);
                history.push("/Princess");                  // Redirect
            }
        };

        xml.send(JSON.stringify(login));
    }

    return (
        <div className="auth-wrapper">
            <div className="auth-inner">
            <form onSubmit={handleSubmit}>
                <h3>Sign In</h3>

                <div className="form-group">
                    <label>Email address</label>
                    <input
                        type="email"
                        className="form-control"
                        placeholder="Enter email"
                        value={email}              // Value type def
                        onChange={e => setEmail(e.target.value)}    //set value
                     />
                </div>

                <div className="form-group">
                    <label>Password</label>
                    <input
                        type="password"
                        className="form-control"
                        placeholder="Enter password"
                        value={password}
                        onChange ={e => setPassword(e.target.value)}
                      />
                    </div>

                <button
                    type="submit"
                    className="btn btn-primary btn-block"
                        onClick={async (evt) => {
                            const user = await handleSubmit(evt);
                        }}>
                    Submit
                </button>
              </form>
            </div>
        </div>

        );
}

export default Login;

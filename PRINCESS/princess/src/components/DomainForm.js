import React, { useState } from "react";
import Form from 'react-bootstrap/Form';
import { useHistory } from "react-router-dom";
import '../style/DomainForm.css';

import MenuItem from '@material-ui/core/MenuItem';
import Info from '@material-ui/icons/Info';
import Tooltip from '@material-ui/core/Tooltip';
import TextField from '@material-ui/core/TextField';

/**
 * authors: Ali Asbai, Anna Mann, Clara Hällgren, Olivia Höft
 *          
 * */

function DomainForm() {

    const [title, setTitle] = useState("");         // Hook for title
    const [writer, setWriter] = useState("");   // Hook for writer
    const [review, setReview] = useState("");   // Hook for review
    const [rating, setRating] = useState("");   // Hook for rating
    const [menu, setMenu] = useState("");   // Hook for menu
    const [URL, setURL] = useState("");   // Hook for url
    const [quotes, setQuotes] = useState(false);
    const [priority, setPriority] = useState(0);
    const [nextButton, setNextButton] = useState(false);
    const [nbValue, setNbValue] = useState("");
    const [paywall, setPaywall] = useState(false);
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [ignoreTitleKeyProp, setIgnoreTitleKeyProp] = useState(false);
    const [itkValue, setItkValue] = useState("");
    const [country, setCountry] = useState("");

    const history = useHistory();                   // Redirect

    const toggleQuotes = (event) => {
        setQuotes(!quotes);
    }

    const togglePaywall = (event) => {
        setPaywall(!paywall);
        setLogin("");
        setPassword("");
    }

    const toggleNext = (event) => {
        setNextButton(!nextButton);
    }

    const toggleIgnore = (event) => {
        setIgnoreTitleKeyProp(!ignoreTitleKeyProp);
    }

    const handleSubmit = (evt) => {
        evt.preventDefault();

        var domainForm = {
            Title: title,
            Writer: writer,
            Review: review,
            Rating: rating,
            Menu: menu,
            URL: URL,
            Quotes: quotes,
            NextButton: nextButton,
            NbValue: nbValue,
            IgnoreTitleKeyProp: ignoreTitleKeyProp,
            ItkValue: itkValue,
            Country: country,
            Priority: priority,
            Paywall: paywall,
            Login: login,
            Password: password,
        };

        var xml = new XMLHttpRequest();
        xml.open('POST', 'https://localhost:44307/Domain/Domain', true);
        xml.responseType = 'json';
        xml.setRequestHeader('Content-Type', 'application/json');
        xml.setRequestHeader('Authorization', 'Bearer ' + sessionStorage.getItem('Token'));

        xml.onload = () => {
            if (xml.status >= 400) {
                alert("Something went wrong, did you enter input on all the required fields?");
            }
            else {
                history.push("/Princess")             // Redirect
            }
        };

        xml.send(JSON.stringify(domainForm));
    }

    return (
        <div className="domain-wrapper">
            <div className="domain-inner">
                <form onSubmit={handleSubmit}>
                    <h3>Add domain</h3>

                    <TextField
                        label="Title" required
                        className="form-control-short"
                        helperText="Enter code for title from domain"
                        margin="dense"
                        variant="outlined"
                        value={title}
                        onChange={e => setTitle(e.target.value)}
                    />

                    <TextField
                        label="Writer" required
                        className="form-control-short"
                        helperText="Enter code for writer from domain"
                        margin="dense"
                        variant="outlined"
                        value={writer}
                        onChange={e => setWriter(e.target.value)}
                    />

                    <TextField
                        label="Review" required
                        className="form-control-short"
                        helperText="Enter code for review from domain"
                        margin="dense"
                        variant="outlined"
                        value={review}
                        onChange={e => setReview(e.target.value)}
                    />

                    <TextField
                        label="Rating" required
                        className="form-control-short"
                        helperText="Enter code for rating from domain"
                        margin="dense"
                        variant="outlined"
                        value={rating}
                        onChange={e => setRating(e.target.value)}
                    />

                    <TextField
                        label="Menu" required
                        className="form-control-short"
                        helperText="Enter code for menu from domain"
                        margin="dense"
                        variant="outlined"
                        value={menu}
                        onChange={e => setMenu(e.target.value)}
                    />

                    <TextField
                        label="URL" required
                        className="form-control-short"
                        helperText="Enter URL from domain where reviews are found"
                        margin="dense"
                        variant="outlined"
                        value={URL}
                        onChange={e => setURL(e.target.value)}
                    />

                    <TextField
                        select
                        label="Priority" required
                        value={priority}
                        onChange={e => setPriority(e.target.value)}
                        helperText="Please select priority for domain"
                        variant="outlined"
                        margin="dense"
                    >
                        <MenuItem value={1}>1</MenuItem>
                        <MenuItem value={2}>2</MenuItem>
                        <MenuItem value={3}>3</MenuItem>
                        <MenuItem value={4}>4</MenuItem>
                        <MenuItem value={5}>5</MenuItem>
                    </TextField>

                    <TextField
                        select
                        label="Country" required
                        value={country}
                        onChange={e => setCountry(e.target.value)}
                        helperText="Please select country of domain"
                        variant="outlined"
                        margin="dense"
                    >
                        <MenuItem value="Sweden">Sweden</MenuItem>
                        <MenuItem value="Denmark">Denmark</MenuItem>
                    </TextField>

                    <Form.Group id="custom-switch">
                        <Form.Check type="switch"
                            label="Remove characters"
                            id="custom-switch-1"
                            onClick={(event) => toggleQuotes(event)}
                        />
                        <Tooltip className="btn-info" title="If any of the attributes have characters that needs to be removed, this switch needs to be active">
                            <Info style={{ fontSize: 17 }} />
                        </Tooltip>
                    </Form.Group>

                    <Form.Group id="custom-switch">
                        <Form.Check type="switch"
                            id="custom-switch-2"
                            label="Next Button"
                            onClick={(event) => toggleNext(event)}
                        />
                        <Tooltip className="btn-info" title="If the menu are divided in pages and you need to press a button to come to the next page, this switch needs to be active">
                            <Info style={{ fontSize: 17 }} />
                        </Tooltip>
                    </Form.Group>

                    <Form.Group id={!nextButton ? "hideTextBox" : ""}>
                        <Form.Control placeholder="Enter code for next button"
                            value={nbValue}
                            onChange={e => setNbValue(e.target.value)}
                        />
                    </Form.Group>

                    <Form.Group id="custom-switch">
                        <Form.Check type="switch"
                            id="custom-switch-3"
                            label="Paywall"
                            onClick={(event) => togglePaywall(event)}
                        />
                        <Tooltip className="btn-info" title="If the domain has a paywall, this switch needs to be active">
                            <Info style={{ fontSize: 17 }} />
                        </Tooltip>
                    </Form.Group>

                    <Form.Group id={!paywall ? "hideTextBox" : ""}>
                        <Form.Control placeholder="Enter login"
                            value={login}
                            onChange={e => setLogin(e.target.value)}
                        />
                        <Form.Control placeholder="Enter password"
                            value={password}
                            onChange={e => setPassword(e.target.value)}
                        />
                    </Form.Group>

                    <Form.Group id="custom-switch">
                        <Form.Check type="switch"
                            id="custom-switch-4"
                            label="Ignore title key properties"
                            onClick={(event) => toggleIgnore(event)}
                        />
                        <Tooltip className="btn-info" title="If the menu has other links than reviews you need to find the common code for these links, this switch needs to be active">
                            <Info style={{ fontSize: 17 }} />
                        </Tooltip>
                    </Form.Group>

                    <Form.Group id={!ignoreTitleKeyProp ? "hideTextBox" : ""}>
                        <Form.Control placeholder="Enter code for ignore title key"
                            value={itkValue}
                            onChange={e => setItkValue(e.target.value)}
                        />
                    </Form.Group>

                    <button
                        type="submit"
                        className="btn btn-primary btn-block"
                        onClick={handleSubmit}>
                        Submit
                </button>
                </form>
            </div>
        </div>
    );
}

export default DomainForm;
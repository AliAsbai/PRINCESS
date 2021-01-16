import React, { Component, useState, useReducer, useEffect, useMemo } from "react";
import { makeStyles } from '@material-ui/core/styles';
import Paper from '@material-ui/core/Paper';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableContainer from '@material-ui/core/TableContainer';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import '../style/Frontpage.css';
import { ExitToApp, Delete, Edit, NavigateNext, Save, DeleteForever } from '@material-ui/icons';
import movies from './JsonData/reviews.json';
import Form from 'react-bootstrap/Form'
import nl2br from 'react-newline-to-break';

/**
 * Authors: Ali Asbai, Clara Hällgren, Gabriel Vega
 * 
 * This component renders a preview of the resulting reviews from the belonging database after a search is performed
 * Language: React.js
 * Depentet on reviews.json
 * */

const columns = [
    {
        id: 'Title',
        label: 'Title',
        minWidth: 50
    },
    {
        id: 'Url',
        label: 'Url',
        minWidth: 50
    },
    {
        id: 'Rating',
        label: 'Rating',
        minWidth: 50,
        align: 'right',
    },
    {
        id: 'Writer',
        label: 'Writer',
        minWidth: 50,
        align: 'right',

    },
    {
        id: 'PublishDate',
        label: 'Publish Date',
        minWidth: 50,
        align: 'right',
    }
];

const Qcolumns = [
    {
        id: 'QuoteText',
        label: 'Quote',
        minWidth: 50
    }];

const rows = [

];

const useStyles = makeStyles({
    root: {
        width: '80%',
        marginLeft: 150,
        marginTop: 40,
    },
    container: {
        maxHeight: 460,
    },
});

export default function StickyHeadTable() {
    const classes = useStyles();
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(10);

    const [isVisible, setVisibility] = React.useState(false);

    const [currentReview, setCurrentReview] = React.useState({});

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(+event.target.value);
        setPage(0);
    };

    const [searchWord, setSearchWord] = useState("");
    const [searchBy, setSearchBy] = useState("");

    const [newQuote, setNewQuote] = React.useState("");
    const [sortBy, setSortBy] = useState(sessionStorage.getItem("Sort"));

    const handleSubmit = (evt) => {
        var searchKeys = {
            searchString: searchWord,
            searchType: searchBy,
            sortBy: sortBy
        };

        evt.preventDefault();
        var xml = new XMLHttpRequest();
        xml.open('POST', 'https://localhost:44307/api/Database');
        xml.setRequestHeader('Content-Type', 'application/json');
        xml.setRequestHeader('Authorization', 'Bearer ' + sessionStorage.getItem('Token'));
        xml.send(JSON.stringify(searchKeys));
    }

    const previewReview = (review) => {
        setVisibility(!isVisible);
        setCurrentReview(review);
        setNewQuote("");
    }

    const cancelPreview = () => {
        setQuotesArray([]);
    }

    const [quotesArray, setQuotesArray] = React.useState([]);

    const addQuotesArray = (item) => {
        setQuotesArray(quotesArray.concat(item));
    }

    const removeQuote = (quoteKey) => {
        var list = quotesArray.filter(quote => quote.Quote !== quoteKey);
        setQuotesArray(list);
    }

    const editQuote = (quote) => {
        removeQuote(quote.Quote);
        setNewQuote(quote.Quote);
        setCurrentReview(quote.Review);
    }

    const saveQuotes = () => {

        var xml = new XMLHttpRequest();
        xml.open('POST', 'https://localhost:44307/Quote/activeQuote', true);
        xml.responseType = 'json';
        xml.setRequestHeader('Content-Type', 'application/json');
        xml.setRequestHeader('Authorization', 'Bearer ' + sessionStorage.getItem('Token'));

        xml.onload = () => {
            if (xml.status >= 400) {
                alert(xml.response.message);
            }
        };

        xml.send(JSON.stringify(quotesArray));
        cancelPreview();
    }

    const browseNextReview = () => {

        var i = movies.indexOf(currentReview);
        var l = movies.length;
        setNewQuote("");
        if (i < l - 1) setCurrentReview(movies[(i + 1) % l]);
        else setCurrentReview(movies[l-1]);
    }
    const browsePrevReview = () => {

        var i = movies.indexOf(currentReview);
        var l = movies.length;
        setNewQuote("");
        if (i > 0) setCurrentReview(movies[(i - 1) % l]);
        else setCurrentReview(movies[0]);
    }

    const handleNewQuote = (quote, review) => {
        var Quote = [{
            Review: review,
            Quote: quote,
        }];
        addQuotesArray(Quote);
        setNewQuote("");
    }

    const handleSortBy = (evt) => {
        evt.preventDefault();
        const sortBy = evt.target.value;
        sessionStorage.setItem("Sort", sortBy);
        setSortBy(sortBy);

        var xml = new XMLHttpRequest();
        xml.open('POST', 'https://localhost:44321/api/Database/sortBy');
        xml.setRequestHeader('Content-Type', 'application/json');
        xml.setRequestHeader('Authorization', 'Bearer ' + sessionStorage.getItem('Token'));

        xml.onload = (evt) => {
            evt.preventDefault();
            if (xml.status >= 400) {
                alert(xml.response);
            }
        };

        xml.send(JSON.stringify(sortBy));

    };
    
    return (
        <React.Fragment>
            <div id={!isVisible ? "previewWindow" : ""} className="previewContainer">
                <div id="previewBody">
                    <ExitToApp id="exitButton" style={{ fontSize: 35 }} onClick={(event) => previewReview(event, 'AliAsbai')} />

                    <Form className="quoteContainer">
                        <Form.Group className="quoteForm" controlId="exampleForm.ControlInput1">
                            <Form.Label>Title</Form.Label>
                            <Form.Control readOnly className="data" value={currentReview.Title} />
                        </Form.Group>
                        <Form.Group className="quoteForm" controlId="exampleForm.ControlInput2">
                            <Form.Label>URL</Form.Label>
                            <Form.Control readOnly className="data" value={currentReview.Url} />
                        </Form.Group>
                        <Form.Group className="quoteForm" controlId="exampleForm.ControlTextarea1">
                            <Form.Label>Review</Form.Label>
                            <Form.Control readOnly className="data reviewBox" as="textarea" rows="12" value={currentReview.ReviewText} />
                        </Form.Group>
                        <Form.Group className="quoteForm" controlId="exampleForm.ControlInput3">
                            <Form.Label>Writer</Form.Label>
                            <Form.Control readOnly className="data" value={currentReview.Writer} />
                        </Form.Group>
                        <Save id="browseButton" style={{ fontSize: 35 }} className="btn-submit-quote" onClick={(event) => saveQuotes()}/>
                        <DeleteForever id="browseButton" style={{ fontSize: 35 }} className="btn-DeleteAll" onClick={(event) => cancelPreview()}/>
                        <NavigateNext id="browseButton" style={{ fontSize: 35 }} className="btn-browse" onClick={(event) => browseNextReview()} />
                        <NavigateNext id="browseButton" style={{ fontSize: 35 }} className="btn-browse-prev" onClick={(event) => browsePrevReview()} />
                    </Form>

                    <Form.Group id="newQuote" controlId="exampleForm.ControlTextarea2">
                        <Form.Label>New qoute</Form.Label>
                        <Form.Control className="data quoteBox" as="textarea" rows="6" value={newQuote} onChange={e => setNewQuote(e.target.value)} />
                        <button id="addQuoteButton" onClick={(event) => handleNewQuote(newQuote, currentReview)}>
                            Submit
                        </button>
                    </Form.Group>

                    < TableContainer id="activeQuotes">

                        < Table stickyHeader aria-label="sticky table" >

                            < TableHead id="tableHead" >
                                <TableRow>
                                    <th>Active quotes</th>
                                </TableRow>
                            </ TableHead >
                            < TableBody >
                                {
                                    quotesArray.slice().map((row) => {
                                        return (
                                            <React.Fragment>
                                                < TableRow hover role="checkbox" id="holdingQuotes" tabIndex={-1}
                                                    key={row.Quote}
                                                    id="tableRow"
                                                >
                                                    {
                                                        Qcolumns.map((column) => {
                                                            const value = row.Review.Title + "\n=====\n" + row.Quote + "\n=====\n-" + row.Review.Writer + "\n Länk:" + row.Review.Url;
                                                            return (
                                                                < TableCell key={column.id}
                                                                    align={column.align}>
                                                                    {nl2br(value)}
                                                                </ TableCell >
                                                            );
                                                        })}
                                                    <Delete style={{ fontSize: 35 }} id="quoteButton" onClick={(event) => removeQuote(row.Quote)} />
                                                    <Edit style={{ fontSize: 35 }} id="quoteButton" onClick={(event) => editQuote(row)} />
                                                </ TableRow >
                                            </React.Fragment>
                                        );
                                    })}
                            </TableBody>
                        </Table>
                    </TableContainer>
                </div>
            </div>
            < Paper id={isVisible ? "previewWindow" : ""} className={classes.root} >
                <form onSubmit={handleSubmit}>
                    <div className="form-group">
                        <input
                            type="searchWord"
                            className="form-control"
                            placeholder="Search..."
                            value={searchWord}
                            onChange={e => setSearchWord(e.target.value)}

                        />
                    </div>
                </form>
                <select
                    value={searchBy}
                    onChange={e => setSearchBy(e.target.value)}>
                    <option value="Title">Title</option>
                    <option value="Domain">Domain</option>
                    <option value="Writer">Writer</option>
                </select>
                <select
                    value={sortBy}
                    onChange={e => setSortBy(e.target.value)}>
                    <option>Sort By...</option>
                    <option value="PublishDate">Publish Date</option>
                    <option value="Priority">Priority</option>
                </select>
                < TableContainer className={classes.container}>

                    < Table stickyHeader aria-label="sticky table" >

                        < TableHead id="tableHead">

                            < TableRow >
                                {
                                    columns.map((column) => (
                                        < TableCell
                                            key={column.id}
                                            align={column.align}
                                            style={{ minWidth: column.minWidth }}
                                        >
                                            {column.label}
                                        </ TableCell >
                                    ))}
                            </ TableRow >
                        </ TableHead >
                        < TableBody >
                            {
                                movies.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map((row) => {
                                    return (

                                        < TableRow hover role="checkbox" tabIndex={-1}
                                            key={row.code}
                                            id="tableRow" onClick={(event) => previewReview(row)}>
                                            {
                                                columns.map((column) => {
                                                    const value = row[column.id];
                                                    return (

                                                        < TableCell key={column.id}
                                                            align={column.align}>
                                                            {column.format && typeof value === 'number' ? column.format(value) : value}
                                                        </ TableCell >

                                                    );
                                                })}
                                        </ TableRow >
                                    );
                                })}
                        </TableBody>
                    </Table>
                </TableContainer>
            </Paper>
        </React.Fragment>
    );
}

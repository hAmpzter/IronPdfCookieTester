import React, { Component } from 'react';
import fetchInject from '../utils/fetchInject';

export class FetchData extends Component {
    static displayName = FetchData.name;

    constructor(props) {
        super(props);
        this.state = { cookies: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    async getPdf() {
        const response = await fetch('print');
        var file = await response.blob().catch(error => {
            console.log(error);
        });

        const url = window.URL.createObjectURL(file);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'file.pdf');
        document.body.appendChild(link);
        link.click();
    }

    static renderCookiesTable(cookies) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel" id="test1">
                <thead>
                    <tr>
                        <th>Index</th>
                        <th>Key</th>
                        <th>Value</th>
                    </tr>
                </thead>
                <tbody>
                    {cookies.map(cookie =>
                        <tr key={cookie.index}>
                            <td>{cookie.index}</td>
                            <td>{cookie.key}</td>
                            <td>{cookie.value}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchData.renderCookiesTable(this.state.cookies);

        return (
            <div>
                <h1 id="tabelLabel" >Cookies</h1>
                <button className="btn btn-primary" onClick={this.getPdf}>Download PDF</button>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    async populateData() {
        fetchInject([
            '/scripts/fetchInject',
        ])
        const response = await fetch('cookies');
        const data = await response.json();
        this.setState({ cookies: data, loading: false });
    }
}

import React, { Component } from 'react';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = { unpData: {}, loading: true, unpNum: "", unpExist: false };

        this.handleChange = this.handleChange.bind(this);
        this.renderForm = this.renderForm.bind(this);
    }

    handleChange(event) {
        const exist = this.checkIfUnpExist(event.target.value);

        this.setState({ unpNum: event.target.value, unpExist: exist.unpExist });
    }

    componentDidMount() {
        this.fetchUnp();
    }   

    static renderUnp(unp) {
        return (
            <p>{ unp.vnaimp }</p>
            )
    }

    renderForm() {
        return (
            <form>
                <label>
                    UNP:
                    <input type="number" value={this.state.unpNum} onChange={this.handleChange} />
                </label>
                <p>Unp exist in db: {this.state.unpExist === true ? "Yes" : "No"}</p>
            </form>
            )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : App.renderUnp(this.state.unpData);

        let form = this.renderForm();

        return (
            <div>
                <h1 id="tabelLabel" >Check UNP</h1>
                {contents}
                {form}
            </div>
        );
    }

    async fetchUnp() {
        const response = await fetch('api/unp/getData?unp=123456789&type=json');
        console.log(response);

        const data = await response.json();

        console.log("Obj: " + JSON.stringify(data));

        this.setState({ unpData: data, loading: false });
    }

    async checkIfUnpExist(unpNum) {
        const response = await fetch('api/unp/checkUnp?unp=' + unpNum);
        const data = await response.json();

        console.log(JSON.stringify(data));
        this.setState({ unpExist: data.unpExist });

        return data;
    }
}

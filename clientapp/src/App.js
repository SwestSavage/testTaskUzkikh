import React from "react";
import './style.css'

export default class App extends React.Component {
    constructor(props) {
        super(props)
        this.state = {
            formValues: [{ unp: "", exist: "" }],
            user: {email: ""}
        };
        this.handleSubmit = this.handleSubmit.bind(this)
    }

    handleChange(i, e) {
        let formValues = this.state.formValues;
        formValues[i][e.target.name] = e.target.value;
        formValues[i]["exist"] = this.checkIfUnpExist(i, e.target.value);
        this.setState({ formValues });
    }

    addFormFields() {
        this.setState(({
            formValues: [...this.state.formValues, { unp: "", exist: "" }]
        }))
    }

    removeFormFields(i) {
        let formValues = this.state.formValues;
        formValues.splice(i, 1);
        this.setState({ formValues });
    }

    handleEmailChange(value) {
        let formValues = this.state.formValues;

        this.setState({ formValues, user: { email: value } });
    }

    handleSubmit(event) {
        event.preventDefault();

        let unps = this.state.formValues.map(item => item.unp);
        let email = this.state.user.email;

        //alert(JSON.stringify({ unps, email }));

        this.sendDataToServer({ unps, email });
    }

    render() {

        return (
            <form onSubmit={this.handleSubmit}>
                {this.state.formValues.map((element, index) => (
                    <div className="form-inline" key={index}>
                        <label>Unp</label>
                        <input type="number" name="unp" value={element.unp || ""} onChange={e => this.handleChange(index, e)} />
                        <label>Unp exist in db: {this.state.formValues[index]["exist"] === true ? "Yes" : "No"}</label>
                        {
                            index ?
                                <button type="button" className="button remove" onClick={() => this.removeFormFields(index)}>Remove</button>
                                : null
                        }
                    </div>
                ))}
                <label>Email</label>
                <input type="text" name="userEmail" value={this.state.user.email} onChange={e => this.handleEmailChange(e.target.value)} />
                <div className="button-section">
                    <button className="button add" type="button" onClick={() => this.addFormFields()}>Add</button>
                    <button className="button submit" type="submit">Submit</button>
                </div>
            </form>
        );
    }

    async checkIfUnpExist(i, unpNum) {
        const response = await fetch('api/unp/checkUnp?unp=' + unpNum);
        const data = await response.json();

        let formValues = this.state.formValues;
        formValues[i]["exist"] = data.unpExist;

        this.setState({ formValues });

        return data;
    }

    async sendDataToServer(data) {
        const options = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        };

        const response = await fetch('api/unp/postData', options);
        alert(response.status);
    }
}
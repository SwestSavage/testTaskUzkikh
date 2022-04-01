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

        this.sendDataToServer({ unps, email });
    }

    render() {

        return (               
            <form onSubmit={this.handleSubmit} className="container">
                <table>
                    <thead>
                        <tr>
                            <td>Введите УНП:</td>
                            <td>Есть в БД:</td>
                            <td>Нет в БД:</td>
                        </tr>
                    </thead>
                    <tbody>
                {this.state.formValues.map((element, index) => (
                    <tr  key={index}>
                        <td>
                            <input type="number" name="unp" value={element.unp || ""} onChange={e => this.handleChange(index, e)} />
                        </td>
                        <td>
                            {this.state.formValues[index]["exist"] === true ? <strong>+</strong> : null}
                        </td>
                        <td>
                            {this.state.formValues[index]["exist"] === false ? <strong>-</strong> : ""}
                        </td>
                        
                            {
                                index ?
                                <td><button type="button" className="button remove" onClick={() => this.removeFormFields(index)}>Убрать</button></td>
                                    : null
                            }
                        
                    </tr>
                ))}
                    </tbody>
                    </table>
                    <div className="form-inline">
                        <label>Email</label>
                        <input type="text" name="userEmail" className="form-inline" value={this.state.user.email} onChange={e => this.handleEmailChange(e.target.value)} />
                    </div>
                    <div className="button-section">
                        <button className="button add" type="button" onClick={() => this.addFormFields()}>Добавить поле для УНП</button>
                        <button className="button submit" type="submit">Подписаться на отправку Email со статусом УНП</button>
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
        console.log(response.status);
    }
}
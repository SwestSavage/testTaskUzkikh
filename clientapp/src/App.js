import React, { Component } from 'react';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = { unpData: {}, loading: true };
    }

    componentDidMount() {
        this.fetchUnp();
    }   

    static renderUnp(unp) {
        return (
            <p>{ unp.vnaimp }</p>
            )
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Загрузка...</em></p>
            : App.renderUnp(this.state.unpData);

        return (
            <div>
                <h1 id="tabelLabel" >Проверка УНП</h1>
                {contents}
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
}

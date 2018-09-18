class App extends React.Component {
    render() {
        return (
            <div>
                <h3>This Worked</h3>
                <button className="btn btn-default">increment</button>
            </div>
            );
    }
}


ReactDOM.render(
    <App />,
    document.getElementById('root')
);





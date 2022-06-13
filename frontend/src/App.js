import './App.css';
import React from 'react'
import logo from './img/logo.png';
import Login from './pages/Login';
import Main from './pages/Main';
import User from './pages/User';
import SignOut from './pages/SignOut';
import { Route, Routes, BrowserRouter} from 'react-router-dom'
import Api from './services/Api';
import Devices from './pages/Devices'
import {NotificationContainer} from 'react-notifications'
import DataTypes from './pages/DataTypes';
import DataView from './pages/DataView';

class App extends React.Component {
  constructor(props){
    super(props);
    this.state = {expandNavbar: false}
  }
  updateFromChild() {
    this.setState({expandNavbar: this.state.expandNavbar})
  }
  signOut(){
    Api.signOut();
    this.setState({expandNavbar: this.state.expandNavbar})
  }
  expandNavbar(){
    this.setState({expandNavbar: !this.state.expandNavbar})
  }
  render(){
    const logged = Api.loggedIn();
    let signPart = !logged ? <li className="sign-in" >
      <a href="/login" className="inline btn btn-outline-success my-2 my-sm-0" >Sign In</a>
    </li>
    : <li className="nav-item">
      <a className="nav-link" href="/user">User</a>
    </li>;
    let signOut = !logged ? "" : <button onClick={this.signOut.bind(this)} className="sign-out inline btn btn-outline-danger my-2 my-sm-0" >Sign Out</button>
    return (
      <div>
        <NotificationContainer/>
        <nav className="navbar navbar-expand-lg navbar-light container" id="mainNavbar">
          <a className="navbar-brand" href="/">
            <img src={logo} alt="err" className="logo"/>
            SysOT</a>
          <div className="justify-content-between" >
            <ul className={!this.state.expandNavbar ? "navbar-nav collapsed" : "navbar-nav"} id="mainNavbarLinks">
              <li className="nav-item">
                <a className={logged ? "nav-link" : "nav-link disabled"} href="/devices">Devices</a>
              </li>
              <li className="nav-item">
                <a className={logged ? "nav-link" : "nav-link disabled"} href="/data">Data</a>
              </li>
              <li className="nav-item">
                <a className={logged ? "nav-link" : "nav-link disabled"} href="/data-types">Data Types</a>
              </li>
              <li className="nav-item">
                <a className="nav-link disabled" href="/">Contact</a>
              </li>
              {signPart}
              <li>
                {signOut}
              </li>
            </ul>
            <button className="navbar-toggler" type="button" onClick={this.expandNavbar.bind(this)}
                data-bs-toggle="collapse" data-bs-target="#mainNavbarLinks"
                aria-controls="mainNavbarLinks" aria-expanded="false"
                aria-label="Toggle navigation">
                <span className="navbar-toggler-icon"></span>
            </button>
          </div>
        </nav>
        <section className="container" id='mainContent'>
        <BrowserRouter>
          <Routes>
            <Route path="/" element={<Main />} />
            <Route path="/data" element={<DataView />} />
            <Route path="/devices" element={<Devices />} />
            <Route path="/data-types" element={<DataTypes />} />
            <Route path="/user" element={<User/>} />
            <Route path="/login" element={<Login updateFromChild={this.updateFromChild.bind(this)} />} />
            <Route path="/signout" element={<SignOut updateFromChild={this.updateFromChild.bind(this)}/>} />
          </Routes>
        </BrowserRouter>
        </section>
        <footer id="mainFooter">
          <div className="container navbar navbar-dark">
            <a className="navbar-brand" href="/">
              <img src={logo} alt="err" className="logo"/>
              SysOT</a>
            <div>
              <ul className="navbar-nav">
                <li className="nav-item active">
                  <a className="nav-link" href="/">Docs</a>
                </li>
                <li className="nav-item">
                  <a className="nav-link disabled" href="/">Pricing</a>
                </li>
                <li className="nav-item">
                  <a className="nav-link disabled" href="/">About Us</a>
                </li>
                <li className="nav-item">
                  <a className="nav-link disabled" href="/">Concact</a>
                </li>
              </ul>
            </div>
          </div>
        </footer>
      </div>
    );
  }
  
}

export default App;

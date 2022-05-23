import './App.css';
import logo from './img/logo.png';
import Devices from './components/Devices';

function App() {
  return (
    <div>
     <nav className="navbar navbar-expand-lg navbar-light container" id="mainNavbar">
      <a className="navbar-brand" href="/">
        <img src={logo} alt="err" className="logo"/>
        SysOT</a>
      <div >
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
    </nav>
      <section className="container" id='mainContent'>
        <Devices />
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

export default App;

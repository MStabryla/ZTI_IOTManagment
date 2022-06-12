import React from 'react'
import './Login.css'
import Api from '../services/Api';
import { Navigate } from 'react-router-dom';
import 'react-notifications/lib/notifications.css';
import {NotificationManager} from 'react-notifications';

class Login extends React.Component {
   constructor (props){
      super(props);
      this.state = {
         Email: '',
         Password: ''
      }
      this.loggedIn = Api.loggedIn()
      this.props = props;
      console.log(this.loggedIn) 
   }
   change (event,parameter){
      const newState = this.state;
      newState[parameter] = event.target.value;
      this.setState(newState);
   }
   componentDidMount(){

   }
   async signIn(event){
      event.preventDefault();
      let res;
      res = await Api.post("auth/login",this.state).catch((error) => {
         if(error.response.status === 401)
            NotificationManager.error("Błędny login lub hasło!","Nie zalogowano",5000);
         else if(error.response.status >= 500)
            NotificationManager.error("Serwer nie mógł wykonać żądania!","Błąd serwera",5000);
         else
            NotificationManager.error("Nie można połączyć się z serwerem!","Nieznany błąd",5000);
      });
      Api.setToken(res.data.token);
      NotificationManager.success("","Zalogowano",1000);
      this.loggedIn = Api.loggedIn()
      this.props.updateFromChild();
      this.setState({});
   }
   render() {
      if(this.loggedIn)
         return <Navigate to="/" />
      return (
         <div>
            <h2>Sign in</h2>
            <form onSubmit={this.signIn.bind(this)}>
               <div className="form-group">
                  <label htmlFor="email">Email address</label>
                  <input type="email" className="form-control" id="email" aria-describedby="emailHelp" placeholder="Enter email" value={this.state.Email} onChange={(e) => this.change(e,'Email')} />
               </div>
               <div className="form-group">
                  <label htmlFor="pass">Password</label>
                  <input type="password" className="form-control" id="pass" placeholder="Password" value={this.state.Password} onChange={(e) => this.change(e,'Password')}/>
               </div>
               <button type="submit" className="btn btn-primary">Log In</button>
            </form>
         </div>
      )
   }
}
export default Login;
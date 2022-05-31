import React from 'react'
import './Login.css'
import Api from '../services/Api';
import { Navigate } from 'react-router-dom';

class Login extends React.Component {
   constructor (props){
      super(props);
      this.state = {
         Email: '',
         Password: ''
      }
   }
   change (event,parameter){
      const newState = {};
      newState[parameter] = event.target.value;
      this.setState(newState);

   }
   async signIn(event){
      event.preventDefault();
      //console.log(this)
      const res = await Api.post("auth/login",this.state);
      if(res.status != 200)
         throw Error("User not logged");
      Api.setToken(res.data.token);
      return <Navigate to="/user" />
      //this.props.history.push("/user");
      
   }
   render() {
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
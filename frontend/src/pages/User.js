import React from 'react'
import Api from '../services/Api';
import { Navigate } from 'react-router-dom'

class User extends React.Component{
    componentDidMount(){
    }
    render() {
        this.logged = Api.loggedIn();
        console.log("render",this.logged)
        if(!this.logged)
            return <Navigate to="/login"/>
        return (
            <div className='user-data'>
                
            </div>
        )
    }
}

export default User
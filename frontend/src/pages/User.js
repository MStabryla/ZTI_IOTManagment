import React from 'react'
import Api from '../services/Api';
import { Navigate } from 'react-router-dom'

class User extends React.Component{
    componentDidMount(){
        this.token = Api.getToken();
    }
    render() {
        if(!this.token)
            return <Navigate to="/login"/>
        return (
            <div className='UserData'>
                UserData
            </div>
        )
    }
}

export default User
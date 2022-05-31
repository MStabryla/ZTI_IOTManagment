import React from 'react'
import { Navigate } from 'react-router-dom';
import Api from '../services/Api';

class User extends React.Component{
    constructor(props){
        super(props);
    }
    componentDidMount(){
        const token = Api.getToken();
        if(!token)
            return <Navigate to="/" />
    }
    render() {
        return (
            <div className='UserData'>
                UserData
            </div>
        )
    }
}

export default User
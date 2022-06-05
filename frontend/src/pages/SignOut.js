import React from 'react'
import Api from '../services/Api';
import { Navigate } from 'react-router-dom';

class User extends React.Component{
    render() {
        Api.signOut();
        this.props.updateFromChild();
        return <Navigate to="/"/>
    }
}

export default User
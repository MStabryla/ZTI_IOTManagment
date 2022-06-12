import axios from "axios";
import jwt from 'jwt-decode'

class Api {
    constructor(){
        this.base_url = "https://localhost:5001/"
        this.loggedInState = undefined;
    }
    getToken(){
        this.token = localStorage.getItem("JWT_TOKEN");
        return this.token;
    }
    loggedIn(){
        this.getToken();
        if(!this.token) return false;
        let decodedToken = jwt(this.token, {complete: true});
        let dateNow = new Date();
        let dateToken = new Date(Date.parse(decodedToken.expStr));
        if(dateToken < dateNow)
            return false;
        return true;
    }
    signOut(){
        localStorage.removeItem("JWT_TOKEN");
    }
    setToken(token){
        localStorage.setItem("JWT_TOKEN",token);
    }
    get(url) {
        return axios.get(this.base_url + url,{
            headers: {
                Authorization: `Bearer ${this.token}`
            }
        })
    }
    post(url,data){
        return axios.post(this.base_url + url,data,{
            headers: {
                Authorization: `Bearer ${this.token}`
            }
        })
    }
    put(url,data){
        return axios.put(this.base_url + url,data,{
            headers: {
                Authorization: `Bearer ${this.token}`
            }
        })
    }
    delete(url){
        return axios.delete(this.base_url + url,{
            headers: {
                Authorization: `Bearer ${this.token}`
            }
        })
    }
}

export default new Api();
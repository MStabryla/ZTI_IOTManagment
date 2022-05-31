import axios from "axios";

class Api {
    constructor(){
        this.base_url = "https://localhost:5001/"
    }
    getToken(){
        this.token = localStorage.getItem("JWT_TOKEN");
    }
    loggedIn(){
        this.getToken();
        if(this.token)
            return true;
        else
            return false;
    }
    setToken(token){
        localStorage.setItem("JWT_TOKEN",token);
    }
    get(url){
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
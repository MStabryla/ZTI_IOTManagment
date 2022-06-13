import './Devices.css'
import Device from '../components/Device'
import Api from '../services/Api';
import React from 'react'
import {NotificationManager} from 'react-notifications';

const defaultState =  {
    devices:[],
    formType:null,
    device:{
        description: "",
        deviceName: "",
        ipAddress: "",
        mobile: false,
        type: ""
    },
    scrollToForm:true,
}
class Devices extends React.Component {
    constructor(){
        super();
        this.state = {}
        Object.assign(this.state,defaultState);
        this.deviceFormDescriptor = [
            { fieldName:"Device Name", fieldValue:"deviceName", type:"text", description:"Enter device name"},
            { fieldName:"Device Ip Address", fieldValue:"ipAddress", type:"text", description:"Enter device IPv4 address"},
            { fieldName:"Device Type", fieldValue:"type", type:"text", description:"Enter type of device"},
            { fieldName:"Device Description", fieldValue:"description", type:"text", description:"Enter device description"},
            { fieldName:"Mobile", fieldValue:"mobile", type:"checkbox", fieldDivClass:"form-check",fieldInputClass:"form-check-input", description:"Is device mobile?"},
        ]
        this.formDescriptor = {
            add:"Insert New Device",
            edit:"Edit Device"
        }
        this.form = React.createRef();
    }
    refresh(){
        this.getDevices().then(devices => {let ds = {defaultState};ds.devices = devices;ds.formType = null;this.setState(ds)})
    }
    async getDevices(){
        return (await Api.get("devices")).data
    }
    componentDidMount(){
        this.refresh();
        if(this.state.scrollToForm && this.form.current){
            this.form.current.scrollIntoView();
            const newState = this.state; newState.scrollToForm = false;this.setState(newState);
        }
            
    }
    change (event,field,parameter){
        const newState = this.state;
        if(field.type === "checkbox")
            newState.device[parameter] = event.target.checked;
        else
            newState.device[parameter] = event.target.value;
        this.setState(newState);
    }
    setDeviceSwitch(formType,device){
        let newState = this.state;
        newState.formType = formType
        if(device)
            newState.device = Object.assign({},device);
        else
            newState.device = Object.assign({},defaultState.device)
        this.setState(newState)
    }
    setDevice(event){
        event.preventDefault();
        let method = "";
        switch(this.state.formType){
            case "add":
                method = "post"; break;
            case "edit":
                method = "put"; break;
            default:
                return;
        }
        if(method === "put"){
            Api.put("devices/" + this.state.device.id,this.state.device).then(() => {
                NotificationManager.success("","Device updated.",1000);
                this.refresh()
            }).catch((error) => {
                NotificationManager.error(error.response.data,"Unknown error.",5000);
             });
        } 
        else{
            Api.post("devices",this.state.device).then(() => {
                NotificationManager.success("","Device added.",1000);
                this.refresh()
            }).catch((error) => {
                NotificationManager.error(error.response.data,"Unknown error.",5000);
            });
        }
    }
    removeDevice(id){
        Api.delete("devices/" + id).then(() => {
            NotificationManager.success("","Device removed.",1000);
            this.refresh()
        }).catch((error) => {
            NotificationManager.error(error,"Unknown error.",5000);
        });
    }
    back(){
        this.setDeviceSwitch(null)
    }
    render() {
        return (
            <section>                
                <div className="device-actions">
                    <button type="button" className="btn btn-action" onClick={() => this.setDeviceSwitch("add")}><i className="bi bi-plus"></i></button>
                </div>
                { this.state.formType ?
                    <div className="device-form" ref={this.form}>
                        <form onSubmit={this.setDevice.bind(this)}>
                            <h4>{this.formDescriptor[this.state.formType]}</h4>  
                            { this.deviceFormDescriptor.map((field) => 
                                <div key={field.fieldName} className={field.fieldDivClass ? field.fieldDivClass : "form-group"}>
                                    <label title={field.description} htmlFor={field.fieldValue}>{field.fieldName}</label>
                                    <input type={field.type} className={field.fieldInputClass ? field.fieldInputClass : "form-control"} id={field.fieldValue} aria-describedby={field.fieldValue + "Help"} placeholder={field.description} value={this.state.device[field.fieldValue]} checked={this.state.device[field.fieldValue]} onChange={(e) => this.change(e,field,field.fieldValue)} />
                                </div>
                            )}
                            { (() => {switch(this.state.formType){
                                case "add":
                                    return <button type="submit" className="btn btn-primary">Add Device</button>
                                case "edit":
                                    return <button type="submit" className="btn btn-warning">Update Device</button>
                                default:
                                    return ""
                            }})() }
                            <button type="button" className="btn" onClick={this.back.bind(this)}>Back</button>
                        </form>
                    </div>
                    : ""
                }
                <div className="devices">
                    <h2>Devices</h2>
                    <div className='bg-light container'>
                        {this.state.devices ? this.state.devices.map((device, key) => <Device key={device.deviceName} editAction={(device) => this.setDeviceSwitch("edit",device)} removeAction={this.removeDevice.bind(this)} device={device} />) : ""}
                    </div>
                </div>
            </section>
            
        );
    }
}

export default Devices;
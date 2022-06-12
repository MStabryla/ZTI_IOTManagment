import './Devices.css'
import Device from '../components/Device'
import Api from '../services/Api';
import React from 'react'

class Devices extends React.Component {
    constructor(){
        super();
        this.state = {devices:[]}
    }
    async getDevices(){
        return (await Api.get("devices")).data
    }
    componentDidMount(){
        this.getDevices().then(devices => this.setState({devices:devices}))
        
    }
    render() {
        return (
            <div className="devices">
                <h2>Devices</h2>
                <div className='bg-light container'>
                    {this.state.devices ? this.state.devices.map((device, key) => <Device key={device.deviceName} device={device} />) : ""}
                </div>

            </div>
        );
    }
}

export default Devices;
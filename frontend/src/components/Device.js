import './Device.css'
import cpu from '../img/cpu.png'
import React from 'react'

class Device extends React.Component{
    remove(){
        this.props.removeAction(this.props.device.id)
    }
    render() { 
        return (
            <div className='device row'>
                <h5 className='device-id'>{this.props.device.deviceName}</h5>
                <div className="device-left-part col-2">
                    <span className="device-ip">{this.props.device.ipAddress}</span>
                    <img src={cpu} alt='err'/>
                </div>
                <div className="col-8">
                    <p className="device-desc">{this.props.device.description}</p>
                </div>
                <div className="device-right-part col-2">
                    <div className="device-actions">
                        <button className='btn device-action btn-warning'><i className="bi bi-pencil"></i></button>
                        <button className='btn device-action btn-danger' onClick={this.remove.bind(this)}><i className="bi bi-trash"></i></button>
                    </div>
                    
                    <span className="device-type">{this.props.device.type}</span>
                    {(() => {
                        if(this.props.device.mobile) 
                            return (<span className="device-mobile">mobile</span>)
                    })()}
                </div>
            </div>
        );
    }
}

export default Device;
import './Device.css'
import cpu from '../img/cpu.png'

function Device(props){
    return (
        <div className='device row'>
            
            <h5 className='device-id'>{props.device.deviceName}</h5>
            <div className="device-left-part col-2">
                <span className="device-ip">{props.device.ipAddress}</span>
                <img src={cpu} alt='err'/>
            </div>
            <div className="col-8">
                <p className="device-desc">{props.device.description}</p>
            </div>
            <div className="device-right-part col-2">
                <span className="device-type">{props.device.type}</span>
                {(() => {
                    if(props.device.mobile) 
                        return (<span className="device-mobile">mobile</span>)
                })()}
            </div>
        </div>
    );
}

export default Device;
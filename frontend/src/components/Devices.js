import './Devices.css'
import Device from './Device'

function Devices(){
    const device1 = {
        "deviceName": "CR01",
        "ipAddress": "192.168.105.90",
        "type": "atmospheric",
        "managers": ["b3BlbnNzaC1rZXktdjEAAAA1"],
        "mobile": false
    };
    const device2 = {
        "deviceName": "CR02",
        "ipAddress": "192.168.105.91",
        "type": "thermal",
        "managers": ["b3BlbnNzaC1rZXktdjEAAAA1", "b3BlbnNzaC1rZXktdjEAAAA2"],
        "mobile": false
    };
    const device3 = {
        "deviceName": "SR01",
        "ipAddress": "192.168.100.30",
        "type": "gas sensor",
        "managers": ["b3BlbnNzaC1rZXktdjEAAAA3"],
        "mobile": true
    };
    return (
        <div className="devices">
            <h2>Devices</h2>
            <div className='bg-light container'>
                <Device device={device1}/>
                <Device device={device2}/>
                <Device device={device3}/>
            </div>
            
        </div>
    );
}

export default Devices;
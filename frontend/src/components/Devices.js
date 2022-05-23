import './Devices.css'
import Device from './Device'

function Devices(){
    const device1 = {id:'1'};
    const device2 = {id:'2'};
    const device3 = {id:'3'};
    return (
        <div className="devices">
            <h2>Devices</h2>
            <div className='bg-light'>
                <Device device={device1}/>
                <Device device={device2}/>
                <Device device={device3}/>
            </div>
            
        </div>
    );
}

export default Devices;
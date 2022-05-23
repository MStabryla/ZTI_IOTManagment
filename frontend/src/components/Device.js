import './Device.css'

function Device(props){
    return (
        <div className='device'>
            <span className='device-id'>{props.device.id}</span>
        </div>
        
    );
}

export default Device;
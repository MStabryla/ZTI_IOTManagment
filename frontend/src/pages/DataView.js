import React from "react"
import './DataView.css'
import Api from '../services/Api';
import jwt from 'jwt-decode'
import { LineChart, Line, CartesianGrid, XAxis, YAxis} from 'recharts';

const defaultState = {
    devices:[],
    dataClusters:[],
    data:[],
};
class DataView extends React.Component {
    constructor(props){
        super(props)
        this.state = Object.assign({},defaultState);
    }
    async getDevices(){
        return (await Api.get("devices")).data
    }
    async getData(id){
        return (await Api.get("data/" + id)).data
    }
    async refresh(){
        this.getDevices().then(async devices => {
            let ds = Object.assign({},defaultState);
            ds.devices = devices;
            const token = jwt(Api.getToken())
            const dataClusters = await Promise.all(devices.map(async (device) => {
                if(token.role.includes("Admin") || device.managers.includes(token.nameid)){
                    const clusters = await this.getData(device.id);
                    return clusters
                }
                return []
            }))
            ds.dataClusters = dataClusters
            const cl = this.drawCharts(dataClusters);
            ds.data = cl.filter(( element ) => {
                return element !== undefined;
             });
            console.log(ds.data);
            this.setState(ds)
        })
    }
    drawCharts(dataClusters){
        return dataClusters.map((clusterArray) => {
            console.log("clusterArray",clusterArray)
            if(!clusterArray || clusterArray.length < 1) return undefined;
            let cl = clusterArray[0];
            cl.values = cl.values.map(x => {let t = new Date(x.time);x.time = t.toLocaleDateString() + " " + t.toLocaleTimeString(); return x})
            return cl
        })
    }
    componentDidMount(){
        this.refresh();
    }
    render(){
        return (
            <div className="data">
                {
                    this.state.data.map(c => {
                        return <div key={c.id} className="chart">
                            <h5>{c.device.deviceName + ": " + c.measurementType.typeName }</h5>
                            <LineChart  width={window.innerWidth * 0.8} height={200} data={c.values}>
                                <Line type="monotone" dataKey="value" stroke="#2fc0f0" />
                                <CartesianGrid stroke="#ccc" strokeDasharray="5 5" />
                                <XAxis dataKey="time" />
                                <YAxis />
                            </LineChart>
                        </div>
                            
                    })
                }
                
            </div>
        )
    }
}

export default DataView
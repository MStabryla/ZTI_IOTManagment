import './DataTypes.css'
import DataType from '../components/DataType'
import Api from '../services/Api';
import React from 'react'
import {NotificationManager} from 'react-notifications';

const defaultState =  {
    dataType:{
        numeric: false,
        typeName: "",
        variableType: ""
    },
    dataTypes:[],
    formType:null
}
class DataTypes extends React.Component {
    constructor(){
        super();
        this.state = Object.assign({},defaultState);
        this.dataTypeFormDescriptor = [
            { fieldName:"Data Type", fieldValue:"typeName", type:"text", description:"Enter data type name"},
            { fieldName:"Variable Type", fieldValue:"variableType", type:"text", description:"Enter variable type"},
            { fieldName:"Numeric", fieldValue:"numeric", type:"checkbox",fieldDivClass:"form-check",fieldInputClass:"form-check-input", description:"Is data type a number?"},
        ]
        this.formDescriptor = {
            add:"Insert New Data Type",
            edit:"Edit Data Type"
        }
        this.form = React.createRef();
    }
    refresh(){
        this.getDataTypes().then(dataTypes => {let ds = Object.assign({},defaultState);ds.dataTypes = dataTypes;ds.formType = null;this.setState(ds)})
    }
    async getDataTypes(){
        return (await Api.get("data/types")).data
    }
    componentDidMount(){
        this.refresh(); 
    }
    change (event,field,parameter){
        const newState = this.state;
        if(field.type === "checkbox")
            newState.dataType[parameter] = event.target.checked;
        else
            newState.dataType[parameter] = event.target.value;
        this.setState(newState);
    }
    setDataTypeSwitch(formType,dataType){
        let newState = this.state;
        if(dataType)
            newState.dataType = Object.assign({},dataType);
        else
            newState.dataType = Object.assign({},defaultState.dataType)
        
        newState.formType = formType;
        this.setState(newState)
    }
    setDataType(event){
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
            Api.put("data/types/" + this.state.dataType.id,this.state.dataType).then(() => {
                NotificationManager.success("","Data Type updated.",1000);
                this.refresh()
            }).catch((error) => {
                NotificationManager.error(error.response.data,"Unknown error.",5000);
             });
        } 
        else{
            Api.post("data/types/",this.state.dataType).then(() => {
                NotificationManager.success("","Data Type added.",1000);
                this.refresh()
            }).catch((error) => {
                NotificationManager.error(error.response.data,"Unknown error.",5000);
            });
        }
    }
    removeDataType(id){
        Api.delete("data/types/" + id).then(() => {
            NotificationManager.success("","Data Type removed.",1000);
            this.refresh()
        }).catch((error) => {
            NotificationManager.error(error,"Unknown error.",5000);
        });
    }
    back(){
        this.setDataTypeSwitch(null)
    }
    render() {
        return (
            <section>                               
                <div className="data-types">
                    <h2>Data Types</h2>
                    <div className='container'>
                        <table className="table">
                            <thead>
                                <tr>
                                    <th scope="col">Type Name</th>
                                    <th scope="col">Variable Type</th>
                                    <th scope="col">Is Numeric</th>
                                </tr>
                            </thead>
                            <tbody>
                                {this.state.dataTypes ? this.state.dataTypes.map((dataType, key) => <DataType key={dataType.id} editAction={(dataType) => this.setDataTypeSwitch("edit",dataType)} removeAction={this.removeDataType.bind(this)} dataType={dataType} />) : <tr></tr>}
                                { this.state.formType ?
                                    <tr className="data-type-form" ref={this.form}>
                                        { this.dataTypeFormDescriptor.map((field) => 
                                            <td key={field.fieldName} className={field.fieldDivClass ? field.fieldDivClass : "form-group"}>
                                                <input type={field.type} className={field.fieldInputClass ? field.fieldInputClass : "form-control"} id={field.fieldValue} aria-describedby={field.fieldValue + "Help"} placeholder={field.description} value={this.state.dataType[field.fieldValue]} checked={this.state.dataType[field.fieldValue]} onChange={(e) => this.change(e,field,field.fieldValue)} />
                                            </td>
                                        )}
                                    </tr>
                                    : <tr></tr>
                                }
                            </tbody>
                        </table>
                        <div className="data-type-actions">
                            { this.state.formType ?
                                <div>
                                    { (() => {switch(this.state.formType){
                                        case "add":
                                            return <button type="button" className="btn btn-primary" onClick={this.setDataType.bind(this)}>Add Data Type</button>
                                        case "edit":
                                            return <button type="button" className="btn btn-warning" onClick={this.setDataType.bind(this)}>Update Data Type</button>
                                        default:
                                            return ""
                                    }})() }
                                    <button type="button" className="btn" onClick={this.back.bind(this)}>Back</button>
                                </div>
                                : <button type="button" className="btn btn-action" onClick={() => this.setDataTypeSwitch("add")}><i className="bi bi-plus"></i></button>
                            }
                        </div>
                    </div>
                </div>
            </section>
            
        );
    }
}

export default DataTypes;
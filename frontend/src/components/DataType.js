import React from "react";
import './DataType.css'

class DataType extends React.Component{
    remove(){
        this.props.removeAction(this.props.dataType.id)
    }
    render(){
        return (
        <tr className="data-type">
            <td >{this.props.dataType.typeName}</td>
            <td >{this.props.dataType.variableType}</td>
            <td >{this.props.dataType.numeric ? "true" : "false"}</td>
            <td>
                <button className='btn data-type-action btn-danger' onClick={this.remove.bind(this)}><i className="bi bi-trash"></i></button>
            </td>
        </tr>
        )
    }
}

export default DataType
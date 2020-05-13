import React from 'react'
import OneTaskComponent from '../../../components/User/OneTaskComponent/OneTaskComponent'
import { connect } from 'react-redux'
import { onSendCreate, fetchTaskCreate, changeChecked } from '../../../store/actions/teacher'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import AfterCreate from '../../../components/AfterCreate/AfterCreate'

class CreateTask extends React.Component {
    componentDidMount() {
        this.props.fetchTaskCreate()
    }

    render() {
        return (
            <Auxiliary>
                {this.props.successId !== null ?
                    <AfterCreate id={this.props.successId}/> : 
                    null
                }
            
                <OneTaskComponent
                    typeTask='create'
                    types={this.props.createData.types}
                    subjects={this.props.createData.subjects} 
                    groups={this.props.createData.groups}
                    taskAdditionData={this.props.taskAdditionData}
                    onSendCreate={this.props.onSendCreate}
                    changeChecked={this.props.changeChecked}
                />
            </Auxiliary>
        )
    }
}

function mapStateToProps(state) {
    return {
        createData: state.teacher.createData,
        successId: state.teacher.successId,
        taskAdditionData: state.teacher.taskAdditionData
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchTaskCreate: () => dispatch(fetchTaskCreate()),
        onSendCreate: (task, path) => dispatch(onSendCreate(task, path)),
        changeChecked: (studentIndex, groupIndex, val) => dispatch(changeChecked(studentIndex, groupIndex, val))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(CreateTask)
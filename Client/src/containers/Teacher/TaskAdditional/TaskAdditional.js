import React from 'react'
import OneTaskComponent from '../../../components/User/OneTaskComponent/OneTaskComponent'
import Error from '../../../components/Error/Error'
import { connect } from 'react-redux'
import {fetchTaskById, errorWindow, onCloseTask} from '../../../store/actions/teacher'

class TasksComponent extends React.Component { 
  
    componentDidMount() {
        this.props.fetchTaskById(this.props.match.params.id)
    }

    render() {
        return (
            <OneTaskComponent
                typeTask='task'
                taskAdditionData={this.props.taskAdditionData}
                loading={this.props.loading}
                onCloseTask={() => this.props.onCloseTask(this.props.match.params.id)}
            >
                {this.props.errorShow ? 
                    <Error 
                        errorMessage={this.props.errorMessage}
                        errorWindow={() => this.props.errorWindow(false, [])}
                    /> : 
                    null
                }
            </OneTaskComponent>
        )
    }
}

function mapStateToProps(state) {
    return {
        taskAdditionData: state.teacher.taskAdditionData,
        errorShow: state.teacher.errorShow,
        errorMessage: state.teacher.errorMessage,
        loading: state.teacher.loading
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchTaskById: id => dispatch(fetchTaskById(id)),
        errorWindow: (errorShow, errorMessage) => dispatch(errorWindow(errorShow, errorMessage)),
        onCloseTask: (id) => dispatch(onCloseTask(id))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(TasksComponent)
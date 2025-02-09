import React from 'react'
import OneTaskComponent from '../../../components/User/OneTaskComponent/OneTaskComponent'
import { connect } from 'react-redux'
import { fetchTaskById, errorWindow, onSendSolution } from '../../../store/actions/student'
import Error from '../../../components/Error/Error'

class TasksComponent extends React.Component {    
    componentDidMount() {
        this.props.fetchTaskById(this.props.match.params.id)
    }

    render() {
        return (
            <OneTaskComponent
                typeTask='task'
                idTask={this.props.match.params.id}
                taskAdditionData={this.props.taskAdditionData}
                descriptionTextarea={Object.keys(this.props.taskAdditionData).length !== 0 && this.props.taskAdditionData.solution !== null ? this.props.taskAdditionData.solution.contentText : ''}
                loading={this.props.loading}
                onSendSolution={this.props.onSendSolution}
                role={this.props.role}
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
        taskAdditionData: state.student.taskAdditionData,
        errorShow: state.student.errorShow,
        errorMessage: state.student.errorMessage,
        loading: state.student.loading,
        role: state.auth.role
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchTaskById: id => dispatch(fetchTaskById(id)),
        onSendSolution: (createSolution, id, path) => dispatch(onSendSolution(createSolution, id, path)),
        errorWindow: (errorShow, errorMessage) => dispatch(errorWindow(errorShow, errorMessage)),
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(TasksComponent)
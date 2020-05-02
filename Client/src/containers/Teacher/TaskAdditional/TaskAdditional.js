import React from 'react'
import OneTaskComponent from '../../../components/User/OneTaskComponent/OneTaskComponent'
import Error from '../../../components/Error/Error'
import { connect } from 'react-redux'
import {fetchTaskById, errorWindow} from '../../../store/actions/teacher'

class TasksComponent extends React.Component { 
  
    componentDidMount() {
        this.props.fetchTaskById(this.props.match.params.id)
    }

    render() {
        return (
            <OneTaskComponent
                taskAdditionData={this.props.taskAdditionData}
            >
                {this.props.errorShow ? 
                    <Error 
                        errorMessage={this.props.errorMessage}
                        errorWindow={() => this.props.errorWindow(false, [])}
                    /> : 
                    null
                }
                <div>Оппаааа</div>
            </OneTaskComponent>
        )
    }
}

function mapStateToProps(state) {
    return {
        taskAdditionData: state.teacher.taskAdditionData,
        errorShow: state.teacher.errorShow,
        errorMessage: state.teacher.errorMessage
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchTaskById: id => dispatch(fetchTaskById(id)),
        errorWindow: (errorShow, errorMessage) => dispatch(errorWindow(errorShow, errorMessage))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(TasksComponent)
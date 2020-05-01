import React from 'react'
import TasksComponent from '../../../components/User/TasksComponent/TasksComponent'
import { connect } from 'react-redux'
import { fetchTaskFilters, choiceSubjectTask, fetchListTasks } from '../../../store/actions/student'

class Tasks extends React.Component {
    componentDidMount() {
        this.props.fetchTaskFilters()
    }
    
    render() {
        return (
            <TasksComponent 
                subjects={this.props.taskData.subjects}
                types={this.props.taskData.types}
                labs={this.props.labs}
                choiceSubjectTask={this.props.choiceSubjectTask}
                fetchListTasks={this.props.fetchListTasks}
            />
        )
    }
}

function mapStateToProps(state) {
    return {
        taskData: state.student.taskData,
        labs: state.student.labs
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchTaskFilters: () => dispatch(fetchTaskFilters()),
        choiceSubjectTask: (indexSubject) => dispatch(choiceSubjectTask(indexSubject)),
        fetchListTasks: (filters) => dispatch(fetchListTasks(filters))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Tasks)
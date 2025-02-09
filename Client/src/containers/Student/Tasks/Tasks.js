import React from 'react'
import TasksComponent from '../../../components/User/TasksComponent/TasksComponent'
import { connect } from 'react-redux'
import { fetchTaskFilters, choiceSubjectTask, fetchListTasks } from '../../../store/actions/student'

class Tasks extends React.Component {  
    render() {
        return (
            <TasksComponent 
                subjects={this.props.taskData.subjects}
                types={this.props.taskData.types}
                tasks={this.props.tasks}
                loading={this.props.loading}
                choiceSubjectTask={this.props.choiceSubjectTask}
                fetchTaskFilters={this.props.fetchTaskFilters}
                fetchListTasks={this.props.fetchListTasks}
                role={this.props.role}
            />
        )
    }
}

function mapStateToProps(state) {
    return {
        taskData: state.student.taskData,
        tasks: state.student.tasks,
        loading: state.student.loading,
        role: state.auth.role
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchTaskFilters: () => dispatch(fetchTaskFilters()),
        choiceSubjectTask: (filters) => dispatch(choiceSubjectTask(filters)),
        fetchListTasks: (filters) => dispatch(fetchListTasks(filters)),
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Tasks)
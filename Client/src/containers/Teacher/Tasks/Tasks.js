import React from 'react'
import TasksComponent from '../../../components/User/TasksComponent/TasksComponent'
import { fetchTaskFilters, choiceSubjectTask, choiceGroupTask, fetchListTasks } from '../../../store/actions/teacher'
import { connect } from 'react-redux'

class Tasks extends React.Component {  
    render() {
        return (
            <TasksComponent 
                subjects={this.props.taskData.subjects}
                types={this.props.taskData.types}
                tasks={this.props.tasks}
                loading={this.props.loading}
                choiceSubjectTask={this.props.choiceSubjectTask}
                choiceGroupTask={this.props.choiceGroupTask}
                fetchTaskFilters={this.props.fetchTaskFilters}
                fetchListTasks={this.props.fetchListTasks}
            />
        )
    }
}

function mapStateToProps(state) {
    return {
        taskData: state.teacher.taskData,
        tasks: state.teacher.tasks,
        loading: state.teacher.loading
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchTaskFilters: () => dispatch(fetchTaskFilters()),
        choiceSubjectTask: (indexSubject) => dispatch(choiceSubjectTask(indexSubject)),
        choiceGroupTask: (indexSubject, indexGroup) => dispatch(choiceGroupTask(indexSubject, indexGroup)),
        fetchListTasks: (filters) => dispatch(fetchListTasks(filters))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Tasks)
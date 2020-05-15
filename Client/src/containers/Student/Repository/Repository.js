import React from 'react'
import RepositoryComponent from '../../../components/User/RepositoryComponent/RepositoryComponent'
import { connect } from 'react-redux'
import {fetchRepository, choiceSubjectHandler, choiceRepoHandler} from '../../../store/actions/student'

class Repository extends React.Component {  
    componentDidMount() {
        this.props.fetchRepository()
    }

    render() {
        return (
            <RepositoryComponent 
                repositoryData={this.props.repositoryData}
                subjectFullData={this.props.subjectFullData}
                loading={this.props.loading}
                choiceSubject={this.props.choiceSubjectHandler}
                choiceRepo={this.props.choiceRepoHandler}
            />
        )
    }
}

function mapStateToProps(state) {
    return {
        repositoryData: state.student.repositoryData,
        subjectFullData: state.student.subjectFullData,
        loading: state.teacher.loading
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchRepository: () => dispatch(fetchRepository()),
        choiceSubjectHandler: (index) => dispatch(choiceSubjectHandler(index)),
        choiceRepoHandler: (index) => dispatch(choiceRepoHandler(index)),
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Repository)
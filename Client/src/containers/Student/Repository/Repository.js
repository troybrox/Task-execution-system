import React from 'react'
import RepositoryComponent from '../../../components/User/RepositoryComponent/RepositoryComponent'
import { connect } from 'react-redux'
import {fetchRepository, choiceSubjectHandler} from '../../../store/actions/student'

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
        choiceSubjectHandler: (index) => dispatch(choiceSubjectHandler(index))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Repository)
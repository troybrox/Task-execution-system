import React from 'react'
import RepositoryComponent from '../../../components/User/RepositoryComponent/RepositoryComponent'
import { connect } from 'react-redux'
import {fetchRepository, choiceSubjectHandler, deleteRepo, editRepo, sendCreateRepositoryFile, choiceRepoHandler} from '../../../store/actions/teacher'

class Repository extends React.Component {
    componentDidMount() {
        this.props.fetchRepository()
    }
    
    render() {
        return (
            <RepositoryComponent 
                repositoryData={this.props.repositoryData}
                loading={this.props.loading}
                subjectFullData={this.props.subjectFullData}
                choiceSubject={this.props.choiceSubjectHandler}
                choiceRepo={this.props.choiceRepoHandler}
                deleteRepo={this.props.deleteRepo}
                editRepo={this.props.editRepo}
                sendCreateRepositoryFile={this.props.sendCreateRepositoryFile}
            />
        )
    }
}

function mapStateToProps(state) {
    return {
        repositoryData: state.teacher.repositoryData,
        subjectFullData: state.teacher.subjectFullData,
        loading: state.teacher.loading
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchRepository: () => dispatch(fetchRepository()),
        choiceSubjectHandler: (index) => dispatch(choiceSubjectHandler(index)),
        choiceRepoHandler: (index) => dispatch(choiceRepoHandler(index)),
        deleteRepo: (index) => dispatch(deleteRepo(index)),
        editRepo: (index, contentText, name) => dispatch(editRepo(index, contentText, name)),
        sendCreateRepositoryFile: (filters) => dispatch(sendCreateRepositoryFile(filters))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Repository)
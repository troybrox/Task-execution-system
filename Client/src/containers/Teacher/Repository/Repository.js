import React from 'react'
import RepositoryComponent from '../../../components/User/RepositoryComponent/RepositoryComponent'
import { connect } from 'react-redux'
import {fetchRepository, choiceSubjectHandler} from '../../../store/actions/teacher'

class Repository extends React.Component {
    state = { 
        topicText: {
            'Множества': 'Под множеством понимают совокупность определенных и отличных друг от друга объектов (элементов), объединенных общим характерным признаком в единое целое. Множество, не содержащее ни одного элемента, называют пустым и обозначают символом O. В математике вместо термина «множество» часто говорят «класс», «семейство», «совокупность». Множество считается определенным, если указаны все его элементы или правило их нахождения (характерное свойство элементов).',
            'Отображения': 'Соответствие, при котором каждому из элементов множества X сопоставялется единственный элемент из множества Y, называется отображением.',
            'Вещественные числа': 'Вещественные или действительные числа — это вместе взятые множества рациональных и иррациональных чисел.',
            'Предел последовательности': ''
        }
    }

    // changeRepositoryHanlder = (target, key) => {
    //     const topicText = [...this.state.topicText]
    //     topicText[key] = target
    //     this.setState({
    //         topicText
    //     })
    // }

    componentDidMount() {
        this.props.fetchRepository()
    }
    
    render() {
        return (
            <RepositoryComponent 
                // topicText={this.state.topicText}
                repositoryData={this.props.repositoryData}
                loading={this.props.loading}
                subjectFullData={this.props.subjectFullData}
                choiceSubject={this.props.choiceSubjectHandler}
                // changeRepositoryHandler={this.changeRepositoryHanlder}
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
        choiceSubjectHandler: (index) => dispatch(choiceSubjectHandler(index))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Repository)
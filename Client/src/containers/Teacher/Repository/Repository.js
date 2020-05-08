import React from 'react'
import RepositoryComponent from '../../../components/User/RepositoryComponent/RepositoryComponent'

class Repository extends React.Component {
    state = { 
        subjects: [
            {value: 'Математический анализ', topics: ['Множества', 'Отображения', 'Вещественные числа', 'Предел последовательности'], open: false},
            {value: 'UML', topics: [], open: false},
            {value: 'Моделирование сложных систем', topics: [], open: false},
            {value: 'ЭВМ', topics: [], open: false}
        ],
        topicText: {
            'Множества': 'Под множеством понимают совокупность определенных и отличных друг от друга объектов (элементов), объединенных общим характерным признаком в единое целое. Множество, не содержащее ни одного элемента, называют пустым и обозначают символом O. В математике вместо термина «множество» часто говорят «класс», «семейство», «совокупность». Множество считается определенным, если указаны все его элементы или правило их нахождения (характерное свойство элементов).',
            'Отображения': 'Соответствие, при котором каждому из элементов множества X сопоставялется единственный элемент из множества Y, называется отображением.',
            'Вещественные числа': 'Вещественные или действительные числа — это вместе взятые множества рациональных и иррациональных чисел.',
            'Предел последовательности': ''
        }
    }

    choiceSubjectHandler = index => {
        const subjects = [...this.state.subjects]
        subjects[index].open = !subjects[index].open

        this.setState({
            subjects
        })
    }

    changeRepositoryHanlder = (target, key) => {
        const topicText = [...this.state.topicText]
        topicText[key] = target
        this.setState({
            topicText
        })
    }
    
    render() {
        return (
            <RepositoryComponent 
                subjects={this.state.subjects}
                topicText={this.state.topicText}
                choiceSubject={this.choiceSubjectHandler}
                changeRepositoryHandler={this.changeRepositoryHanlder}
            />
        )
    }
}

export default Repository
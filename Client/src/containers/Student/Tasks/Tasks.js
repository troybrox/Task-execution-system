import React from 'react'
import TasksComponent from '../../../components/User/TasksComponent/TasksComponent'

class Tasks extends React.Component {
    state = {
        subjects: [
            {value: 'Моделирование сложных систем', open: false},
            {value: 'ЭВМ', open: false}
        ],
        labs: [
            {name: 'Лабораторная работа №1',  countComments: 3, lastOpen: ['2 недели', 'Студент1']},
            {name: 'Лабораторная работа №2',  countComments: 2, lastOpen: ['1 месяц', 'Студент2']},
            {name: 'Лабораторная работа №3',  countComments: 10, lastOpen: ['3 дня', 'Студент3 Студент4']},
        ]
    }
    choiceSubjectHandler = index => {
        const subjects = [...this.state.subjects]
        subjects.forEach(el => {
            el.open = false
        })

        subjects[index].open = !subjects[index].open

        this.setState({
            subjects
        })
    }
    
    render() {
        return (
            <TasksComponent 
                subjects={this.state.subjects}
                labs={this.state.labs}
                choiceSubjectHandler={this.choiceSubjectHandler}
            />
        )
    }
}

export default Tasks
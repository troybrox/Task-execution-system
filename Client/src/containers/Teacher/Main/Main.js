import React from 'react'
import './Main.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'

class Main extends React.Component {
    state = {
        subjects: [
            {value: 'Моделирование сложных систем', groups: ['6001-020304D', '6002-020304D'], open: false},
            {value: 'ЭВМ', groups: ['6005-020304D', '6004-020304D'], open: false}
        ],
        students: [
            {
                name: 'Студент 1', 
                open: false, 
                labs: [
                    {name: 'Лабораторная работа №1', begin: '10.10.2020', end: '10.11.2020'},
                    {name: 'Лабораторная работа №2', begin: '18.10.2020', end: ''}
                ]
            },
            {
                name: 'Студент 2', 
                open: false, 
                labs: [
                    {name: 'Лабораторная работа №1', begin: '10.10.2020', end: ''},
                    {name: 'Лабораторная работа №2', begin: '18.10.2020', end: '10.11.2020'}
                ]
            },
            {
                name: 'Студент 3', 
                open: false, 
                labs: [
                    {name: 'Лабораторная работа №1', begin: '10.10.2020', end: ''},
                    {name: 'Лабораторная работа №2', begin: '18.10.2020', end: ''}
                ]
            },
            {
                name: 'Студент 4', 
                open: false, 
                labs: [
                    {name: 'Лабораторная работа №1', begin: '10.10.2020', end: '10.11.2020'},
                    {name: 'Лабораторная работа №2', begin: '18.10.2020', end: '10.11.2020'}
                ]
            }
        ],
        title: ''
    }

    choiceGroup = (value, item) => {
        const title = value + '. Группа ' + item
        this.setState({title})
    }

    choiceSubject = index => {
        const subjects = [...this.state.subjects]
        subjects[index].open = !subjects[index].open

        this.setState({
            subjects
        })
    }

    choiceStudent = index => {
        const students = [...this.state.students]
        students[index].open = !students[index].open 

        this.setState({
            students
        })

    }

    renderMiniList(groups, value) {
        return groups.map((item, index) => {
            return (
                <li 
                    key={index}
                    className='small_items'
                    onClick={this.choiceGroup.bind(this, value, item)}
                >
                    <img src='images/folder-regular.svg' alt='' />
                    {item}
                </li>
            )
        })
    }
    
    renderList() {
        const list = this.state.subjects.map((item, index) => {
            const cls = ['big_items']
            let src = 'images/angle-right-solid.svg'
            if (item.open) {
                cls.push('active_big')
                src = 'images/angle-down-solid.svg'

            }
            return (
                <Auxiliary key={index}>
                    <li 
                        className={cls.join(' ')}
                        onClick={this.choiceSubject.bind(this, index)}
                    >
                        {<img src={src} alt='' />}
                        {item.value}
                    </li>

                    {item.open ? 
                        <ul className='small_list'>
                            {this.renderMiniList(item.groups, item.value)}
                        </ul> : null
                    }
                </Auxiliary>
            )
        })

        return (
            <ul className='big_list'>{list}</ul>
        )
    }

    renderLabs = labs => {
        return labs.map((item, index) => {
            let src = 'images/times-solid.svg'
            if (item.end) src = 'images/check-square-regular.svg'
            return (
                <div key={index} className='labs'>
                    <span className='title'>{item.name}</span>
                    <span className='time'>
                        <span>Открыта {item.begin}</span><br />
                        {item.end ? <span>Закрыта {item.end}</span> : null}
                    </span>
                    <img src={src} alt='' />
                </div>
            )
        })
    }

    renderStudents() {
        return this.state.students.map((item, index) => {
            const cls = ['each_student']
            if (item.open) cls.push('active_student')
            return (
                <Auxiliary key={index}>
                    <div 
                        className={cls.join(' ')} 
                        onClick={this.choiceStudent.bind(this, index)}
                    >
                        <img src='images/card.svg' alt='' />
                        <p>{item.name}</p>
                    </div>
                    {item.open ? this.renderLabs(item.labs) : null}
                </Auxiliary>
            )
        })
    }

    render() {
        return (
            <Frame active_index={1}>
                <div className='value_subject'>{this.state.title}</div>
                <div className='main_subject'>
                    <aside className='aside_subject'>
                        {this.renderList()}
                    </aside>
                    <div className='persons_group'>
                        { this.state.title ? this.renderStudents() : null}
                    </div>
                </div>
            </Frame>
        )
    }
}

export default Main
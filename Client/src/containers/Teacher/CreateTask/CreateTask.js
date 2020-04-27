import React from 'react'
import './CreateTask.scss'
import OneTaskComponent from '../../../components/User/OneTaskComponent/OneTaskComponent'
// import Label from '../../../components/UI/Label/Label'
// import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
// import Input from '../../../components/UI/Input/Input'

class CreateTask extends React.Component {
    state = {
        type: [
            {
                id: null, 
                name: 'Выберите тип',
            },
            {
                id: 1,
                name: 'Лабораторная работа'
            },
            {
                id: 2,
                name: 'Курсовая работа'
            },
            {
                id: 3,
                name: 'Самостоятельная работа'
            }
        ],
        subjects: [
            {
                id: null, 
                name: 'Выберите предмет',
            },
            {
                id: 1, 
                name: 'Математический анализ',
                groups: [
                    {
                        id: null,
                        name: 'Все'
                    },
                    { 
                        id: 1, 
                        name: '6114-020304D',
                        students: [
                            {id: 1, name: 'Студент 1', check: false},
                            {id: 2, name: 'Студент 2', check: false},
                            {id: 3, name: 'Студент 3', check: false},
                            {id: 4, name: 'Студент 4', check: false},
                            {id: 5, name: 'Студент 5', check: false},
                            {id: 6, name: 'Студент 6', check: false}
                        ]
                    },
                    {
                        id: 2, 
                        name: '2234-030303Z',
                        students: [
                            {id: 4, name: 'Студент 4', check: false},
                            {id: 5, name: 'Студент 5', check: false},
                            {id: 6, name: 'Студент 6', check: false}
                        ]
                    },
                    {
                        id: 3, 
                        name: '4523-050306A',
                        students: [
                            {id: 7, name: 'Студент 7', check: false},
                            {id: 8, name: 'Студент 8', check: false},
                            {id: 9, name: 'Студент 9', check: false}
                        ]
                    }
                ]
            },
            {
                id: 2, 
                name: 'Другой предмет',
                groups: [
                    {
                        id: null,
                        name: 'Все'
                    },
                    { 
                        id: 1, 
                        name: '5555-020304D',
                        students: [
                            {id: 1, name: 'Студент 1', check: false},
                            {id: 2, name: 'Студент 2', check: false},
                            {id: 3, name: 'Студент 3', check: false}
                        ]
                    },
                    {
                        id: 2, 
                        name: '2234-030303Z',
                        students: [
                            {id: 4, name: 'Студент 4', check: false},
                            {id: 5, name: 'Студент 5', check: false},
                            {id: 6, name: 'Студент 6', check: false}
                        ]
                    },
                    {
                        id: 3, 
                        name: '4523-050306A',
                        students: [
                            {id: 7, name: 'Студент 7', check: false},
                            {id: 8, name: 'Студент 8', check: false},
                            {id: 9, name: 'Студент 9', check: false}
                        ]
                    }
                ]
            }
        ]
    }

    // onCheckStudents = () => {

    // }

    render() {
        return (
            <OneTaskComponent
                typeTask='create'
                type={this.state.type}
                subjects={this.state.subjects} 
            >

            </OneTaskComponent>
        )
    }

}

export default CreateTask
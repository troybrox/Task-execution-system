import React from 'react'
import OneTaskComponent from '../../../components/User/OneTaskComponent/OneTaskComponent'
import { connect } from 'react-redux'
import { onSendCreate, fetchTaskCreate, changeChecked } from '../../../store/actions/teacher'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import AfterCreate from '../../../components/AfterCreate/AfterCreate'

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

    componentDidMount() {
        this.props.fetchTaskCreate()
    }

    render() {
        return (
            <Auxiliary>
                {this.props.successId !== null ?
                    <AfterCreate id={this.props.successId}/> : 
                    null
                }
            
                <OneTaskComponent
                    typeTask='create'
                    types={this.props.createData.types}
                    subjects={this.props.createData.subjects} 
                    groups={this.props.createData.groups}
                    onSendCreate={this.props.onSendCreate}
                    changeChecked={this.props.changeChecked}
                />
            </Auxiliary>
        )
    }
}

function mapStateToProps(state) {
    return {
        createData: state.teacher.createData,
        successId: state.teacher.successId
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchTaskCreate: () => dispatch(fetchTaskCreate()),
        onSendCreate: (task) => dispatch(onSendCreate(task)),
        changeChecked: (studentIndex, groupIndex, val) => dispatch(changeChecked(studentIndex, groupIndex, val))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(CreateTask)
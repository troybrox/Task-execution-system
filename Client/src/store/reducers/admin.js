import { PUSH_USERS, PUSH_SELECTS, ERROR_WINDOW } from "../actions/actionTypes"

const initialState = {
    users: [
        {id: 0, name: 'Преподаватель 1', position: 'Должность', check: false},
        {id: 1, name: 'Преподаватель 2', position: 'Должность', check: false},
        {id: 2, name: 'Преподаватель 3', position: 'Должность', check: false},
        {id: 3, name: 'Преподаватель 4', position: 'Должность', check: false}
    ],
    selects: [
        {
            title: 'Факультет', 
            options: [{id: null, name: 'Все'}, {id: 0, name: 'Информатики'}, {id: 1, name: 'Политологии'}], 
            show: true
        },
        {
            title: 'Кафедра',  
            options: [{id: null, name: 'Все'}, {id: 0, name: 'Институт ракетно-космической техники', facultyId: 0}, {id: 1, name: 'Институт двигателей и энергетических установок', facultyId: 0}, {id: 2, name: 'Что-то о политике', facultyId: 1}], 
            show: true
        },
        {
            title: 'Группа',  
            options: [{id: null, name: 'Все'}, {id: 0, name: '6213-010201D', facultyId: 0}, {id: 1, name: '2402-020502A', facultyId: 0}, {id: 2, name: '2121-090909Z', facultyId: 1}], 
            show: false
        }
    ],
    errorShow: false,
    errorMessage: []
}

export default function adminReducer(state = initialState, action) {
    switch (action.type) {
        case PUSH_USERS:
            return {
                ...state, users: action.users
            }
        case PUSH_SELECTS:
            return {
                ...state, selects: action.selects
            }
        case ERROR_WINDOW:
            return {
                ...state, errorShow: action.errorShow, errorMessage: action.errorMessage
            }
        default:
            return state
    }
}
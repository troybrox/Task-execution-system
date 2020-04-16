import { PUSH_USERS, PUSH_SELECTS } from "../actions/actionTypes"

const initialState = {
    users: [
        {name: 'Преподаватель 1', position: 'Должность', check: false, show: true},
        {name: 'Преподаватель 2', position: 'Должность', check: false, show: true},
        {name: 'Преподаватель 3', position: 'Должность', check: true, show: true},
        {name: 'Преподаватель 4', position: 'Должность', check: false, show: true}
    ],
    selects: [
        {
            title: 'Факультет', 
            options: [{id: null, name: 'Все'}, {id: 0, name: 'Информатики'}, {id: 1, name: 'Политологии'}], 
            show: true
        },
        {
            title: 'Кафедра',  
            options: [{id: null, name: 'Все'}, {id: 0, name: 'Институт ракетно-космической техники'}, {id: 1, name: 'Институт двигателей и энергетических установок'}], 
            show: true
        },
        {
            title: 'Группа',  
            options: [{id: null, name: 'Все'}, {id: 0, name: '6213-010201D'}, {id: 1, name: '2402-020502A'}], 
            show: false
        }
    ]
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
        default:
            return state
    }
}
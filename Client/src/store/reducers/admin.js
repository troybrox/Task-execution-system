import { PUSH_USERS, PUSH_SELECTS } from "../actions/actionTypes"

const initialState = {
    users: [
        {name: 'Студент 1', department: '', faculty: '', check: false, show: true},
        {name: 'Студент 2', department: '', faculty: '', check: false, show: true},
        {name: 'Студент 3', department: '', faculty: '', check: true, show: true},
        {name: 'Студент 4', department: '', faculty: '', check: false, show: true}
    ],

    selects: [
        {
            title: 'Факультет', 
            options: ['Все', 'Информатики', 'Политологии'], 
            show: true
        },
        {
            title: 'Кафедра',  
            options: ['Все', 'Институт ракетно-космической техники', 'Институт двигателей и энергетических установок'], 
            show: true
        },
        {
            title: 'Группа',  
            options: ['Все', '6213-010201D', '2402-020502A'], 
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
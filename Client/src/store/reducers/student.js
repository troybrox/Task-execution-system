import { ERROR_WINDOW, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_LABS } from "../actions/actionTypes"

const initialState = {
    profileData: [
		{ value: 'pasha_terminator', label: 'Имя пользователя', type: 'text', serverName: 'UserName', valid: true },
        { value: 'Павел Карпович Александров', label: 'Фамилия Имя Отчество'},
        { value: 'Топовая 😎', label: 'Группа'},
		{ value: 'Кайфовый', label: 'Факультет'},
        { value: 'aaa@aa.aa', label: 'Адрес эл. почты', type: 'email', serverName: 'Email', valid: true }
    ],
    taskData: {
        subjects: [
            {
                id: 1,
                name: 'Моделирование сложных систем', 
                open: false
            },
            {
                id: 2,
                name: 'ЭВМ',
                open: false
            }
        ],
        types: [
            {id: null, name: 'Все'},
            {id: 1, name: 'Лабораторная работа'},
            {id: 2, name: 'Домашняя работа'},
        ],
    },
    labs: [
        {type: 'Лабораторная работа', name: '№1', dateOpen: '2 дня'},
        {type: 'Лабораторная работа', name: '№2', dateOpen: '1 месяц'},
        {type: 'Лабораторная работа', name: '№3', dateOpen: '3 дня'},
    ],

    errorShow: false,
    errorMessage: [],
}

export default function studentReducer(state = initialState, action) {
    switch (action.type) {
        case SUCCESS_PROFILE:
            return {
                ...state, profileData: action.profileData
            }
        case SUCCESS_TASK:
            return {
                ...state, taskData: action.taskData
            }
        case SUCCESS_LABS:
            return {
                ...state, labs: action.labs
            }
        case ERROR_WINDOW:
            return {
                ...state, 
                errorShow: action.errorShow, 
                errorMessage: action.errorMessage
            }
        default:
            return state
    }
}
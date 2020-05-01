import { ERROR_WINDOW, SUCCESS_PROFILE } from "../actions/actionTypes"

const initialState = {
    profileData: [
		{ value: 'pasha_terminator', label: 'Имя пользователя', type: 'text', serverName: 'UserName', valid: true },
        { value: 'Павел Карпович Александров', label: 'Фамилия Имя Отчество'},
        { value: 'Топовая 😎', label: 'Группа'},
		{ value: 'Кайфовый', label: 'Факультет'},
        { value: 'aaa@aa.aa', label: 'Адрес эл. почты', type: 'email', serverName: 'Email', valid: true }
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
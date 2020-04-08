import { AUTH_SUCCESS, LOGOUT, SUCCESS, ERROR_MESSAGE_AUTH } from "../actions/actionTypes"

const initialState = {
    title: 'Успешно',
    message: 'Действие прошло успешно! Дождитесь, пока администратор проверит информацию. Как только это произойдет, Вам на почту придет сообщение с подтверждением или отказом. Спасибо.',
    errorMessages: null,
    token: localStorage.getItem('token') || null,
    role: localStorage.getItem('role') || null
}

export default function authReadducer(state = initialState, action) {
    switch (action.type) {
        case AUTH_SUCCESS:
            return {
                ...state, token: action.token, role: action.role
            }
        case LOGOUT:
            return {
                ...state, token: null, role: null
            }
        case SUCCESS:
            return {
                ...state, role: action.role, title: action.title, message: action.message
            }
        case ERROR_MESSAGE_AUTH:
            return {
                ...state, errorMessages: action.errorMessageAuth
            }
        default:
            return state
    }
}
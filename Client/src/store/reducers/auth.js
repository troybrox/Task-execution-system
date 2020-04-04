import { AUTH_SUCCESS, LOGOUT, SUCCESS } from "../actions/actionTypes"

const initialState = {
    title: 'Успешно',
    message: 'Действие прошло успешно! Дождитесь, пока администратор проверит информацию. Как только это произойдет, Вам на почту придет сообщение с подтверждением или отказом. Спасибо.',
    token: null,
    role: 'admin'
}

export default function authReducer(state = initialState, action) {
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
        default:
            return state
    }
}
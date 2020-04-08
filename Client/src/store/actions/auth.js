import axios from 'axios'
import { AUTH_SUCCESS, LOGOUT, SUCCESS, ERROR_MESSAGE_AUTH } from './actionTypes'

export function registr(url, data) {
    return async dispatch => {
        const role = 'success'
        try {
            const response = await axios.post(url, data)
            const respData = response.data
            let title = 'Успешно'
            let message = 'Действие прошло успешно! Дождитесь, пока администратор проверит информацию. Как только это произойдет, Вам на почту придет сообщение с подтверждением или отказом. Спасибо.'
            
            if (respData.succeded) {
                dispatch(success(role, title, message))
            } else {
                title = 'Ошибка'
                message = respData.errorMessages.join('. /n')
                dispatch(success(role, title, message))
            }
        } catch (e) {
            dispatch(success(role, 'Ошибка', e.message))
        }
    }
}

export function auth(data) {
    return async dispatch => {
        try {
            const url = 'https://localhost:44303/api/account/login'
            const response = await axios.post(url, data)
            const respData = response.data
            // localStorage.setItem('token'), respData.idToken)
            // localStorage.setItem('userId'), respData.userId)
            // localStorage.setItem('role'), respData.role)

            if (respData.succede) {            
                dispatch(authSuccess(respData.idToken, respData.role))
            } else {
                dispatch(errorMessageAuth('Неверные данные!'))
            }
        } catch (e) {
            const role = 'success'
            dispatch(success(role, 'Ошибка', e.message))
        }
    }
}

export function success(role, title, message) {
    return {
        type: SUCCESS,
        role, title, message
    }
}

export function authSuccess(token, role) {
    return {
        type: AUTH_SUCCESS,
        token, role
    }
}

export function logout() {
    // localStorage.removeItem('token'), respData.idToken)
    // localStorage.removeItem('userId'), respData.userId)
    // localStorage.removeItem('role'), respData.role)
    return {
        type: LOGOUT
    }
}

export function errorMessageAuth(errorMessages) {
    return {
        type: ERROR_MESSAGE_AUTH,
        errorMessages
    }
}

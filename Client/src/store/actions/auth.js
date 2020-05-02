import axios from '../../axios/axiosRole'
import { 
    AUTH_SUCCESS, 
    LOGOUT, 
    SUCCESS, 
    // ERROR_MESSAGE_AUTH, 
    PUSH_FILTERS, 
    ERROR_WINDOW,
    LOADING_START } from './actionTypes'

export function registr(url, data) {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            console.log(data)
            const response = await axios.post(url, data)
            const respData = response.data
            
            if (respData.succeeded) {
                dispatch(success(true))
            } else {
                const err = [...respData.errorMessages]
                err.unshift('Сообщение с сервера')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = ['Ошибка подключения']
            err.push(e.message)
            dispatch(errorWindow(true, err))
        }
    }
}

export function auth(data) {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = 'api/account/login'
            const response = await axios.post(url, data)
            const respData = response.data
            localStorage.setItem('token', respData.data.idToken)
            localStorage.setItem('role', respData.data.role)
            document.cookie = `.AspNetCore.Application.Id=${respData.data.idToken}`

            if (respData.succeeded) {            
                dispatch(authSuccess(respData.data.idToken, respData.data.role))
            } else {
                const err = [...respData.errorMessages]
                err.unshift('Сообщение с сервера')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = ['Ошибка подключения']
            err.push(e.message)
            dispatch(errorWindow(true, err))
        }
    }
}

export function loadingFilters() {
    return async dispatch => {
        try {
            const url = 'api/account/filters'
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                const faculties = [{id: null, name: 'Выберите факультет'}]
	            const groups = [{id: null, name: 'Выберите группу'}]
	            const departments = [{id: null, name: 'Выберите кафедру'}]
    
                data.data.forEach(el => {
                    faculties.push({id: el.id, name: el.name})
                    el.groups.forEach(item => {
                        groups.push({id: item.id, name: item.name, facultyId: item.facultyId})
                    })
                    el.departments.forEach(item => {
                        departments.push({id: item.id, name: item.name, facultyId: item.facultyId})
                    })
                })
    
                dispatch(pushFilters(faculties, groups, departments))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера')
                dispatch(errorWindow(true, err))
            }

        } catch (e) {
            const err = ['Ошибка подключения']
            err.push(e.message)
            dispatch(errorWindow(true, err))
        }
    }
}

export function logoutHandler() {
    return async dispatch => {
        try {
            await axios.get('api/account/signout')
            localStorage.removeItem('token')
            localStorage.removeItem('role')
            dispatch(logout())
        } catch (e) {
            const err = ['Ошибка подключения']
            err.push(e.message)
            dispatch(errorWindow(true, err))
        }
    }
}

export function pushFilters(faculties, groups, departments) {
    return {
        type: PUSH_FILTERS,
        faculties, groups, departments
    }
}

export function success(successPage) {
    return {
        type: SUCCESS,
        successPage
    }
}

export function authSuccess(token, role) {
    return {
        type: AUTH_SUCCESS,
        token, role
    }
}

export function logout() {
    return {
        type: LOGOUT
    }
}

export function errorWindow(catchError, catchErrorMessage) {
    return {
        type: ERROR_WINDOW,
        catchError, catchErrorMessage
    }
}

export function loadingStart() {
    return {
        type: LOADING_START
    }
}


// export function errorMessageAuth(errorMessages) {
//     return {
//         type: ERROR_MESSAGE_AUTH,
//         errorMessages
//     }
// }

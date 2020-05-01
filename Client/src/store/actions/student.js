import axios from 'axios'
import { ERROR_WINDOW, SUCCESS_PROFILE } from './actionTypes'
import { commonURL } from './actionURL'

export function fetchProfile() {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/student/profile`
            const response = await axios.get(url)
            const data = response.data
            if (data.succeeded) {
                const labels = [
                    { label: 'Имя пользователя', serverName: 'UserName', type: 'text'},
                    { label: 'Фамилия Имя Отчество', serverName: 'Surname Name Patronymic'},
                    { label: 'Группа', serverName: 'GroupName'},
                    { label: 'Факультет', serverName: 'FacultyName'},
                    { label: 'Адрес эл. почты', serverName: 'Email', type: 'email'}
                ]

                const profileData = []
                labels.forEach(el => {
                    const value = []
                    el.serverName.split(' ').forEach((item) => {
                        value.push(data.data[item])
                    })
                    const obj = {label: el.label, value: value.join(' ')}
                    if ('type' in el) {
                        obj.type = el.type
                        obj.serverName = el.serverName 
                        obj.valid = true
                    }
                    
                    profileData.push(obj)
                })

                dispatch(successProfile(profileData))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = ['Ошибка подключения']
            err.push(e.message)
            dispatch(errorWindow(true, err))
        }
    }
}

export function onChangeProfile(value, index) {
    return async (dispatch, getState) => {
        const state = getState().student
        const profileData = [...state.profileData]
        profileData[index].value = value
        dispatch(successProfile(profileData))
    }
}

export function updateData(data, path) {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/student/profile/${path}`
            const response = await axios.post(url, data)
            const respData = response.data
            if (respData.succeeded) {
                dispatch(fetchProfile())
            } else {
                const err = [...respData.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = ['Ошибка подключения']
            err.push(e.message)
            dispatch(errorWindow(true, err))
        }
    }
}

export function updateProfile() {
    return async (dispatch, getState) => {
        const state = getState().student
        const profileData = [...state.profileData]
        const data = []
        profileData.forEach(el => {
            if ('serverName' in el) {
                const object = {}
                object[el.serverName] = el.value
                data.push(object)
            }
        })
        dispatch(updateData(data, 'update'))
    }
}

export function successProfile(profileData) {
    return {
        type: SUCCESS_PROFILE,
        profileData
    }
}

export function errorWindow(errorShow, errorMessage) {
    return {
        type: ERROR_WINDOW,
        errorShow, errorMessage
    }
}
import axios from 'axios'
import { ERROR_WINDOW, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_LABS } from './actionTypes'
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

export function fetchTaskFilters() {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/student/task/filters`
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                const taskData = {
                    subjects: [],
                    types: [{id: null, name: 'Все'}]
                }

                data.data.subjects.forEach((el) => {
                    taskData.subjects.push({id: el.id, name: el.name})
                })

                data.data.types.forEach(el => {
                    taskData.types.push(el)
                })

                dispatch(successTask(taskData))
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

export function choiceSubjectTask(indexSubject) {
    return async(dispatch, getState) => {
        const state = getState().student
        const taskData = state.taskData
        taskData.subjects.forEach(el => {
            if (el.id === indexSubject)
                el.open = true
            else 
                el.open = false
        })

        const filters = {
            subjectId: String(indexSubject),
        }

        await dispatch(fetchListTasks(filters))

        dispatch(successTask(taskData))
    }
}

export function fetchListTasks(filters) {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/student/task`
            const response = await axios.post(url, filters)
            const data = response.data

            if (data.succeeded) {
                const labs = []
                data.data.forEach(el => {
                    labs.push({type: el.type, name: el.name, dateOpen: el.beginDate})
                })

                dispatch(successLabs(labs))
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

export function successProfile(profileData) {
    return {
        type: SUCCESS_PROFILE,
        profileData
    }
}

export function successTask(taskData) {
    return {
        type: SUCCESS_TASK,
        taskData
    }
}

export function successLabs(labs) {
    return {
        type: SUCCESS_LABS,
        labs
    }
}

export function errorWindow(errorShow, errorMessage) {
    return {
        type: ERROR_WINDOW,
        errorShow, errorMessage
    }
}
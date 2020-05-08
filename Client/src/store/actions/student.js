import axios from '../../axios/axiosRole'
import { ERROR_WINDOW, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_TASKS, SUCCESS_TASK_ADDITION, LOADING_START } from './actionTypes'

export function fetchProfile() {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = 'api/student/profile'
            const response = await axios.get(url)
            const data = response.data
            if (data.succeeded) {
                const labels = [
                    { label: 'Имя пользователя', serverName: 'userName', type: 'text'},
                    { label: 'Фамилия', serverName: 'surname'},
                    { label: 'Имя', serverName: 'name'},
                    { label: 'Отчество', serverName: 'patronymic'},
                    { label: 'Группа', serverName: 'groupNumber'},
                    { label: 'Факультет', serverName: 'faculty'},
                    { label: 'Адрес эл. почты', serverName: 'email', type: 'email'}
                ]

                const profileData = []
                labels.forEach(el => {
                    const obj = {label: el.label, value: data.data[el.serverName], serverName: el.serverName}
                    if ('type' in el) {
                        obj.type = el.type
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
            const url = `api/student/profile/${path}`
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
        const data = {}
        profileData.forEach(el => {
            if ('serverName' in el)
                data[el.serverName] = el.value
        })
        dispatch(updateData(data, 'update'))
    }
}

export function fetchTaskFilters() {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = 'api/student/task/filters'
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
            const url = 'api/student/task'
            const response = await axios.post(url, filters)
            const data = response.data

            if (data.succeeded) {
                const tasks = []
                data.data.forEach(el => {
                    tasks.push({type: el.type, name: el.name, dateOpen: el.beginDate})
                })

                dispatch(successTasks(tasks))
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

export function fetchTaskById(id) {
    return async dispatch => {
        try {
            const url = `api/student/task/${id}`
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                dispatch(successTaskAddition(data.data))
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

export function loadingStart() {
    return {
        type: LOADING_START
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

export function successTasks(tasks) {
    return {
        type: SUCCESS_TASKS,
        tasks
    }
}

export function successTaskAddition(taskAdditionData) {
    return {
        type: SUCCESS_TASK_ADDITION,
        taskAdditionData
    }
}

export function errorWindow(errorShow, errorMessage) {
    return {
        type: ERROR_WINDOW,
        errorShow, errorMessage
    }
}

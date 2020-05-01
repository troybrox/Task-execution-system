import axios from 'axios'
import { ERROR_WINDOW, SUCCESS_TASK_ADDITION, SUCCESS_MAIN, SUCCESS_PROFILE } from './actionTypes'
import { commonURL } from './actionURL'

export function fetchProfile() {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/teacher/profile`
            const response = await axios.get(url)
            const data = response.data
            if (data.succeeded) {
                const labels = [
                    { label: 'Имя пользователя', serverName: 'UserName', type: 'text'},
                    { label: 'Фамилия Имя Отчество', serverName: 'Surname Name Patronymic'},
                    { label: 'Факультет', serverName: 'FacultyName'},
                    { label: 'Кафедра', serverName: 'DepartmentName'},
                    { label: 'Должность', serverName: 'Position'},
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
        const state = getState().teacher
        const profileData = [...state.profileData]
        profileData[index].value = value
        dispatch(successProfile(profileData))
    }
}

export function updateData(data, path) {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/teacher/profile/${path}`
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
        const state = getState().teacher
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

export function fetchMain() {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/teacher/main`
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                const mainData = [...data.data]

                mainData.forEach((item)=>{
                    item.open = false
                    if ('groups' in item)
                        item.groups.forEach((element)=>{
                            element.open = false
                            if ('students' in element)
                                element.students.forEach((el)=>{
                                    el.open = false
                                })
                        })
                })

                dispatch(successMain(mainData))
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

export function choiceSubjectHandler(indexSubject) {
    return (dispatch, getState) => {
        const state = getState().teacher
        const mainData = [...state.mainData]
        mainData[indexSubject].open = !mainData[indexSubject].open

        dispatch(successMain(mainData))
    }
}

export function choiceGroupHandler(indexSubject, indexGroup) {
    return (dispatch, getState) => {
        const state = getState().teacher
        const mainData = [...state.mainData]
        mainData.forEach(el => {
            if ('groups' in el)
                el.groups.forEach(element => {
                    element.open = false
                })
        })    
        mainData[indexSubject].groups[indexGroup].open = true

        dispatch(successMain(mainData))
    }
}

export function choiceStudentHandler(indexSubject, indexGroup, indexStudent) {
    return (dispatch, getState) => {
        const state = getState().teacher
        const mainData = [...state.mainData]
        const open = mainData[indexSubject].groups[indexGroup].students[indexStudent].open
        mainData[indexSubject].groups[indexGroup].students[indexStudent].open = !open

        dispatch(successMain(mainData))
    }
}

// export function fetchTasks() {}

export function fetchTaskById(id) {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/teacher/task/${id}`
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

// export function fetchRepository() {}

export function successProfile(profileData) {
    return {
        type: SUCCESS_PROFILE,
        profileData
    }
}

export function successMain(mainData) {
    return {
        type: SUCCESS_MAIN,
        mainData
    }
}

export function successTaskAddition(taskData) {
    return {
        type: SUCCESS_TASK_ADDITION,
        taskData
    }
}

export function errorWindow(errorShow, errorMessage) {
    return {
        type: ERROR_WINDOW,
        errorShow, errorMessage
    }
}
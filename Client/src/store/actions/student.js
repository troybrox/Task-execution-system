import axios from '../../axios/axiosRole'
import { ERROR_WINDOW, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_TASKS, SUCCESS_TASK_ADDITION, LOADING_START, SUCCESS_REPOSITORY, SUCCESS_SUBJECT_FULL } from './actionTypes'

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

                data.data.subjects.forEach((el, num) => {
                    const object = {id: el.id, name: el.name}
                    if (num === 0)
                        object.open = true
                    else object.open = false
                    taskData.subjects.push(object)
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

export function choiceSubjectTask(filters) {
    return async(dispatch, getState) => {
        const state = getState().student
        const taskData = state.taskData
        taskData.subjects.forEach(el => {
            if (el.id === +filters[0].value)
                el.open = true
            else
                el.open = false
        })    

        await dispatch(fetchListTasks(filters))

        dispatch(successTask(taskData))
    }
}

function parseToString(num) {
    if (num < 10) 
        return ('0' + num)
    else 
        return num
}

function parseDate(date) {
    const day = parseToString(date.getDate())
    const month = parseToString(date.getMonth() + 1)
    const year = parseToString(date.getFullYear())
    const hours = parseToString(date.getHours())
    const minutes = parseToString(date.getMinutes())

    return (day + '.' + month + '.' + year + ' ' + hours + ':' + minutes)
}

export function fetchListTasks(filters) {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = 'api/student/tasks'
            const response = await axios.post(url, filters)
            const data = response.data

            if (data.succeeded) {
                const tasks = [...data.data]
                tasks.forEach(el => {
                    el.beginDate = parseDate(new Date(el.beginDate))
                    el.updateDate = parseDate(new Date(el.updateDate))
                    el.finishDate = parseDate(new Date(el.finishDate))
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
        dispatch(loadingStart())
        try {
            const url = `api/student/task/${id}`
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                const taskAdditionData = data.data
                taskAdditionData.beginDate = parseDate(new Date(data.data.beginDate)) 
                taskAdditionData.finishDate = parseDate(new Date(data.data.finishDate)) 
    
                if (taskAdditionData.solution !== null) {
                    taskAdditionData.solution.creationDate = parseDate(new Date(data.data.solution.creationDate))
                }

                if (taskAdditionData.updateDate !== null) {
                    taskAdditionData.updateDate = parseDate(new Date(data.data.updateDate))
                }

                dispatch(successTaskAddition(taskAdditionData))
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

export function onSendSolution(createSolution, id) {
    return async dispatch => {
        try {
            const url = 'api/student/solution/add'
            const response = await axios.post(url, createSolution.task)
            const data = response.data

            if (data.succeeded) {
                if (createSolution.file !== null)
                    try {
                        const url2 = 'api/student/solution/add/file'
                        const file = new FormData()
                        file.append('taskId', data.data)
                        file.append('file', createSolution.file)
                        const response2 = await axios.post(url2, file)
                        const data2 = response2.data

                        if (data2.succeeded) {
                            dispatch(fetchTaskById(id))
                        } else {
                            dispatch(fetchTaskById(id))
                            const err = [...data2.errorMessages]
                            err.unshift('Сообщение с сервера.')
                            dispatch(errorWindow(true, err))
                        }
                    } catch (error) {
                        dispatch(fetchTaskById(id))
                        const err = ['Ошибка подключения']
                        err.push(error.message)
                        dispatch(errorWindow(true, err))
                    }
                else 
                    dispatch(fetchTaskById(id))
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

export function fetchRepository() {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const response = await axios.get('/api/student/repo/subjects')
            const data = response.data
            if (data.succeeded) {
                const repositoryData = []
                data.data.forEach(el => {
                    const object = el
                    object.open = false

                    repositoryData.push(object)
                })
                dispatch(successRepository(repositoryData))
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

export function choiceSubjectHandler(index) {
    return (dispatch, getState) => {
        const state = getState().student
        const repositoryData = [...state.repositoryData]
        repositoryData[index].open = !repositoryData[index].open

        dispatch(successRepository(repositoryData))

        const filters = [
            {name: 'subjectId', value: String(repositoryData[index].id)}
        ]
        dispatch(fetchSubjectFull(filters))
    }
}

export function fetchSubjectFull(filters) {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = '/api/student/repo'
            const response = await axios.post(url, filters)
            const data = response.data
            if (data.succeeded) {
                const subjectFullData = []
                data.data.forEach(el => {
                    el.open = false
                    subjectFullData.push(el)
                })
                dispatch(successSubjectFull(subjectFullData))
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

export function successRepository(repositoryData) {
    return {
        type: SUCCESS_REPOSITORY,
        repositoryData
    }
}

export function successSubjectFull(subjectFullData) {
    return {
        type: SUCCESS_SUBJECT_FULL,
        subjectFullData
    }
}

export function errorWindow(errorShow, errorMessage) {
    return {
        type: ERROR_WINDOW,
        errorShow, errorMessage
    }
}

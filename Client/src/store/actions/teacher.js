import axios from '../../axios/axiosRole'
import { ERROR_WINDOW, SUCCESS_TASK_ADDITION, SUCCESS_MAIN, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_TASKS, SUCCESS_CREATE, SUCCESS_CREATE_DATA, LOADING_START, SUCCESS_CREATE_REPOSITORY, SUCCESS_REPOSITORY, SUCCESS_CREATE_REPOSITORY_END, SUCCESS_SUBJECT_FULL, LOGOUT, GOOD_NEWS } from './actionTypes'
import { logoutHandler } from './auth'

export function fetchProfile() {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = 'api/teacher/profile'
            const response = await axios.get(url)
            const data = response.data
            if (data.succeeded) {
                const labels = [
                    { label: 'Имя пользователя', serverName: 'userName', type: 'text'},
                    { label: 'Фамилия', serverName: 'surname'},
                    { label: 'Имя', serverName: 'name'},
                    { label: 'Отчество', serverName: 'patronymic'},
                    { label: 'Факультет', serverName: 'facultyName'},
                    { label: 'Кафедра', serverName: 'departmentName'},
                    { label: 'Должность', serverName: 'position', type: 'text'},
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
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
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

// Тут сделать после изменения пароля - выход
export function updateData(data, path) {
    return async dispatch => {
        try {
            const url = `api/teacher/profile/${path}`
            const response = await axios.post(url, data)
            const respData = response.data
            if (respData.succeeded) {
                if (path === 'update')
                    dispatch(fetchProfile())
                else {
                    dispatch(goodNewsHandler())
                    const err = ['Необходимо заново авторизироваться в системе, ']
                    err.push('чтобы обновление данных вступило в силу.')
                    err.push('Спасибо!')
                    let n = 6
                    err.push(`Выход из системы через ${n}...`)
                    let timerId = setInterval(() => {
                        dispatch(errorWindow(false, []))
                        err.pop()
                        n = n - 1
                        err.push(`Выход из системы через ${n}...`)
                        dispatch(errorWindow(true, err))
                    }, 1000)

                    setTimeout(() => {
                        clearInterval(timerId)
                        dispatch(logoutHandler())
                    }, 6000)
                }
            } else {
                const err = [...respData.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function updateProfile() {
    return async (dispatch, getState) => {
        const state = getState().teacher
        const profileData = [...state.profileData]
        const data = {}
        profileData.forEach(el => {
            if ('serverName' in el)
                data[el.serverName] = el.value
        })
        dispatch(updateData(data, 'update'))
    }
}

export function fetchMain() {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = 'api/teacher/main'
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                const mainData = [...data.data]
                if (mainData !== [])
                    mainData.forEach((item, num)=>{
                        if (num === 0) 
                            item.open = true
                        else 
                            item.open = false

                        item.groups.forEach((element, index)=>{
                            if (index === 0 && num === 0) element.open = true
                            else element.open = false
                            
                            element.students.forEach((el)=>{
                                el.open = false
                                if (el.tasks.length !== 0)
                                    el.tasks.forEach((task) => {
                                        task.beginDate = parseDate(new Date(task.beginDate))
                                        if (task.finishDate !== null)
                                            task.finishDate = parseDate(new Date(task.finishDate))
                                        if (task.updateDate !== null)
                                            task.updateDate = parseDate(new Date(task.updateDate))
                                    })
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
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function choiceSubjectMain(indexSubject) {
    return (dispatch, getState) => {
        const state = getState().teacher
        const mainData = [...state.mainData]
        mainData[indexSubject].open = !mainData[indexSubject].open

        dispatch(successMain(mainData))
    }
}

export function choiceGroupMain(indexSubject, indexGroup) {
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

export function fetchTaskFilters() {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = 'api/teacher/task/filters'
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                const taskData = {
                    subjects: [],
                    types: [{id: null, name: 'Все'}]
                }
                
                if ('subjects' in data.data)
                    data.data.subjects.forEach((el, num) => {
                        const object = {id: el.id, name: el.name, groups: [], open: false}
                        if (num === 0)
                            object.open = true
                        
                        if ('groups' in el)
                            el.groups.forEach((element, index) => {
                                const group = {
                                    id: element.id, 
                                    name: element.name, 
                                    open: false
                                }

                                if (index === 0 && num === 0)
                                    group.open = true
                            
                                object.groups.push(group)
                            })

                        taskData.subjects.push(object)
                    })

                if ('types' in data.data)
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
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function choiceSubjectTask(indexSubject) {
    return (dispatch, getState) => {
        const state = getState().teacher
        const taskData = state.taskData
        taskData.subjects[indexSubject].open = !taskData.subjects[indexSubject].open

        dispatch(successTask(taskData))
    }
}

export function choiceGroupTask(indexSubject, indexGroup) {
    return async (dispatch, getState) => {
        const state = getState().teacher
        const taskData = state.taskData
        taskData.subjects.forEach(el => {
            if ('groups' in el)
                el.groups.forEach(element => {
                    element.open = false
                })
        })
        taskData.subjects[indexSubject].groups[indexGroup].open = true

        const filters = [
            {name: 'subjectId', value: String(taskData.subjects[indexSubject].id)},
            {name: 'groupId', value: String(taskData.subjects[indexSubject].groups[indexGroup].id)}
        ]

        await dispatch(fetchListTasks(filters))

        dispatch(successTask(taskData))
    }
}

export function fetchListTasks(filters) {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = 'api/teacher/task'
            const response = await axios.post(url, filters)
            const data = response.data

            if (data.succeeded) {
                const tasks = [...data.data]
                tasks.forEach(el => {
                    el.beginDate = parseDate(new Date(el.beginDate))
                    if (el.updateDate !== null) el.updateDate = parseDate(new Date(el.updateDate))
                    el.finishDate = parseDate(new Date(el.finishDate))
                })

                dispatch(successTasks(tasks))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
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

export function fetchTaskById(id) {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const url = `api/teacher/task/${id}`
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                const taskAdditionData = data.data
                const beginDate = new Date(data.data.beginDate)
                const finishDate = new Date(data.data.finishDate)
                taskAdditionData.beginDate = parseDate(beginDate) 
                taskAdditionData.finishDate = parseDate(finishDate) 
                if (taskAdditionData.solutions.length !== 0) {
                    taskAdditionData.solutions.forEach(el => {
                        el.creationDate = parseDate(new Date(el.creationDate))
                    })
                }

                if (taskAdditionData.solution !== null) {
                    taskAdditionData.solution.creationDate = parseDate(new Date(data.data.solutions.creationDate))
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
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function fetchTaskCreate() {
    return async dispatch => {
        try {
            const url = 'api/teacher/task/add/filters'
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                const createData = {}
                for (let key in data.data) {
                    createData[key] = data.data[key]
                }
                createData.subjects.unshift({id: null, name: 'Выберите предмет'})
                createData.types.unshift({id: null, name: 'Выберите тип'})
                createData.groups.unshift({id: null, name: 'Выберите группу'})
                dispatch(successCreateData(data.data))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function changeChecked(groupIndex, studentIndex, val) {
    return (dispatch, getState) => {
        const state = getState().teacher
        const createData = state.createData

        if (studentIndex !== null)
            createData.groups[groupIndex].students[studentIndex].check = !createData.groups[groupIndex].students[studentIndex].check
        else {

            createData.groups[groupIndex].students.forEach(el => {
                el.check = val
            })
        }
        dispatch(successCreateData(createData))
    }
}

export function onSendCreate(task, path) {
    return async dispatch => {
        try {
            const url = `api/teacher/task/${path}`
            const response = await axios.post(url, task.task)
            const data = response.data

            if (data.succeeded) {
                let idTask = data.data
                let titleId = 'taskId'
                if (path === 'update') {
                    idTask = task.task.id
                    titleId = 'id'
                }
                if (task.file !== null)
                    try {
                        const url2 = 'api/teacher/task/add/file'
                        const file = new FormData()
                        file.append(titleId, idTask)
                        file.append('file', task.file)
                        const response2 = await axios.post(url2, file)
                        const data2 = response2.data

                        if (data2.succeeded) {
                            dispatch(successCreate(+idTask))
                        } else {
                            const err = [...data2.errorMessages]
                            err.unshift('Сообщение с сервера.')
                            dispatch(errorWindow(true, err))
                        }

                    } catch (error) {
                        const err = [`Ошибка подключения: ${error.name}`]
                        err.push(error.message)
                        if (error.response !== undefined)
                            if (error.response.status !== undefined)
                                if (error.response.status === 401 || error.response.status === 403) {
                                    let n = 4
                                    err.push(`Выход из системы через ${n}...`)
                                    let timerId = setInterval(() => {
                                        dispatch(errorWindow(false, []))
                                        err.pop()
                                        n = n - 1
                                        err.push(`Выход из системы через ${n}...`)
                                        dispatch(errorWindow(true, err))
                                    }, 1000)
                    
                                    setTimeout(() => {
                                        clearInterval(timerId)
                                        dispatch(logoutHandler())
                                    }, 4000)
                                }
                                else 
                                    dispatch(errorWindow(true, err))
                            else 
                                dispatch(errorWindow(true, err))
                        else
                            dispatch(errorWindow(true, err))
                    }
                else 
                    dispatch(successCreate(+idTask))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }

        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function onCloseTask(id) {
    return async dispatch => {
        try {
            const response = await axios.get(`api/teacher/task/${id}/close`)
            const data = response.data
            if (data.succeeded) {
                dispatch(fetchTaskById(id))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function fetchRepository() {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const response = await axios.get('/api/teacher/repo/subjects')
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
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function choiceSubjectHandler(index) {
    return (dispatch, getState) => {
        const state = getState().teacher
        const repositoryData = [...state.repositoryData]
        repositoryData[index].open = !repositoryData[index].open

        dispatch(successRepository(repositoryData))

        const filters = [
            {name: 'subjectId', value: String(repositoryData[index].id)}
        ]
        dispatch(fetchSubjectFull(filters))
    }
}

export function choiceRepoHandler(index) {
    return (dispatch, getState) => {
        const state = getState().teacher
        const subjectFullData = [...state.subjectFullData]
        subjectFullData[index].open = !subjectFullData[index].open

        dispatch(successSubjectFull(subjectFullData))
    }    
}

export function fetchSubjectFull(filters) {
    return (dispatch, getState) => {
        const state = getState().teacher
        const repositoryData = state.repositoryData
        repositoryData.forEach(async element => {
            if (element.id === +filters[0].value && element.open) {
                dispatch(loadingStart())
                try {
                    const url = '/api/teacher/repo'
                    const response = await axios.post(url, filters)
                    const data = response.data
                    if (data.succeeded) {
                        const subjectFullData = [...state.subjectFullData]
                        data.data.forEach(el => {
                            let index = null
                            subjectFullData.forEach((element, num) => {
                                if (el.id === element.id) index = num
                            })
                            if (index === null) {
                                el.open = false
                                subjectFullData.push(el)
                            }                        
                        })
                        dispatch(successSubjectFull(subjectFullData))
                    } else {
                        const err = [...data.errorMessages]
                        err.unshift('Сообщение с сервера.')
                        dispatch(errorWindow(true, err))
                    }
                } catch (e) {
                    const err = [`Ошибка подключения: ${e.name}`]
                    err.push(e.message)
                    if (e.response !== undefined)
                        if (e.response.status !== undefined)
                            if (e.response.status === 401 || e.response.status === 403) {
                                let n = 4
                                err.push(`Выход из системы через ${n}...`)
                                let timerId = setInterval(() => {
                                    dispatch(errorWindow(false, []))
                                    err.pop()
                                    n = n - 1
                                    err.push(`Выход из системы через ${n}...`)
                                    dispatch(errorWindow(true, err))
                                }, 1000)
                
                                setTimeout(() => {
                                    clearInterval(timerId)
                                    dispatch(logoutHandler())
                                }, 4000)
                            }
                            else 
                                dispatch(errorWindow(true, err))
                        else
                            dispatch(errorWindow(true, err))
                    else
                        dispatch(errorWindow(true, err))
                }
            }
        })
    }
}

export function deleteRepo(index) {
    return async (dispatch, getState) => {
        const state = getState().teacher
        const subjectFullData = [...state.subjectFullData]
        const id = subjectFullData[index].id

        try {
            const response = await axios.post('/api/teacher/repo/delete', [id])
            const data = response.data
            if (data.succeeded) {
                subjectFullData.splice(index, 1)
                dispatch(successSubjectFull(subjectFullData))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function editRepo(index, contentText, name) {
    return async (dispatch, getState) => {
        const state = getState().teacher
        const subjectFullData = [...state.subjectFullData]
        subjectFullData[index].contentText = contentText
        subjectFullData[index].name = name

        try {
            const response = await axios.post('/api/teacher/repo/update', subjectFullData[index])
            const data = response.data
            if (data.succeeded) {
                dispatch(successSubjectFull(subjectFullData))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function fetchCreateRepository() {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const response = await axios.get('/api/teacher/repo/add/filters')
            const data = response.data
            if (data.succeeded) {
                const createRepository = [...data.data]
                createRepository.unshift({id: null, name: 'Выбрать предмет'})
                dispatch(successCreateRepository(createRepository))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
            
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function sendCreateRepository(filters) {
    return async dispatch => {
        try {
            const response = await axios.post('/api/teacher/repo/add', filters.repo)
            const data = response.data
            if (data.succeeded) {
                if (filters.file !== null) {
                    const filters2 = new FormData()
                    filters2.append('repoId', data.data)
                    filters2.append('file', filters.file)
                    dispatch(sendCreateRepositoryFile(filters2))
                } else {
                    dispatch(successCreateRepositoryEnd())
                }
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
            if (e.response !== undefined)
                if (e.response.status !== undefined)
                    if (e.response.status === 401 || e.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
                dispatch(errorWindow(true, err))
        }
    }
}

export function sendCreateRepositoryFile(filters) {
    return async dispatch => {
        try {
            const response = await axios.post('/api/teacher/repo/add/file', filters)
            const data = response.data

                if (data.succeeded) {
                    dispatch(successCreateRepositoryEnd())
                } else {
                    dispatch(successCreateRepositoryEnd())
                    const err = [...data.errorMessages]
                    err.unshift('Сообщение с сервера.')
                    dispatch(errorWindow(true, err))
                }
        } catch (error) {
            dispatch(successCreateRepositoryEnd())
            const err = [`Ошибка подключения: ${error.name}`]
            err.push(error.message)
            if (error.response !== undefined)
                if (error.response.status !== undefined)
                    if (error.response.status === 401 || error.response.status === 403) {
                        let n = 4
                        err.push(`Выход из системы через ${n}...`)
                        let timerId = setInterval(() => {
                            dispatch(errorWindow(false, []))
                            err.pop()
                            n = n - 1
                            err.push(`Выход из системы через ${n}...`)
                            dispatch(errorWindow(true, err))
                        }, 1000)

                        setTimeout(() => {
                            clearInterval(timerId)
                            dispatch(logoutHandler())
                        }, 4000)
                    }
                    else 
                        dispatch(errorWindow(true, err))
                else
                    dispatch(errorWindow(true, err))
            else
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

export function successMain(mainData) {
    return {
        type: SUCCESS_MAIN,
        mainData
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

export function successCreateData(createData) {
    return {
        type: SUCCESS_CREATE_DATA,
        createData
    }
}

export function successCreate(successId) {
    return {
        type: SUCCESS_CREATE,
        successId
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

export function successCreateRepository(createRepository) {
    return {
        type: SUCCESS_CREATE_REPOSITORY,
        createRepository
    }
}

export function successCreateRepositoryEnd() {
    return {
        type: SUCCESS_CREATE_REPOSITORY_END
    }
}

export function errorWindow(errorShow, errorMessage) {
    return {
        type: ERROR_WINDOW,
        errorShow, errorMessage
    }
}

export function goodNewsHandler() {
    return {
        type: GOOD_NEWS
    }
}

export function logoutTeacher() {
    return {
        type: LOGOUT
    }
}
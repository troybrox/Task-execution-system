import axios from '../../axios/axiosRole'
import { ERROR_WINDOW, SUCCESS_TASK_ADDITION, SUCCESS_MAIN, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_TASKS, SUCCESS_CREATE, SUCCESS_CREATE_DATA, LOADING_START, SUCCESS_CREATE_REPOSITORY, SUCCESS_REPOSITORY, SUCCESS_CREATE_REPOSITORY_END, SUCCESS_SUBJECT_FULL } from './actionTypes'

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

// Тут сделать после изменения пароля - выход
export function updateData(data, path) {
    return async dispatch => {
        try {
            const url = `api/teacher/profile/${path}`
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
            const err = ['Ошибка подключения']
            err.push(e.message)
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
            const err = ['Ошибка подключения']
            err.push(e.message)
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
                const tasks = []
                if (data.data.length !== 0) {
                    data.data.forEach(el => {
                        const beginDate = new Date(el.beginDate) 
                        const object = {id: el.id, type: el.type, name: el.name, dateOpen: parseDate(beginDate)}
                    
                        object.countAnswers = el.solutionsCount 
                        object.countStudents = el.studentsCount
    
                        tasks.push(object)
                    })
                }

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
                    taskAdditionData.solutions.creationDate = parseDate(new Date(data.data.solutions.creationDate))
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
            const err = ['Ошибка подключения']
            err.push(e.message)
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
            const err = ['Ошибка подключения']
            err.push(e.message)
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

export function onSendCreate(task) {
    return async dispatch => {
        try {
            const url = 'api/teacher/task/add'
            const response = await axios.post(url, task.task)
            const data = response.data

            if (data.succeeded) {
                if (task.file !== null)
                    try {
                        const url2 = 'api/teacher/task/add/file'
                        const file = new FormData()
                        file.append('taskId', data.data)
                        file.append('file', task.file)
                        const response2 = await axios.post(url2, file)
                        const data2 = response2.data

                        if (data2.succeeded) {
                            dispatch(successCreate(+data.data))
                        } else {
                            const err = [...data2.errorMessages]
                            err.unshift('Сообщение с сервера.')
                            dispatch(errorWindow(true, err))
                        }

                    } catch (error) {
                        const err = ['Ошибка подключения']
                        err.push(error.message)
                        dispatch(errorWindow(true, err))
                    }
                else 
                    dispatch(successCreate(+data.data))
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
            const response = await axios.get('/api/teacher/repo/subjects')
            const data = response.data
            if (data.succeeded) {
                const repositoryData = []
                data.data.forEach((el, index) => {
                    const object = el
                    if (index === 0)
                        object.open = true
                    else
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
        const state = getState().teacher
        const repositoryData = [...state.repositoryData]
        repositoryData.forEach((el, num) => {
            if (num === index) el.open = true
            else el.open = false
        })

        dispatch(successRepository(repositoryData))

        const filters = [
            {name: 'subjectId', value: String(repositoryData[index].id)}
        ]
        dispatch(fetchSubjectFull(filters))
    }
}

export function fetchSubjectFull(filters) {
    return async dispatch => {
        try {
            const url = '/api/teacher/repo'
            const response = await axios.post(url, filters)
            const data = response.data
            if (data.succeeded) {
                const subjectFullData = []
                data.data.forEach(el => {
                    
                })
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
            const err = ['Ошибка подключения']
            err.push(e.message)
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
                if (filters.file !== null)
                    try {
                        const filters2 = {
                            repoId: data.data,
                            file: filters.file
                        }
                        const response2 = await axios.post('/api/teacher/repo/add/file', filters2)
                        const data2 = response2.data

                            if (data2.succeeded) {
                                dispatch(successCreateRepositoryEnd())
                            } else {
                                const err = [...data2.errorMessages]
                                err.unshift('Сообщение с сервера.')
                                dispatch(errorWindow(true, err))
                            }
                    } catch (error) {
                        dispatch(successCreateRepositoryEnd())
                        const err = [...data.errorMessages]
                        err.unshift('Сообщение с сервера.')
                        dispatch(errorWindow(true, err))
                    }
                else {
                    dispatch(successCreateRepositoryEnd())
                }
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

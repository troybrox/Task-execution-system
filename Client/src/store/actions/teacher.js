import axios from '../../axios/axiosRole'
import { ERROR_WINDOW, SUCCESS_TASK_ADDITION, SUCCESS_MAIN, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_LABS, SUCCESS_CREATE, SUCCESS_CREATE_DATA } from './actionTypes'

export function fetchProfile() {
    return async dispatch => {
        try {
            const url = 'api/teacher/profile'
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
        try {
            const url = 'api/teacher/main'
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
        try {
            const url = 'api/teacher/task/filters'
            const response = await axios.get(url)
            const data = response.data

            if (data.succeeded) {
                const taskData = {
                    subjects: [],
                    types: [{id: null, name: 'Все'}]
                }

                data.data.subjects.forEach((el) => {
                    const object = {id: el.id, name: el.name, groups: []}
                    el.groups.forEach(element => {
                        object.groups.push({id: element.id, number: element.number})
                    })
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
        try {
            const url = 'api/teacher/task'
            const response = await axios.post(url, filters)
            const data = response.data

            if (data.succeeded) {
                const labs = []
                data.data.forEach(el => {
                    const object = {type: el.type, name: el.name, dateOpen: el.beginDate}
                    let countAnswers = 0
                    el.students.forEach(element => {
                        if (element.solution !== null)
                            countAnswers++
                    })
                    object.countAnswers = countAnswers

                    labs.push(object)
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

export function fetchTaskById(id) {
    return async dispatch => {
        try {
            const url = 'api/teacher/task/${id}'
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
    console.log(task)
    return async dispatch => {
        try {
            const url = 'api/teacher/task/add'
            const response = await axios.post(url, task)
            const data = response.data

            if (data.succeeded) {
                const err = ['Это временное окно, чтобы видно было, что сервер работает. Доделаю.']
                err.unshift('Успешно создано!')
                dispatch(errorWindow(true, err))
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

export function errorWindow(errorShow, errorMessage) {
    return {
        type: ERROR_WINDOW,
        errorShow, errorMessage
    }
}
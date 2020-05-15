import axios from '../../axios/axiosRole'
import { 
    LOADING_START, 
    PUSH_USERS, 
    PUSH_SELECTS, 
    ERROR_WINDOW, 
    CHANGE_CONDITION, 
    LOGOUT} from './actionTypes'
import { logoutHandler } from './auth'

export function changeCheckedHandler(index) {
    return (dispatch, getState) => {
        const state = getState().admin
        const users = state.users
        users[index].check = !users[index].check

        dispatch(pushUsers(users))
    }
}

export function loadingUsers(url, facultyId, groupId, departmentId, searchString) {
    return async dispatch => {
        dispatch(loadingStart())
        try {
            const data = []

            if (searchString.trim() !== '') data.push({name: 'searchString', value: searchString})
            if (groupId !== null || departmentId !== null) {
                if (groupId !== null) data.push({name: 'groupId', value: groupId})
                else data.push({name: 'departmentId', value: departmentId})
            } else {
                if (facultyId !== null) data.push({name: 'facultyId', value: facultyId})
            }

            const response = await axios.post(url, data)
            
            if (response.data.succeeded) {
                const users = response.data.data
                const finalUsers = []
                users.forEach(el => {
                    const name = `${el.surname} ${el.name} ${el.patronymic}`
                    let additional
                    if ('departmentName' in el)
                        additional = `Факультет ${el.faculty}. Кафедра ${el.departmentName}`
                    else 
                        additional = `Факультет ${el.faculty}. Группа ${el.groupNumber}`
                    finalUsers.push({id: el.id, name, additional, check: false})
                })

                dispatch(pushUsers(finalUsers))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
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
        }
    }
}

export function loadingLists(url, roleActive) {
    return async dispatch => {
        try {
            const response = await axios.get(url)
            const data = response.data
            if (data.succeeded) {
                const selects = [
                    {
                        title: 'Факультет', 
                        options: [{id: null, name: 'Все'}], 
                        show: true
                    },
                    {
                        title: 'Кафедра',  
                        options: [{id: null, name: 'Все'}], 
                        show: true && roleActive
                    },
                    {
                        title: 'Группа',  
                        options: [{id: null, name: 'Все'}], 
                        show: true && !roleActive 
                    }
                ]
    
                data.data.forEach(el => {
                    selects[0].options.push({id: el.id, name: el.name})
                    el.departments.forEach(item => {
                        selects[1].options.push({id: item.id, name: item.name, facultyId: item.facultyId})
                    })
                    el.groups.forEach(item => {
                        selects[2].options.push({id: item.id, name: item.name, facultyId: item.facultyId})
                    })
                })
    
                dispatch(pushLists(selects))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
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
        }
    }
}

export function actionUsersHandler(url) {
    return async (dispatch, getState) => {
        await dispatch(changeCondition('loading'))
        
        const idList = []
        const newList = []
        const state = getState().admin

        state.users.forEach(el => {
            if (el.check) idList.push(el.id)
            else newList.push(el)
        })

        try {
            const response = await axios.post(url, idList)
            const data = response.data
            if (data.succeeded) {
                dispatch(changeCondition('ready'))
                setTimeout(() => dispatch(changeCondition(null)), 7000)
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err)) 
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
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
        }
    }
}

export function deleteGroupHandler(url, groupId) {
    return async dispatch => {
        dispatch(changeCondition('loading'))

        try {
            const response = await axios.post(url, {groupId})
            const data = response.data
            if (data.succeeded) {
                dispatch(changeCondition('ready'))
            } else {
                const err = [...data.errorMessages]
                err.unshift('Сообщение с сервера.')
                dispatch(errorWindow(true, err))
            }
        } catch (e) {
            const err = [`Ошибка подключения: ${e.name}`]
            err.push(e.message)
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
        }
    }
}

export function loadingStart() {
    return {
        type: LOADING_START
    }
}

export function pushUsers(users) {
    return {
        type: PUSH_USERS,
        users
    }
}

export function pushLists(selects) {
    return {
        type: PUSH_SELECTS,
        selects
    }
}

export function errorWindow(errorShow, errorMessage) {
    return {
        type: ERROR_WINDOW,
        errorShow, errorMessage
    }
}

export function changeCondition(actionCondition) {
    return {
        type: CHANGE_CONDITION,
        actionCondition
    }
}

export function logoutAdmin() {
    return {
        type: LOGOUT
    }
}
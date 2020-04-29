import axios from 'axios'
import { PUSH_USERS, PUSH_SELECTS, ERROR_WINDOW } from './actionTypes'

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
                    const name = el.surname + ' ' + el.name + ' ' + el.patronymic
                    let additional
                    if (el.position === 'Преподаватель')
                        additional = el.faculty + '. Кафедра ' + el.departmentName
                    else 
                        additional = el.faculty + '. Группа ' + el.groupNumber
                    finalUsers.push({id: el.id, name, additional, check: false})
                })

                dispatch(pushUsers(users))
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
                    el.groups.forEach(item => {
                        selects[1].options.push({id: item.id, name: item.name, facultyId: el.id})
                    })
                    el.departments.forEach(item => {
                        selects[2].options.push({id: item.id, name: item.name, facultyId: el.id})
                    })
                })
    
                dispatch(pushLists(selects))
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

export function actionUsersHandler(url) {
    return async (dispatch, getState) => {
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
                dispatch(pushUsers(newList))
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

export function deleteGroupHandler(url) {
    return async (dispatch, getState) => {
        const idList = []
        const state = getState().admin

        state.users.forEach(el => {
            idList.push(el.id)
        })

        try {
            console.log(url)
            console.log(idList)
            const response = await axios.post(url, idList)
            const data = response.data
            if (data.succeeded) {
                dispatch(pushUsers([]))
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
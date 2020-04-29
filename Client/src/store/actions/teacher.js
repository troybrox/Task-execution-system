import axios from 'axios'
import { ERROR_WINDOW, SUCCESS_TASK_ADDITION, SUCCESS_MAIN } from './actionTypes'
import { commonURL } from './actionURL'

export function fetchProfile() {}

export function fetchMain() {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/some` // ADDRESS MAIN
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


export function fetchTasks() {}

export function fetchTaskById(id) {
    return async dispatch => {
        try {
            const url = `${commonURL}/api/some` // ADDRESS TASK_ADDITION
            const response = await axios.post(url, id)
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

export function fetchRepository() {}

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
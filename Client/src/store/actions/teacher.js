import axios from 'axios'
import { ERROR_WINDOW, SUCCESS_TASK_ADDITION } from './actionTypes'

export function fetchTaskById(id) {
    return async (dispatch) => {
        try {
            const url = ''
            const response = await axios.post(url, id)
            const data = response.data

            if (data.succeeded) {
                const teacherData = {}
                const taskData = {}

                for (let key in data.data) {
                    if (key === 'teaherName' || key === 'teaherSurname' || key === 'teaherPatronymic')
                        teacherData[key] = data.data[key]
                    else 
                        taskData[key] = data.data[key]
                }
                dispatch(successTaskAddition(teacherData, taskData))
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

export function successTaskAddition(teacherData, taskData) {
    return {
        type: SUCCESS_TASK_ADDITION,
        teacherData, taskData
    }
}

export function errorWindow(errorShow, errorMessage) {
    return {
        type: ERROR_WINDOW,
        errorShow, errorMessage
    }
}
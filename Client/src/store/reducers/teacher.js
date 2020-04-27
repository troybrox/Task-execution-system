import { ERROR_WINDOW, SUCCESS_TASK_ADDITION } from "../actions/actionTypes"

const initialState = {
    teacherData: {
        teaherName: "Xxx",
        teaherSurname: "Xxx",
        teaherPatronymic: "Xxx",
    }, 
    taskData: {
        subject: "xx",
        type: "Лабораторная работа",
        name: "xx",
        contentText: "xxxxxxx",
        fileURI: "https://localhost44303/files/taskFile/Math_Lab1_task.docx",
        group: "6315-020304D",
        beginDate: "dd.mm.yyyy",
        finishDate: "dd.mm.yyyy",
        updateDate: "dd.mm.yyyy",
        isOpen: true,
        timeBar: 12,
        students: [
            {
                id: 1,
                name: "Подзаголовкин",
                surname: "Лупа",
                solution: {
                    contentText: "xxxxxxx",
                    creationDate: "dd.mm.yyyy",
                    fileURI: "https://localhost44303/files/solutionfiles/ЛР_1_Отчёт.docx",
                    isExpired: false
                }
            },
            {
                id: 2,
                name: "Заголовкин",
                surname: "Пупа",
                solution: {
                    contentText: "xxxxxxx",
                    creationDate: "dd.mm.yyyy",
                    fileURI: "https://localhost44303/files/solutionfiles/ЛР_1_Отчёт.docx",
                    isExpired: false
                }
            }
        ],
    },

    errorShow: false,
    errorMessage: [],
}

export default function teacherReducer(state = initialState, action) {
    switch (action.type) {
        case SUCCESS_TASK_ADDITION:
            return {
                ...state,
                teacherData: action.teacherData, 
                taskData: action.taskData
            }
        case ERROR_WINDOW:
            return {
                ...state, 
                errorShow: action.errorShow, 
                errorMessage: action.errorMessage
            }
        default:
            return state
    }
}
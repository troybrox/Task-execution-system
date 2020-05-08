import { 
    ERROR_WINDOW, 
    SUCCESS_TASK_ADDITION, 
    SUCCESS_MAIN, 
    SUCCESS_PROFILE, 
    SUCCESS_TASK, 
    SUCCESS_TASKS, 
    SUCCESS_CREATE, 
    SUCCESS_CREATE_DATA,
    LOADING_START } from "../actions/actionTypes"

const initialState = {
    profileData: [],
    mainData: [],
    taskData: {
        subjects: [],
        types: [],
    },
    tasks: [],
    createData: {
        subjects:[],
        types: [],
        groups: []
    },
    taskAdditionData: {
        teacherName: "Xxx",
        teacherSurname: "Xxx",
        teacherPatronymic: "Xxx",
        subject: "Моделирование",
        type: "Лабораторная работа",
        name: "№33",
        contentText: "xxxxxxx",
        fileURI: "https://localhost44303/files/taskFile/Math_Lab1_task.docx",
        group: "6315-020304D",
        beginDate: "11.12.2019",
        finishDate: "11.12.2020",
        updateDate: "dd.mm.yyyy",
        isOpen: true,
        timeBar: 12,
        students: [
            {
                id: 1,
                name: "Подзаголовкин",
                surname: "Лупа"
            },
            {
                id: 2,
                name: "Заголовкин",
                surname: "Пупа"
            }
        ],
        solutions: [
            {
                contentText: "xxxxxxx",
                creationDate: "dd.mm.yyyy",
                fileURI: "https://localhost44303/files/solutionfiles/ЛР_1_Отчёт.docx",
                isExpired: false,
                student: {
                    id: 1,
                    name: "Подзаголовкин",
                    surname: "Лупа"
                }
            },
            {
                contentText: "xxxxxxx",
                creationDate: "dd.mm.yyyy",
                fileURI: "https://localhost44303/files/solutionfiles/ЛР_1_Отчёт.docx",
                isExpired: false,
                student: {
                    id: 2,
                    name: "Заголовкин",
                    surname: "Пупа"
                }
            }
        ]
    },

    successId: null,
    errorShow: false,
    errorMessage: [],

    loading: false
}

export default function teacherReducer(state = initialState, action) {
    switch (action.type) {
        case LOADING_START:
            return {
                ...state, loading: true
            }
        case SUCCESS_PROFILE:
            return {
                ...state, profileData: action.profileData, loading: false
            }
        case SUCCESS_MAIN:
            return {
                ...state, mainData: action.mainData, loading: false
            }
        case SUCCESS_TASK:
            return {
                ...state, taskData: action.taskData, successId: null, loading: false
            }
        case SUCCESS_TASKS:
            return {
                ...state, tasks: action.tasks, loading: false
            }
        case SUCCESS_CREATE_DATA:
            return {
                ...state, createData: action.createData
            }
        case SUCCESS_CREATE:
            return {
                ...state, successId: action.successId
            }
        case SUCCESS_TASK_ADDITION:
            return {
                ...state, taskAdditionData: action.taskAdditionData, loading: false
            }
        case ERROR_WINDOW:
            return {
                ...state, 
                errorShow: action.errorShow, 
                errorMessage: action.errorMessage,
                loading: false
            }
        default:
            return state
    }
}
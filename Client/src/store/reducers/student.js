import { ERROR_WINDOW, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_TASKS, SUCCESS_TASK_ADDITION, LOADING_START } from "../actions/actionTypes"

const initialState = {
    profileData: [],
    taskData: {
        subjects: [],
        types: [],
    },
    tasks: [],
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

    errorShow: false,
    errorMessage: [],
    loading: false
}

export default function studentReducer(state = initialState, action) {
    switch (action.type) {
        case LOADING_START:
            return {
                ...state, loading: true
            }
        case SUCCESS_PROFILE:
            return {
                ...state, profileData: action.profileData, loading: false
            }
        case SUCCESS_TASK:
            return {
                ...state, taskData: action.taskData, loading: false
            }
        case SUCCESS_TASKS:
            return {
                ...state, tasks: action.tasks, loading: false
            }
        case SUCCESS_TASK_ADDITION:
            return {
                ...state, taskAdditionData: action.taskAdditionData, loading: false
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

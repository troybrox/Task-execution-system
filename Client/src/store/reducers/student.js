import { ERROR_WINDOW, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_TASKS, SUCCESS_TASK_ADDITION } from "../actions/actionTypes"

const initialState = {
    profileData: [
		{ value: 'pasha_terminator', label: 'Имя пользователя', type: 'text', serverName: 'userName', valid: true },
        { value: 'Александров', label: 'Фамилия', serverName: 'surname'},
        { value: 'Павел', label: 'Имя', serverName: 'name'},
        { value: 'Сидорович', label: 'Отчество', serverName: 'patronymic'},
        { value: 'Топовая 😎', label: 'Группа', serverName: 'groupNumber'},
		{ value: 'Кайфовый', label: 'Факультет', serverName: 'faculty'},
        { value: 'aaa@aa.aa', label: 'Адрес эл. почты', type: 'email', serverName: 'email', valid: true }
    ],
    taskData: {
        subjects: [
            {
                id: 1,
                name: 'Моделирование сложных систем',
                open: true
            },
            {
                id: 2,
                name: 'ЭВМ',
                open: false
            }
        ],
        types: [
            {id: null, name: 'Все'},
            {id: 1, name: 'Лабораторная работа'},
            {id: 2, name: 'Домашняя работа'},
        ],
    },
    tasks: [
        {type: 'Лабораторная работа', name: '№1', dateOpen: '2 дня'},
        {type: 'Лабораторная работа', name: '№2', dateOpen: '1 месяц'},
        {type: 'Лабораторная работа', name: '№3', dateOpen: '3 дня'},
    ],
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
}

export default function studentReducer(state = initialState, action) {
    switch (action.type) {
        case SUCCESS_PROFILE:
            return {
                ...state, profileData: action.profileData
            }
        case SUCCESS_TASK:
            return {
                ...state, taskData: action.taskData
            }
        case SUCCESS_TASKS:
            return {
                ...state, tasks: action.tasks
            }
        case SUCCESS_TASK_ADDITION:
            return {
                ...state, taskAdditionData: action.taskAdditionData
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

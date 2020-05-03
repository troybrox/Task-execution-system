import { ERROR_WINDOW, SUCCESS_TASK_ADDITION, SUCCESS_MAIN, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_LABS, SUCCESS_CREATE, SUCCESS_CREATE_DATA } from "../actions/actionTypes"

const initialState = {
    profileData: [
		{ value: 'pasha_terminator', label: 'Имя пользователя', type: 'text', serverName: 'UserName', valid: true },
        { value: 'Александров', label: 'Фамилия', serverName: 'Surname' },
        { value: 'Павел', label: 'Имя', serverName: 'Name' },
        { value: 'Карпович', label: 'Отчество', serverName: 'Patronymic'},
		{ value: 'Кайфовый', label: 'Факультет',serverName: 'Faculty'},
		{ value: 'Топовая 😎', label: 'Кафедра', serverName: 'Department'},
        { value: 'Доцент', label: 'Должность', type: 'text', serverName: 'Position', valid: true},
        { value: 'aaa@aa.aa', label: 'Адрес эл. почты', type: 'email', serverName: 'Email', valid: true }
    ],
    mainData: [
        {
            name: 'Моделирование сложных систем', 
            groups: [
                {
                    number: '6001-020304D', 
                    students: [
                        {
                            name: 'Студент 1', 
                            labs: [
                                {id: 1, name: 'Лабораторная работа №1', begin: '10.10.2020', end: '10.11.2020'},
                                {id: 2, name: 'Лабораторная работа №2', begin: '18.10.2020', end: ''}
                            ],
                            open: false, 
                        },
                        {
                            name: 'Студент 2', 
                            labs: [
                                {id: 3, name: 'Лабораторная работа №1', begin: '10.10.2020', end: ''},
                                {id: 4, name: 'Лабораторная работа №2', begin: '18.10.2020', end: '10.11.2020'}
                            ],
                            open: false, 
                        }
                    ],
                    open: true
                }, 
                {
                    number: '6002-020304D', 
                    students: [
                        {
                            name: 'Студент 3', 
                            labs: [
                                {id: 5, name: 'Лабораторная работа №1', begin: '10.10.2020', end: ''},
                                {id: 6, name: 'Лабораторная работа №2', begin: '18.10.2020', end: ''}
                            ],
                            open: false, 
                        },
                        {
                            name: 'Студент 4', 
                            labs: [
                                {id: 7, name: 'Лабораторная работа №1', begin: '10.10.2020', end: '10.11.2020'},
                                {id: 8, name: 'Лабораторная работа №2', begin: '18.10.2020', end: '10.11.2020'}
                            ],
                            open: false, 
                        }
                    ],
                    open: false
                }
            ], 
            open: true
        },
        {
            name: 'ЭВМ', 
            groups: [
                {number: '6005-020304D', open: false}, 
                {number: '6004-020304D', open: false}
            ], 
            open: false
        }
    ],
    taskData: {
        subjects: [
            {
                id: 1,
                name: 'Моделирование сложных систем', 
                groups: [
                    {id: 1, number: '6001-020304D', open: true}, 
                    {id: 2, number: '6002-020304D', open: false}
                ], 
                open: true
            },
            {
                id: 2,
                name: 'ЭВМ', 
                groups: [
                    {id: 3, number: '6005-020304D', open: false}, 
                    {id: 4, number: '6004-020304D', open: false}
                ], 
                open: false
            }
        ],
        types: [
            {id: null, name: 'Все'},
            {id: 1, name: 'Лабораторная работа'},
            {id: 2, name: 'Домашняя работа'},
        ],
    },
    labs: [
        {type: 'Лабораторная работа', name: '№1',  countAnswers: 3, dateOpen: '2 дня'},
        {type: 'Лабораторная работа', name: '№2',  countAnswers: 2, dateOpen: '1 месяц'},
        {type: 'Лабораторная работа', name: '№3',  countAnswers: 10, dateOpen: '3 дня'},
    ],
    createData: {
        subjects:[
            {
                id: null, 
                name: 'Выберите предмет',
            },
            {
                id: 1,
                name: "Алгебра"
            }
        ],
        types: [
            {
                id: null, 
                name: 'Выберите тип',
            },
            {
                id: 1,
                name: "Лабораторная работа"
            },
            {
                id: 2,
                name: "Самостоятельная работа"
            }
        ],
        groups: [
            {
                id: null,
                number: 'Все'
            },
            {
                id: 14,
                number: "6246-020304D",
                students: [
                    {
                        id: 1,
                        name: "Семён",
                        surname: "Александров",
                        patronymic: "Петрович",
                        groupId: 14,
                        check: false
                    },
                    {
                        id: 2,
                        name: "Семён",
                        surname: "Александров",
                        patronymic: "Петрович",
                        groupId: 14,
                        check: false
                    }
                ]
            }
        ]
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
}

export default function teacherReducer(state = initialState, action) {
    switch (action.type) {
        case SUCCESS_PROFILE:
            return {
                ...state, profileData: action.profileData
            }
        case SUCCESS_MAIN:
            return {
                ...state, mainData: action.mainData
            }
        case SUCCESS_TASK:
            return {
                ...state, taskData: action.taskData, successId: null
            }
        case SUCCESS_LABS:
            return {
                ...state, labs: action.labs
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
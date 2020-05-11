import { 
    ERROR_WINDOW, 
    SUCCESS_TASK_ADDITION, 
    SUCCESS_MAIN, 
    SUCCESS_PROFILE, 
    SUCCESS_TASK, 
    SUCCESS_TASKS, 
    SUCCESS_CREATE, 
    SUCCESS_CREATE_DATA,
    LOADING_START, 
    SUCCESS_CREATE_REPOSITORY,
    SUCCESS_REPOSITORY,
    SUCCESS_CREATE_REPOSITORY_END,
    SUCCESS_SUBJECT_FULL} from "../actions/actionTypes"

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
    taskAdditionData: {},
    createRepository: [],
    repositoryData: [],
    subjectFullData: [],

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
        case SUCCESS_CREATE_REPOSITORY:
            return {
                ...state, createRepository: action.createRepository, loading: false
            }
        case SUCCESS_REPOSITORY:
            return {
                ...state, repositoryData: action.repositoryData, repoActive: false, loading: false
            }
        case SUCCESS_SUBJECT_FULL:
            return {
                ...state, subjectFullData: action.subjectFullData, loading: false
            }
        case SUCCESS_CREATE_REPOSITORY_END:
            return {
                ...state, repoActive: true
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
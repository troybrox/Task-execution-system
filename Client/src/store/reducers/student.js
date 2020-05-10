import { ERROR_WINDOW, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_TASKS, SUCCESS_TASK_ADDITION, LOADING_START, SUCCESS_REPOSITORY } from "../actions/actionTypes"

const initialState = {
    profileData: [],
    taskData: {
        subjects: [],
        types: [],
    },
    tasks: [],
    taskAdditionData: {},
    repositoryData: [
        {id: 1, name: 'Алгебра', open: true},
        {id: 2, name: 'Информатика', open: false},
        {id: 3, name: 'Физика', open: false}
    ],

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
        case SUCCESS_REPOSITORY:
            return {
                ...state, repositoryData: action.repositoryData
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

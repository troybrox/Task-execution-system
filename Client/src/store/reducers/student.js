import { ERROR_WINDOW, SUCCESS_PROFILE, SUCCESS_TASK, SUCCESS_TASKS, SUCCESS_TASK_ADDITION, LOADING_START, SUCCESS_REPOSITORY, SUCCESS_SUBJECT_FULL, LOGOUT, GOOD_NEWS } from "../actions/actionTypes"

const initialState = {
    profileData: [],
    taskData: {
        subjects: [],
        types: [],
    },
    tasks: [],
    taskAdditionData: {},
    repositoryData: [],
    subjectFullData: [],

    errorShow: false,
    errorMessage: [],
    goodNews: false,
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
                ...state, profileData: action.profileData, loading: false, taskAdditionData: {}
            }
        case SUCCESS_TASK:
            return {
                ...state, taskData: action.taskData, loading: false, taskAdditionData: {}
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
                ...state, repositoryData: action.repositoryData, loading: false, taskAdditionData: {}
            }
        case SUCCESS_SUBJECT_FULL:
            return {
                ...state, subjectFullData: action.subjectFullData, loading: false, taskAdditionData: {}
            }
        case ERROR_WINDOW:
            return {
                ...state,
                errorShow: action.errorShow,
                errorMessage: action.errorMessage
            }
        case GOOD_NEWS:
            return {
                ...state, goodNews: true
            }
        case LOGOUT:
            return {
                ...state,
                profileData: [],
                taskData: {
                    subjects: [],
                    types: [],
                },
                tasks: [],
                taskAdditionData: {},
                repositoryData: [],
                subjectFullData: [],
            
                errorShow: false,
                errorMessage: [],
                goodNews: false,
                loading: false
            }
        default:
            return state
    }
}

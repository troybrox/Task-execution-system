import { 
    AUTH_SUCCESS, 
    LOGOUT, 
    SUCCESS, 
    PUSH_FILTERS, 
    ERROR_WINDOW,
    LOADING_START} from "../actions/actionTypes"

const initialState = {
    faculties: [],
	groups: [],
	departments: [],    
    
    loading: false,

    successPage: false,

    catchError: false,
    catchErrorMessage: [],

    role: localStorage.getItem('role') || null
}

export default function authReducer(state = initialState, action) {
    switch (action.type) {
        case AUTH_SUCCESS:
            return {
                ...state, role: action.role, loading: false
            }
        case LOGOUT:
            return {
                ...state, role: null, loading: false
            }
        case SUCCESS:
            return {
                ...state, successPage: action.successPage, loading: false
            }
        case PUSH_FILTERS:
            return {
                ...state, faculties: action.faculties, groups: action.groups, departments: action.departments,
            }
        case ERROR_WINDOW:
            return {
                ...state, catchError: action.catchError, catchErrorMessage: action.catchErrorMessage, loading: false
            }
        case LOADING_START:
            return {
                ...state, loading: true
            }
        default:
            return state
    }
}
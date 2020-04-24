import { 
    AUTH_SUCCESS, 
    LOGOUT, 
    SUCCESS, 
    // ERROR_MESSAGE_AUTH, 
    PUSH_FILTERS, 
    ERROR_WINDOW,
    LOADING_START} from "../actions/actionTypes"

const initialState = {
    faculties: [
        {id: null, name: 'Выберите факультет'},
        {id: 1, name: 'Факультет 1'},
        {id: 2, name: 'Факультет 2'}
    ],
	groups: [
        {id: null, name: 'Выберите группу'},
        {id: 1, name: 'Группа 1', facultyId: 1},
        {id: 2, name: 'Группа 2', facultyId: 2}
    ],
	departments: [
        {id: null, name: 'Выберите кафедру'},
        {id: 1, name: 'Кафедра 1', facultyId: 1},
        {id: 2, name: 'Кафедра 2', facultyId: 2}
    ],    
    
    loading: false,

    successPage: false,

    // errorMessages: null,

    catchError: false,
    catchErrorMessage: [],

    token: localStorage.getItem('token') || '',
    role: localStorage.getItem('role') || null
}

export default function authReadducer(state = initialState, action) {
    switch (action.type) {
        case AUTH_SUCCESS:
            return {
                ...state, token: action.token, role: action.role, loading: false
            }
        case LOGOUT:
            return {
                ...state, token: null, role: null
            }
        case SUCCESS:
            return {
                ...state, successPage: action.successPage, loading: false
            }
        // case ERROR_MESSAGE_AUTH:
        //     return {
        //         ...state, errorMessages: action.errorMessages
        //     }
        case PUSH_FILTERS:
            return {
                ...state, faculties: action.faculties, groups: action.groups, departments: action.departments,
            }
        case ERROR_WINDOW:
            return {
                ...state, catchError: action.catchError, catchErrorMessage: action.catchErrorMessage
            }
        case LOADING_START:
            return {
                ...state, loading: true
            }
        default:
            return state
    }
}
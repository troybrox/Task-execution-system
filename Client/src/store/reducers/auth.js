import { 
    AUTH_SUCCESS, 
    LOGOUT, 
    SUCCESS, 
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

    catchError: false,
    catchErrorMessage: [],

    token: localStorage.getItem('token') || '',
    role: localStorage.getItem('role') || null
}

export default function authReducer(state = initialState, action) {
    switch (action.type) {
        case AUTH_SUCCESS:
            return {
                ...state, token: action.token, role: action.role, loading: false
            }
        case LOGOUT:
            return {
                ...state, token: '', role: null, loading: false
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
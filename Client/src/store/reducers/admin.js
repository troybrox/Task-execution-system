import { 
    LOADING_START, 
    PUSH_USERS, 
    PUSH_SELECTS, 
    ERROR_WINDOW, 
    CHANGE_CONDITION } from "../actions/actionTypes"

const initialState = {
    users: [],
    selects: [
        {
            title: 'Факультет', 
            options: [{id: null, name: 'Все'}], 
            show: true
        },
        {
            title: 'Кафедра',  
            options: [{id: null, name: 'Все'}], 
            show: true
        },
        {
            title: 'Группа',  
            options: [{id: null, name: 'Все'}], 
            show: false
        }
    ],
    errorShow: false,
    errorMessage: [],

    loading: false,
    actionCondition: null
}

export default function adminReducer(state = initialState, action) {
    switch (action.type) {
        case LOADING_START:
            return {
                ...state, loading: true
            }
        case PUSH_USERS:
            return {
                ...state, users: action.users, loading: false, 
                actionCondition: null
            }
        case PUSH_SELECTS:
            return {
                ...state, selects: action.selects, loading: false
            }
        case ERROR_WINDOW:
            return {
                ...state, 
                errorShow: action.errorShow, 
                errorMessage: action.errorMessage, 
                loading: false, 
                actionCondition: null
            }
        case CHANGE_CONDITION:
            return {
                ...state, actionCondition: action.actionCondition
            }
        default:
            return state
    }
}
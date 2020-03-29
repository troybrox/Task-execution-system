import { AUTH_SUCCESS, LOGOUT } from "../actions/actionTypes"

const initialState = {
    token: null,
    role: 'admin'
}

export default function authReducer(state = initialState, action) {
    switch (action.type) {
        case AUTH_SUCCESS:
            return {
                ...state, token: action.token, role: action.role
            }
        case LOGOUT:
            return {
                ...state, token: null, role: null
            }
        default:
            return state
    }
}
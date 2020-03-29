import axios from 'axios'
import { AUTH_SUCCESS, LOGOUT } from './actionTypes'

export function auth(data) {
    return async dispatch => {
        const url = 'https://localhost:44303/api/account/login'
        
        const response = await axios.post(url, data)
        const respData = response.data
        // localStorage.setItem('token'), respData.idToken)
        // localStorage.setItem('userId'), respData.userId)
        // localStorage.setItem('role'), respData.role)
        
        dispatch(authSuccess(respData.idToken, respData.role))
    }
}

export function authSuccess(token, role) {
    return {
        type: AUTH_SUCCESS,
        token, role
    }
}

export function logout() {
    // localStorage.removeItem('token'), respData.idToken)
    // localStorage.removeItem('userId'), respData.userId)
    // localStorage.removeItem('role'), respData.role)
    return {
        type: LOGOUT
    }
}

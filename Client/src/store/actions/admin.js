import axios from 'axios'
import { PUSH_USERS, PUSH_SELECTS } from './actionTypes'

export function changeCheckedHandler(index) {
    return (dispatch, getState) => {
        const state = getState().admin
        const users = state.users
        users[index].check = !users[index].check
        
        dispatch(pushUsers(users))
    }
}

export function loadingUsers(url) {
    return async dispatch => {
        try {
            const response = await axios.get(url)
            const users = response.data.users

            dispatch(pushUsers(users))
        } catch (e) {
            console.log(e)
        }
    }
}

export function loadingLists(url) {
    return async dispatch => {
        try {
            const response = await axios.get(url)
            const selects = response.data.selects

            dispatch(pushLists(selects))
        } catch (e) {
            console.log(e)
        }
    }
}

export function searchUsers(search) {
    return (dispatch, getState) => {
        const state = getState().admin
        const users = [...state.users]
        users.forEach(item => {
            if (item.name.indexOf(search) > -1) item.show = true 
            else item.show = false
        })
        dispatch(pushUsers(users))
    }
}

export function pushUsers(users) {
    return {
        type: PUSH_USERS,
        users
    }
}

export function pushLists(selects) {
    return {
        type: PUSH_SELECTS,
        selects
    }
}
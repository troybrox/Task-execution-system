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

export function loadingUsers(url, idFaculty, idGroup, idDepartment, search) {
    return async dispatch => {
        try {
            const data = {
                idGroup,
                idFaculty,
                idDepartment,
                search
            }
            const response = await axios.post(url, data)
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

            // преобразовать в нужный объект

            // selects: {
                // '': {id: 0, groups: [{id: 0, name: ''}, {id: 0, name: ''}], departments: [{id: 0, name: ''}, {id: 0, name: ''}]}
                // '': {id: 1, groups: [{id: 0, name: ''}, {id: 0, name: ''}], departments: [{id: 0, name: ''}, {id: 0, name: ''}]}
            // }

            dispatch(pushLists(selects))
        } catch (e) {
            console.log(e)
        }
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
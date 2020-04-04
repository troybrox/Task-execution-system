// import axios from 'axios'
import { CHANGE_CHECKED } from './actionTypes'

export function changeCheckedHandler(index) {
    return (dispatch, getState) => {
        const state = getState().admin
        const users = [...state.users]
        users[index].check = !users[index].check
        
        dispatch(changeChecked(users))
    }
}

export function changeChecked(users) {
    return {
        type: CHANGE_CHECKED,
        users
    }
}

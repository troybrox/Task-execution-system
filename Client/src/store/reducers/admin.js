import { CHANGE_CHECKED } from "../actions/actionTypes"

const initialState = {
    users: [
        {name: 'Студент 1', check: false},
        {name: 'Студент 2', check: false},
        {name: 'Студент 3', check: true},
        {name: 'Студент 4', check: false}
    ]
}

export default function adminReducer(state = initialState, action) {
    switch (action.type) {
        case CHANGE_CHECKED:
            return {
                ...state, users: action.users
            }
        default:
            return state
    }
}
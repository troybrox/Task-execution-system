import {combineReducers} from 'redux'
import authReducer from './auth'
import adminReducer from './admin'

export default combineReducers({
    auth: authReducer,
    admin: adminReducer
})
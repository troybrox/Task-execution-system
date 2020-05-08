import {combineReducers} from 'redux'
import authReducer from './auth'
import adminReducer from './admin'
import teacherReducer from './teacher'
import studentReducer from './student'

export default combineReducers({
    auth: authReducer,
    admin: adminReducer,
    teacher: teacherReducer,
    student: studentReducer
})
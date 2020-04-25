import {combineReducers} from 'redux'
import authReducer from './auth'
import adminReducer from './admin'
import commonReducer from './common'
import teacherReducer from './teacher'
import studentReducer from './student'

export default combineReducers({
    auth: authReducer,
    admin: adminReducer,
    common: commonReducer,
    teacher: teacherReducer,
    student: studentReducer
})
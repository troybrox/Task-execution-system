import React from 'react'
import { connect } from 'react-redux'
import { Redirect } from 'react-router-dom'
import { logoutHandler } from '../../store/actions/auth'
import { logoutTeacher } from '../../store/actions/teacher'
import { logoutStudent } from '../../store/actions/student'
import { logoutAdmin } from '../../store/actions/admin'

// Компонент переадресации на выход
class Logout extends React.Component {
    componentDidMount() {
        this.props.logoutHandler()
        this.props.logoutTeacher()
        this.props.logoutStudent()
        this.props.logoutAdmin()
    }
    
    render() {
        return(
            <Redirect to={'/auth'} />
        )
    }
}

function mapDispatchToProps(dispatch) {
    return {
        logoutHandler: () => dispatch(logoutHandler()),
        logoutTeacher: () => dispatch(logoutTeacher()),
        logoutStudent: () => dispatch(logoutStudent()),
        logoutAdmin: () => dispatch(logoutAdmin())
    }
}

export default connect(null, mapDispatchToProps)(Logout)
import React from 'react'
import { connect } from 'react-redux'
import { Redirect } from 'react-router-dom'
import { logoutHandler } from '../../store/actions/auth'

class Logout extends React.Component {
    componentDidMount() {
        this.props.logoutHandler()
    }
    
    render() {
        return(
            <Redirect to={'/auth'} />
        )
    }
}

function mapDispatchToProps(dispatch) {
    return {
        logoutHandler: () => dispatch(logoutHandler())
    }
}

export default connect(null, mapDispatchToProps)(Logout)
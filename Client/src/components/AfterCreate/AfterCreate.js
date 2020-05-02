import React from 'react'
import { Redirect } from 'react-router-dom'

class AfterCreate extends React.Component {
    render() {
        return(
            <Redirect to={`/tasks/${this.props.id}`} />
        )
    }
}


export default AfterCreate
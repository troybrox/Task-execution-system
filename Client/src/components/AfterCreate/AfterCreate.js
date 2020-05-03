import React from 'react'
import { Redirect } from 'react-router-dom'

// Компонент переадресации на страницу задачи после ее создания преподавателем
class AfterCreate extends React.Component {
    render() {
        return(
            <Redirect to={`/tasks/${this.props.id}`} />
        )
    }
}


export default AfterCreate
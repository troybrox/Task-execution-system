import React from 'react'
import './Condition.scss'

// Компонент отображения сообщения успеха при добавлении/удалении пользователя админом
class Condition extends React.Component {  
    render() {
        let title = 'Обработка'
        let message = 'Подождите...'
        const cls = ['condition_window']
        if (this.props.actionCondition === 'ready') {
            title = 'Успешно'
            message = 'Действие прошло успешно!'
        }

        return (
            <div className={cls.join(' ')}>
                <h2>{title}</h2>
                <div>
                    {message}
                </div>
            </div>
        )
    }
}

export default Condition
import React from 'react'
import './Error.scss'
import Button from '../UI/Button/Button'

// Компонент отображения ошибки
class Error extends React.Component {
    renderMessage() {
        return this.props.errorMessage.map((item, index) => {
            return <p key={index}>{item}</p>
        })
    }

    render() {
        const cls = this.props.goodNews ? 'good' : 'bad'
        return (
            <div className='error_field'>
                <div className='error_window'>
                    <h2 className={cls}>{!this.props.goodNews ? 'Ошибка' : 'Пароль успешно изменен!'}</h2>
                    <div>
                        {this.renderMessage()}
                    </div>
                    {!this.props.goodNews ? 
                        <Button 
                            typeButton='grey'
                            onClickButton={this.props.errorWindow}
                            value='Закрыть'
                        /> : 
                        null
                    }
                </div>
            </div>
        )
    }
}

export default Error
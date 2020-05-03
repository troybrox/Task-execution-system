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
        return (
            <div className='error_field'>
                <div className='error_window'>
                    <h2>Ошибка</h2>
                    <div>
                        {this.renderMessage()}
                    </div>
                    <Button 
                        typeButton='grey'
                        onClickButton={this.props.errorWindow}
                        value='Закрыть'
                    />
                </div>
            </div>
        )
    }
}

export default Error
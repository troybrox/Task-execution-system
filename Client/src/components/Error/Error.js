import React from 'react'
import './Error.scss'

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
                    <button onClick={this.props.errorWindow}>Закрыть</button>
                </div>
            </div>
        )
    }
}

export default Error
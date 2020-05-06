import React from 'react'
import './Answer.scss'

// Компонент отображения ответов на странице задачи
class Answer extends React.Component {
    render() {
        return (
            <div className='answer_block'>
                <img src={`/images/${this.props.source}`} alt='' />
                <div className='text_block'>
                    <h4>
                        {this.props.data.student.surname} {this.props.data.student.name}
                        {' '} написал {this.props.data.creationDate}    
                    </h4>
                    <p>
                        {this.props.data.contentText}
                    </p>
                    <p>
                        <a href={this.props.data.fileURI}>Скачать файл</a>
                    </p>
                </div>
            </div>
        )
    }
}

export default Answer
import React from 'react'
import './Answer.scss'
import Button from '../Button/Button'

// Компонент отображения ответов на странице задачи
class Answer extends React.Component {
    render() {
        return (
            <div className='answer_block'>
                <img src={`/images/${this.props.source}`} className='author' alt='' />
                <div className='text_block'>
                    <h4>
                        {this.props.data.studentSurname} {this.props.data.studentName + ' '}
                        {this.props.role === 'teacher' ? 'создал' : 'решил'} {this.props.data.creationDate}    
                    </h4>
                    <p className='content_text_answer'>
                        {this.props.data.contentText}
                    </p>
                    { 
                        this.props.data.fileURI !== null ?
                            <p>
                                <a href={this.props.data.fileURI} target='_blank' rel='noopener noreferrer' download={this.props.data.fileName}>
                                    <Button 
                                        typeButton='download'
                                        value={this.props.data.fileName}
                                    />
                                </a>
                            </p> : 
                            null
                    }
                </div>
            </div>
        )
    }
}

export default Answer
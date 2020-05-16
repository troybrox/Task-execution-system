import React from 'react'
import './RepositoryComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import { Link } from 'react-router-dom'
import Button from '../../UI/Button/Button'
import Loader from '../../UI/Loader/Loader'

// Компонент отображения репозиториев для препода и студента
class RepositoryComponent extends React.Component {
    state = {
        text: '',
        editText: '',
        title: '',
        editTitle: '',
        edit: false,
        activeSubjectIndex: null,
        activeRepoIndex: null
    }

    editRepository = () => {
        const edit = !this.state.edit
        const editText = this.state.text
        const editTitle = this.state.title
        

        this.setState({
            edit,
            editText,
            editTitle
        })
    }

    onChoiceSubject = async (index) => {
        await this.props.choiceSubject(index)
        this.setState({
            activeSubjectIndex: index  
        })
    }

    onChoiceRepo = (item, index) => {
        if (this.state.activeRepoIndex === index || (this.state.activeRepoIndex !== index && !item.open)) this.props.choiceRepo(index)
        const text = item.contentText
        const title = item.name
        this.setState({
            activeRepoIndex: index,
            text,
            title,
            edit: false
        })
    }

    onLoadFile = event => {
        const file = event.target.files[0]
        const filters = new FormData()
        filters.append('repoId', this.props.subjectFullData[this.state.activeRepoIndex].id)
        filters.append('file', file)
        this.props.sendCreateRepositoryFile(filters)
    }

    editRepoHandler = index => {
        const edit = !this.state.edit
        const text = this.state.editText
        let editTitle = this.state.editTitle
        let title = this.state.title
        if (this.state.editTitle.trim() === '')
            editTitle = title
        else 
            title = editTitle

        let success = window.confirm('Подтвердите ваше действие!');
        if (success) {
            this.props.editRepo(index, text, title)
        

            this.setState({
                edit,
                text,
                title,
                editTitle
            })
        }
    }

    deleteRepoHandler = index => {
        let success = window.confirm('Подтвердите ваше действие!');
        if (success) {
            this.props.deleteRepo(index)
            this.setState({
                activeRepoIndex: null
            })
        }
    }

    renderFileList(files) {
        return files.map(item => {
            return (
                <li key={item.fileURI} className='download_small'>
                    <a href={item.fileURI} target='_blank' rel='noopener noreferrer' download={item.fileName}>
                        <img src='/images/download-solid.svg' alt='' />
                        {item.fileName} 
                    </a>
                </li>
            )
        })
    }

    renderMiniList(subjectId) {
        return this.props.subjectFullData.length !== 0 ? 
            this.props.subjectFullData.map((item, index) => {
                if (item.subjectId === subjectId) {
                    const cls = ['small_items']
                    if (this.state.activeRepoIndex === index) cls.push('active_small')
                    return (
                        <Auxiliary key={index}>
                            <li 
                                className={cls.join(' ')}
                                onClick={this.onChoiceRepo.bind(this, item, index)}
                            >
                                <img src='images/folder-regular.svg' alt='' />
                                {item.name}
                            </li>
                            {item.open && item.files.length !== 0 ? 
                                <ul className='small_list'>
                                    {this.renderFileList(item.files)}
                                </ul> :
                                null
                            }
                        </Auxiliary>

                    )
                } else 
                    return null
            }) :
            null
    }

    renderList() {
        const list = this.props.repositoryData.length === 0 ? 
            localStorage.getItem('role') === 'teacher' ?
                <p className='empty_field'>
                    <Link to='/create_repository'>Создайте репозиторий</Link>,
                    чтобы видеть предметы по созданным репозиториям
                </p> : 
                <p className='empty_field'>
                    Здесь будет список предметов ваших задач, пока задач нет
                </p> :
            this.props.repositoryData.map((item, index) => {
                const cls = ['big_items']
                let src = 'images/angle-right-solid.svg'
                if (item.open) {
                    src = 'images/angle-down-solid.svg'

                }
                return (
                    <Auxiliary key={index}>
                        <li 
                            className={cls.join(' ')}
                            onClick={this.onChoiceSubject.bind(this, index)}
                        >
                            {<img src={src} alt='' />}
                            {item.name}
                        </li>

                        {item.open ? 
                            <ul className='small_list'>
                                {this.props.loading ? <Loader /> : this.renderMiniList(item.id)}
                            </ul> : null
                        }
                    </Auxiliary>
                )
        })

        return (
            <ul className='big_list'>{list}</ul>
        )
    }

    renderButtonsLook() {
        if (localStorage.getItem('role') === 'teacher') {
            return (
                <div className='buttons'>
                    <Button 
                        typeButton='blue'
                        value='Удалить'
                        onClickButton={() => this.deleteRepoHandler(this.state.activeRepoIndex)}
                    />
                    <Button 
                        typeButton='blue'
                        value='Изменить'
                        onClickButton={this.editRepository}
                    />
                    <Button 
                        typeButton='download'
                        value='Добавить файл'
                    >
                        <input 
                            type='file' 
                            accept='application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.openxmlformats-officedocument.wordprocessingml.template,application/pdf,image/jpeg,image/pjpeg' 
                            onChange={event => this.onLoadFile(event)}
                        />
                    </Button>
    

                </div>
            )
        }
    }

    renderButtonsEdit() {
        return (
            <div className='buttons'>
                <Button 
                    typeButton='blue'
                    value='Изменить'
                    onClickButton={() => this.editRepoHandler(this.state.activeRepoIndex)}
                />
                <Button 
                    typeButton='grey'
                    value='Отмена'
                    onClickButton={this.editRepository}
                />
            </div>
        )
    }

    changeRepositoryText = target => {
        this.setState({
            editText: target.value
        })
    }

    changeRepositoryTitle = target => {
        this.setState({
            editTitle: target.value
        })
    }

    render() {
        const teacherLook = (
            <Auxiliary>
                <p className='text_topic'>
                        {this.state.text}
                </p>
                {this.renderButtonsLook()}
            </Auxiliary>
        )

        const teacherEdit =(
            <Auxiliary>
                <textarea 
                    className='text_topic_edit' 
                    defaultValue={this.state.editText} 
                    onChange={event => this.changeRepositoryText(event.target)}
                />
                {this.renderButtonsEdit()}
            </Auxiliary>
        )

        const name = ( 
            this.props.subjectFullData.length !== 0 && this.state.activeRepoIndex !== null && !this.state.edit ?
                <span>{this.state.title}</span> :
                
                <input type='text' value={this.state.editTitle} onChange={event => this.changeRepositoryTitle(event.target)} />
        )

        const main = (
            <div className='topic'>
                { this.props.subjectFullData.length !== 0 && this.state.activeRepoIndex !== null && this.props.subjectFullData[this.state.activeRepoIndex] !== undefined ?
                    <div className='repo_title'>
                        <h2>{this.props.subjectFullData[this.state.activeRepoIndex].subject}. {name}</h2> 
                        { localStorage.getItem('role') === 'student' ? 
                            <p className='author'>Автор: {this.props.subjectFullData[this.state.activeRepoIndex].teacherSurname} {this.props.subjectFullData[this.state.activeRepoIndex].teacherName} {this.props.subjectFullData[this.state.activeRepoIndex].teacherPatronymic}</p> : 
                            null
                        } 
                    </div>:
                    null
                }

                {!this.state.edit ? teacherLook : teacherEdit}
            </div>
        )

        return (
            <Frame active_index={3}>
                <div className='main_subject'>
                    <aside className='aside_subject'>
                        { this.props.repositoryData.length !== 0 && localStorage.getItem('role') === 'teacher' ?
                            <div className='create_repo_button'>
                                <Link 
                                    to='/create_repository'
                                >
                                    <Button 
                                        typeButton='blue'
                                        value='Создать репозиторий'
                                    />
                                </Link>
                            </div> : 
                            null    
                        }
                        {this.props.loading && this.props.repositoryData.length === 0? <Loader /> : this.renderList()}
                    </aside>
                    { this.state.activeRepoIndex !== null ? main : null}

                </div>
            </Frame>
        )
    }
}

export default RepositoryComponent
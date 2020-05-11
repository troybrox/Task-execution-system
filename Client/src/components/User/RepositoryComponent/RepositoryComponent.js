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
        edit: false,

        activeSubjectIndex: null,
        activeRepoIndex: null
    }

    editRepository = () => {
        const edit = !this.state.edit
        const editText = this.state.text
        

        this.setState({
            edit,
            editText
        })
    }

    onChoiceSubject = async (index) => {
        await this.props.choiceSubject(index)
        this.setState({
            activeSubjectIndex: index  
        })
    }

    choiceRepo = (item, index) => {
        const text = item.contentText
        this.setState({
            activeRepoIndex: index,
            text,
            edit: false
        })
    }

    renderFileList(files) {
        return files.map(item => {
            return (
                <li className='download_small'>
                    <a href={item.fileURI} download={item.fileName}>
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
                        <Auxiliary>
                            <li 
                                key={index}
                                className={cls.join(' ')}
                                onClick={this.choiceRepo.bind(this, item, index)}
                            >
                                <img src='images/folder-regular.svg' alt='' />
                                {item.name}
                            </li>
                            {this.state.activeRepoIndex === index && item.files.length !== 0 ? 
                                <ul className='small_list'>
                                    {this.renderFileList(item.files)}
                                </ul> : null
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
                        onClickButton={() => this.props.deleteRepo(this.state.activeRepoIndex)}
                    />
                    <Button 
                        typeButton='blue'
                        value='Изменить'
                        onClickButton={this.editRepository}
                    />
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
                    onClickButton={() => this.props.editRepo(this.state.activeRepoIndex)}
                />
                <Button 
                    typeButton='grey'
                    value='Отмена'
                    onClickButton={this.editRepository}
                />
            </div>
        )
    }

    changeRepository = target => {
        // const editText = this.state.editText
        // editText = target.value
        this.setState({
            editText: target.value
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
                    value={this.state.editText} 
                    onChange={event => this.changeRepository(event.target)}
                />
                {this.renderButtonsEdit()}
            </Auxiliary>
        )

        const main = (
            <div className='topic'>
                { this.props.subjectFullData.length !== 0 && this.state.activeRepoIndex !== null ?
                    <div className='repo_title'>
                        <h2>{this.props.subjectFullData[this.state.activeRepoIndex].subject}. {this.props.subjectFullData[this.state.activeRepoIndex].name}</h2> 
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
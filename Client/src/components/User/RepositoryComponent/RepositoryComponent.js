import React from 'react'
import './RepositoryComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import { Link } from 'react-router-dom'
import Button from '../../UI/Button/Button'

// Компонент отображения репозиториев для препода и студента
class RepositoryComponent extends React.Component {
    state = {
        active: false,
        smallIndex: null,
        text: [],
        edit: false,
    }

    editRepository = () => {
        const edit = !this.state.edit

        this.setState({
            edit
        })
    }

    choiceGroup = (item, index) => {
        const topicText = this.props.topicText
        const text = [item, topicText[item]]
        this.setState({
            smallIndex: index,
            text,
            active: true,
            edit: false
        })
    }

    // renderMiniList() {
    //     return this.props.someData.map((item, index) => {
    //         const cls = ['small_items']
    //         if (this.state.smallIndex === index) cls.push('active_small')
    //         return (
    //             <li 
    //                 key={index}
    //                 className={cls.join(' ')}
    //                 onClick={this.choiceGroup.bind(this, item, index)}
    //             >
    //                 <img src='images/folder-regular.svg' alt='' />
    //                 {item}
    //             </li>
    //         )
    //     })
    // }

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
                            onClick={this.props.choiceSubject.bind(this, index)}
                        >
                            {<img src={src} alt='' />}
                            {item.name}
                        </li>

                        {/* {item.open ? 
                            <ul className='small_list'>
                                {this.renderMiniList()}
                            </ul> : null
                        } */}
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
                    <button>Удалить</button>
                    <button onClick={this.editRepository}>Изменить</button>
                </div>
            )
        }
    }

    renderButtonsEdit() {
        return (
            <div className='buttons'>
                <button>Изменить</button>
                <button className='cancel' onClick={this.editRepository}>Отмена</button>
            </div>
        )
    }

    changeRepository = target => {
        const text = this.state.text
        text[1] = target.value
        this.setState({
            text
        })
    }

    render() {
        const teacherLook = (
            <Auxiliary>
                <p className='text_topic'>
                        {this.state.text[1]}
                </p>
                {this.renderButtonsLook()}
            </Auxiliary>
        )

        const teacherEdit =(
            <Auxiliary>
                <textarea 
                    className='text_topic_edit' 
                    value={this.state.text[1]} 
                    onChange={event => this.changeRepository(event.target)}
                />
                {this.renderButtonsEdit()}
            </Auxiliary>
        )

        const main = (
            <div className='topic'>
                <div className='search'>
                    <input type='search' placeholder='Поиск...' />
                    <button 
                        // onClick={this.searchHandler}
                    >
                        Поиск
                    </button>
                </div>

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
                        {this.renderList()}
                    </aside>
                    { this.state.active ? main : null}

                </div>
            </Frame>
        )
    }
}

export default RepositoryComponent
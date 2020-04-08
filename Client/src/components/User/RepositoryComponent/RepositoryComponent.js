import React from 'react'
import './RepositoryComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'

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

    renderMiniList(topics) {
        return topics.map((item, index) => {
            const cls = ['small_items']
            if (this.state.smallIndex === index) cls.push('active_small')
            return (
                <li 
                    key={index}
                    className={cls.join(' ')}
                    onClick={this.choiceGroup.bind(this, item, index)}
                >
                    <img src='images/folder-regular.svg' alt='' />
                    {item}
                </li>
            )
        })
    }

    renderList() {
        const list = this.props.subjects.map((item, index) => {
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
                        {item.value}
                    </li>

                    {item.open ? 
                        <ul className='small_list'>
                            {this.renderMiniList(item.topics)}
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
                        {this.renderList()}
                    </aside>
                    { this.state.active ? main : null}

                </div>
            </Frame>
        )
    }
}

export default RepositoryComponent
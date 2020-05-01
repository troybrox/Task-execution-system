import React from 'react'
import './TasksComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Button from '../../UI/Button/Button'
import { Link } from 'react-router-dom'
import Select from '../../UI/Select/Select'

class TasksComponent extends React.Component {
    state = {
        activeSubjectIndex: null,
        activeGroupIndex: null,
        title: '',
        search: ''
    }

    choiceSubjectTeacher = indexSubject => {
        this.props.choiceSubjectTask(indexSubject)
        this.setState({})
    }

    choiceSubjectStudent = (indexSubject, title) => {
        this.props.choiceSubjectTask(indexSubject)

        this.setState({
            activeSubjectIndex: indexSubject,
            title
        })
    }

    choiceGroup = (indexSubject, indexGroup) => {
        this.props.choiceGroupTask(indexSubject, indexGroup)

        const nameSubject = this.props.subjects[indexSubject].name
        const nameGroup = this.props.subjects[indexSubject].groups[indexGroup].number

        const title = nameSubject + '. Группа ' + nameGroup
        
        this.setState({
            activeSubjectIndex: this.props.subjects[indexSubject].id,
            activeGroupIndex: this.props.subjects[indexSubject].groups[indexGroup].id,
            title
        })
    }

    renderList() {
        if (localStorage.getItem('role') === 'teacher') {
            return this.renderListTeacher()
        } else {
            return this.renderListStudent()
        }
    }

    renderListTeacher() {
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
                        onClick={() => this.choiceSubjectTeacher(index)}
                    >
                        {<img src={src} alt='' />}
                        {item.name}
                    </li>

                    {item.open && 'groups' in item ? 
                        <ul className='small_list'>
                            {this.renderMiniList(item.groups, index)}
                        </ul> : null
                    }
                </Auxiliary>
            )
        })

        return (
            <ul className='big_list'>{list}</ul>
        )
    }

    renderMiniList(groups, indexSubject) {
        return groups.map((item, index) => {
            const cls = ['small_items']
            if (item.open) cls.push('active_small')
            return (
                <li 
                    key={index}
                    className={cls.join(' ')}
                    onClick={this.choiceGroup.bind(this, indexSubject, index)}
                >
                    <img src='images/folder-regular.svg' alt='' />
                    {item.number}
                </li>
            )
        })
    }

    renderListStudent() {
        const list = this.props.subjects.map((item) => {
            const cls = ['big_items']
            let src = 'images/folder-regular.svg'
            if (item.open) {
                cls.push('active_big')
            }
            return (
                <Auxiliary key={item.id}>
                    <li 
                        className={cls.join(' ')}
                        onClick={() => this.choiceSubjectStudent(item.id, item.name)}
                    >
                        {<img src={src} alt='' />}
                        {item.name}
                    </li>
                </Auxiliary>
            )
        })

        return (
            <ul className='big_list'>{list}</ul>
        )
    }

    renderLabs() {
        const subject = this.state.title.split(' ')
        return this.props.labs.map((item, index) => {
            return (
                    <Link
                        to={`/tasks/${index}`}
                        key={index}
                        className='each_labs' 
                    >
                        <div className='labs_left'>
                            <span className='subject_for_lab'>{subject[0]}</span>
                            <span>{item.type} {item.name}</span><br />
                            <span className='small_text'>Открыта {item.dateOpen} назад</span>
                        </div>
                        { localStorage.getItem('role') === 'teacher' ?
                            <div className='labs_right'>
                                <img src='images/comment-regular.svg' alt='' />
                                <span>{item.countAnswers}</span>
                            </div> :
                            null
                        }
                    </Link>
            )
        })
    }

    renderOptions() {
        return this.props.types.map((item) => {
            return (
                <option
                    key={item.id}
                >
                    {item.name}
                </option>
            )
        })
    }

    onChangeSelect = event => {
        const index = event.target.options.selectedIndex
        const typeId = event.target.options[index].getAttribute('index')
        const filters = {
            subjectId: String(this.state.activeSubjectIndex), 
            groupId: String(this.state.activeGroupIndex),
            typeId
        }

        this.props.fetchListTasks(filters)
    }

    onChangeSearch = event => {
        const search = event.target.value

        this.setState({
            search
        })
    }

    onSearchHandler = () => {
        if (this.state.search.trim() !== '') {
            const filters = {
                subjectId: String(this.state.activeSubjectIndex), 
                groupId: String(this.state.activeGroupIndex),
                searchString: this.state.search
            }
            this.props.fetchListTasks(filters)
        }
    }
    
    render() {
        const main = (
            <div className='labs_group'>               
                <div className='search'>
                    <input 
                        type='search' 
                        placeholder='Поиск...' 
                        value={this.state.search}
                        onChange={event => this.onChangeSearch(event)}
                    />
                    <Button 
                        onClickButton={this.onSearchHandler}
                        typeButton='grey'
                        value='Поиск'
                    />
                </div>

                <div className='some_functions'>
                    <div className='sort'>
                        <span className='small_text'>Тип задач</span>
                        <Select
                            onChangeSelect={(event) => this.onChangeSelect(event)}
                        >
                            {this.renderOptions()}
                        </Select>
                    </div>
                    {localStorage.getItem('role') === 'teacher' ?
                        <div className='new_task'>
                            <Link
                                to={'/create_task'}
                            >
                                <Button 
                                    typeButton='blue'
                                    value='Новая задача'
                                />
                            </Link>
                        </div> : 
                        null
                    }
                </div>
            
                {this.renderLabs()}
            </div>
        )

        return (
            <Frame active_index={2}>
                <div className='value_subject'>{this.state.title}</div>
                <div className='main_subject'>
                    <aside className='aside_subject'>
                        {this.renderList()}
                    </aside>                       
                    { this.state.title ? main : null}
                </div>
            </Frame>
        )
    }
}

export default TasksComponent
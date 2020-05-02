import React from 'react'
import './OneTaskComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Select from '../../UI/Select/Select'
import Button from '../../UI/Button/Button'
import { Link } from 'react-router-dom'

class OneTaskComponent extends React.Component {
    state = {
        subjectId: null,
        groupId: null,
        typeId: null,
        studentsId: [1,2,3],
        titleInput: '',
        descriptionInput: '',
        files: null,
        beginDate: null,
        finishDate: null
    }

    choiceSubject = event => {
        const index = event.target.options.selectedIndex
        let subjectId = event.target.options[index].getAttribute('index')
        if (subjectId !== null)
            subjectId = +subjectId

        this.setState({
            subjectId
        })
    }

    choiceType = event => {
        const index = event.target.options.selectedIndex
        let typeId = event.target.options[index].getAttribute('index')
        if (typeId !== null)
            typeId = +typeId

        this.setState({
            typeId
        })
    }

    choiceGroup = event => {
        const index = event.target.options.selectedIndex
        let groupId = event.target.options[index].getAttribute('index')
        if (groupId !== null)
            groupId = +groupId


        this.setState({
            groupId
        })
    }

    onChangeTitle = event => {
        const titleInput = event.target.value

        this.setState({
            titleInput
        })
    }

    onChangeDescription = event => {
        const descriptionInput = event.target.value

        this.setState({
            descriptionInput
        })
    }

    onLoadFile = event => {
        this.setState({
            files: event.target.files[0]
        })
    }

    changeDate = (event, type) => {
        if (type === 'begin') {
            let beginDate = null
            if (event.target.value !== '')
                beginDate = event.target.value
            this.setState({beginDate})
        } else {
            let finishDate = null
            if (event.target.value !== '')
                finishDate = event.target.value
            this.setState({finishDate})
        }
    }


    
    renderMemberCreate() {
        const subjectId = this.state.subjectId
        const groupId = this.state.groupId

        let subjectIndex = 0
        let groupIndex = 0

        this.props.subjects.forEach((el, index) => {
            if (subjectId === null || el.id === null) return
            if (el.id === subjectId) {
                subjectIndex = index
                el.groups.forEach((item, num) => {
                    if (groupId === null || item.id === null) return
                    if (item.id === groupId) 
                    groupIndex = num 

                })
            }
        })

        const select = subjectIndex !== 0 ?        
            this.props.subjects[subjectIndex].groups.map(item => {
                return (
                    <option
                        key={item.id}
                        index={item.id}
                    >
                        {item.name}
                    </option>
                )
            }) : null
        
        const check = groupId !== null ? 
            <div className='check_all'>
                <input type='checkbox' id='all_check' className='student_checkbox' />
                <label htmlFor='all_check' className='label_all student_label_check'>
                    <span>Назначить всю группу</span>
                </label>
            </div>
        : null

        const users = groupId !== null ? 
            this.props.subjects[subjectIndex].groups[groupIndex].students.map((item)=>{
                return (
                    <li
                        key={item.id}
                        index={item.id}
                        className='student_li'
                    >
                        <input 
                            type='checkbox' 
                            className='student_checkbox' 
                            id={`student_${item.id}`}
                        />
                        <label 
                            className='student_label student_label_check' 
                            htmlFor={`student_${item.id}`}
                        >
                            <img src='/images/card.svg' alt='' />
                            <span>{item.name}</span>
                        </label>
                    </li>
                )
            }) : 
            null

        return (
            
            <Auxiliary>
                <h4>
                    Назначить
                    <span className='need_field'>*</span>
                </h4>
                <span>Группа:</span>
                <Select
                    typeSelect={''}
                    onChangeSelect={event => this.choiceGroup(event)}
                >
                    {select}
                </Select>
                {check}
                <ul>
                    {users}
                </ul>
            </Auxiliary>
        )
    }

    renderMemberTask() {
        const users = this.props.group.students.map((item, index)=>{
                return (
                    <li
                        key={index}
                        className='student_li'
                    >
                        <img src='/images/card.svg' alt='' />
                        {item}
                    </li>
                )
            })

        return (
            <Auxiliary>
                <h4>Группа</h4>
                <p className='group_create'>{this.props.group.name}</p>
                <h4>Участники {users.length}</h4>
                <ul>
                    {users}
                </ul>
            </Auxiliary>
        )
    }
    
    renderMembers() {
        if (this.props.typeTask === 'create') {
            return this.renderMemberCreate()
        } else {
            return this.renderMemberTask()
        }
    }

    renderDateCreate() {
        return (
            <Auxiliary>
                <p className='date_p_create'>Дата начала:</p>
                <input 
                    type='datetime-local'
                    className='date_input_create' 
                    onChange={(event) => this.changeDate(event, 'begin')}
                />
                <p className='date_p_create'>Дата сдачи:</p>
                <input 
                    type='datetime-local'
                    className='date_input_create' 
                    onChange={(event) => this.changeDate(event, 'end')}
                />
            </Auxiliary>
        )
    }

    renderDateTask() {
        return (
            <div>Wait Task...</div>
        )
    }

    renderDate() {
        if (this.props.typeTask === 'create') {
            return this.renderDateCreate()
        } else {
            return this.renderDateTask()
        }
    }

    renderContainCreate() {
        const selectSubject = this.props.subjects.map(item => {
            return (
                <option
                    key={item.id}
                    index={item.id}
                >
                    {item.name}
                </option>
            )
        })

        const selectType = this.props.type.map(item => {
            return (
                <option
                    key={item.id}
                    index={item.id}
                >
                    {item.name}
                </option>
            )
        })

        return (
            <div className='contain_create'>
                <h4>Предмет<span className='need_field'>*</span></h4>
                <Select
                    onChangeSelect={event => this.choiceSubject(event)}
                >
                    {selectSubject}
                </Select><br />
                <h4>Тип задачи<span className='need_field'>*</span></h4>
                <Select
                    onChangeSelect={event => this.choiceType(event)}
                >
                    {selectType}
                </Select><br />
                <h4>Заголовок<span className='need_field'>*</span></h4>
                <input 
                    type='text' 
                    className='title_input' 
                    value={this.state.titleInput}
                    onChange={event => this.onChangeTitle(event)}
                /><br />
                <h4>Описание:<span className='need_field'>*</span></h4>
                <textarea
                    type='text' 
                    className='description_textarea' 
                    placeholder='Добавьте описание задачи...'
                    defaultValue={this.state.descriptionInput}
                    onChange={event => this.onChangeDescription(event)}
                />
                <label className='label_file'>
                    <span className='title_file'>
                        Перетащите файл в это поле или кликните сюда для загрузки
                        {/*Сменная надпись на Успешно загружено <name> или Неверный формат*/}
                    </span><br />
                    <input 
                        type='file' 
                        accept='application/msword,text/plain,application/pdf,image/jpeg,image/pjpeg' 
                        onChange={event => this.onLoadFile(event)}
                    />
                </label>
                {this.state.files ? <p>Файл загружен если чо <span role='img'>😎😎😎</span>)) Я доделаю это место, чтобы было красиво, это так сделано по фасту))</p> : null}
            </div>
        )
    }   

    renderContainTask() {
        return (
            <div>Opa</div>
        )
    }

    renderContain() {
        const cls = []
        const createTask = {
            task: {}
        }
        if (
            this.state.subjectId !== null && 
            this.state.typeId !== null && 
            this.state.groupId !== null &&
            this.state.studentsId.length !== 0 &&
            this.state.titleInput.trim() !== '' &&
            this.state.descriptionInput.trim() !== '' &&
            this.state.beginDate !== null &&
            this.state.finishDate !== null      
            ) {
                createTask.task.subjectId = this.state.subjectId 
                createTask.task.typeId = this.state.typeId 
                createTask.task.groupId = this.state.groupId
                createTask.task.name = this.state.titleInput 
                createTask.task.contentText = this.state.descriptionInput
                createTask.task.studentId = this.state.studentsId
                createTask.task.beginDate = this.state.beginDate
                createTask.task.finishDate = this.state.finishDate

                createTask.file = this.state.files
                cls.push('blue_big')
        } else 
            cls.push('disactive_big')
        
        if (this.props.typeTask === 'create') {
            return (
                <Auxiliary>
                    {this.renderContainCreate()}
                    <Button 
                        typeButton={cls.join(' ')}
                        onClickButton={() => this.props.onSendCreate(createTask)}
                        value='Добавить задачу'
                    />
                </Auxiliary>
            )
        } else {
            return this.renderContainTask()
        }
    }

    renderTitle() {
        if (this.props.typeTask === 'create') {
            return (
                <h2>Создание задачи</h2>
            )
        } else {
            return(
                <h2>Задача</h2>
            )
        } 
    }

    render() {
        return (
            <Frame>
                    <div className='big_title_task'>
                        <Link
                            className='back_task'
                            to='/tasks'
                        >
                            Вернуться к списку задач
                        </Link>
                        { this.renderTitle() }
                    </div>
                <div className='main_one_task'>
                    <div className='task_options'>
                        { this.renderContain() }
                    </div>
                    <aside className='aside_one_task'>
                        <div className='memebers'>
                            { this.renderMembers() }
                        </div>
                        <div className='date_create'>
                            <h4>
                                Срок выполнения
                                <span className='need_field'>*</span>
                            </h4>
                            { this.renderDate() }
                            <p className='small_text_date'>Обратите внимание, что дата сдачи должна быть не ранее даты начала</p>
                        </div>
                    </aside>
                </div>
            </Frame>
        )
    }
}

export default OneTaskComponent
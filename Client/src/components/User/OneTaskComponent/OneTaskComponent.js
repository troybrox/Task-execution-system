import React from 'react'
import './OneTaskComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Select from '../../UI/Select/Select'
import Button from '../../UI/Button/Button'
import { Link } from 'react-router-dom'
import Answer from '../../UI/Answer/Answer'
import Loader from '../../UI/Loader/Loader'

// Компонент отображения отдельного окна задач(создание задачи и открытая задача) для препода и студента
class OneTaskComponent extends React.Component {
    state = {
        subjectId: null,
        groupId: null,
        typeId: null,
        studentIds: [],
        titleInput: '',
        descriptionInput: '',
        files: null,
        beginDate: null,
        finishDate: null,
        checkAll: false
    }

    choiceSubject = event => {
        const index = event.target.options.selectedIndex
        let subjectId = event.target.options[index].getAttribute('index')

        this.setState({
            subjectId
        })
    }

    choiceType = event => {
        const index = event.target.options.selectedIndex
        let typeId = event.target.options[index].getAttribute('index')

        this.setState({
            typeId
        })
    }

    choiceGroup = event => {
        const index = event.target.options.selectedIndex
        let groupId = event.target.options[index].getAttribute('index')
        if (index !== 0)
            this.props.changeChecked(index, null, false)

        this.setState({
            groupId,
            studentIds: [],
            checkAll: false
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

    changeCheckedHandler = (groupIndex, flag, studentIndex, id) => {
        const studentIds = [...this.state.studentIds]
        let checkAll = false

        if (flag === 'all') {
            this.props.changeChecked(groupIndex, null, !this.state.checkAll)
            checkAll = !this.state.checkAll
            studentIds.length = 0
            if (checkAll) {
                this.props.groups[groupIndex].students.forEach(el => {
                    studentIds.push(el.id)
                })
            }
        } else {
            this.props.changeChecked(groupIndex, studentIndex)

            const checkIndex = studentIds.indexOf(id)
            if (checkIndex === -1) {
                studentIds.push(id)           
            } else {
                studentIds.splice(checkIndex, 1)
            }
        }

        this.setState({
            studentIds,
            checkAll
        })
    }

    removeFile = () => {
        this.setState({
            files: null
        })
    }

    sendSolution = () => {
        const createSolution = {
            task: {}
        }
        createSolution.task.contentText = this.state.descriptionInput
        createSolution.task.taskId = this.props.idTask
        createSolution.file = this.state.files
        this.props.onSendSolution(createSolution, this.props.idTask)
    }

    renderMemberCreate() {
        const groupId = this.state.groupId
        let groupIndex = 0

        this.props.groups.forEach((item, num) => {
            if (groupId !== null && item.id !== null && item.id === +groupId) 
                groupIndex = num
        })

        const select = this.props.groups.map(item => {
            return (
                <option
                    key={item.id}
                    index={item.id}
                >
                    {item.name}
                </option>
            )
        })
        
        const check = groupId !== null ? 
            <div className='check_all'>
                <input 
                    type='checkbox' 
                    id='all_check' 
                    className='student_checkbox'
                    checked={this.state.checkAll}
                    readOnly
                />
                <label 
                    htmlFor='all_check' 
                    className='label_all student_label_check'
                    onClick={() => this.changeCheckedHandler(groupIndex, 'all')}
                >
                    <span>Назначить всю группу</span>
                </label>
            </div>
        : null

        const users = groupId !== null ? 
            this.props.groups[groupIndex].students.map((item, index)=>{
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
                            checked={item.check}
                            readOnly
                        />
                        <label 
                            className='student_label student_label_check' 
                            htmlFor={`student_${item.id}`}
                            onClick={() => this.changeCheckedHandler(groupIndex, '', index, item.id)}
                        >
                            <img src='/images/card.svg' alt='' />
                            <span>{item.surname} {item.name}</span>
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
        const users = this.props.taskAdditionData.students.map((item, index)=>{
                return (
                    <li
                        key={index}
                        className='student_li'
                    >
                        <img src='/images/card.svg' alt='' />
                        <span>{item.surname} {item.name}</span>
                    </li>
                )
            })

        return ( this.props.loading ? <Loader /> :
            <Auxiliary>
                <h4>Группа</h4>
                <p className='group_create'>{this.props.taskAdditionData.group}</p>
                <h4>Участники {this.props.taskAdditionData.studentsCount}</h4>
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
                <h4>
                    Срок выполнения
                    <span className='need_field'>*</span>
                </h4>
                <p className='date_p_create'>Дата начала:</p>
                <input 
                    type='datetime-local'
                    min={(new Date()).toJSON().substr(0, 16)}
                    max={this.state.finishDate}
                    required
                    className='date_input_create' 
                    onChange={(event) => this.changeDate(event, 'begin')}
                />
                <p className='date_p_create'>Дата сдачи:</p>
                <input 
                    type='datetime-local'
                    min={this.state.beginDate || (new Date()).toJSON().substr(0, 16)}
                    required
                    className='date_input_create' 
                    onChange={(event) => this.changeDate(event, 'end')}
                />
                <p className='small_text_date'>Обратите внимание, что дата сдачи должна быть не ранее даты начала</p>
            </Auxiliary>
        )
    }

    renderDateTask() {
        const timeBar = this.props.taskAdditionData.timeBar
        return ( this.props.loading ? <Loader /> :
            <Auxiliary>
                <h4>Срок выполнения</h4>
                <div>
                    
                    <p className='date_p_create'>
                        Дата начала:
                        <span>{this.props.taskAdditionData.beginDate}</span>
                    </p>
                    <p className='date_p_create'>
                        Дата сдачи:
                        <span>{this.props.taskAdditionData.finishDate}</span>
                    </p>
                    <p className='date_p_create'>
                        Осталось времени: 
                        <span className='time_bar' style={{background: `linear-gradient(90deg, rgb(81, 163, 201) ${timeBar}%, white ${timeBar}%)`}}>
                            {timeBar} %
                        </span>
                    </p>
                    {localStorage.getItem('role') === 'teacher' ?
                        <Button 
                            typeButton='close'
                            onClickButton={this.props.onCloseTask}
                            value='Закрыть задачу'
                        /> :
                        null
                    }
                </div>
            </Auxiliary>

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

        const selectType = this.props.types.map(item => {
            return (
                <option
                    key={item.id}
                    index={item.id}
                >
                    {item.name}
                </option>
            )
        })

        const clsForFile = ['label_file']
        if (this.state.files !== null) clsForFile.push('ready_file')

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
                <label 
                    className={clsForFile.join(' ')}
                >
                    {this.state.files === null ?
                        <Auxiliary>
                            <span className='title_file'>
                                Перетащите файл в это поле или кликните сюда для загрузки
                            </span><br />
                            <input 
                                type='file' 
                                accept='application/msword,text/plain,application/pdf,image/jpeg,image/pjpeg' 
                                onChange={event => this.onLoadFile(event)}
                            />
                        </Auxiliary> :
                        <Auxiliary>
                            <span className='title_file'>
                                Файл успешно загружен
                            </span><br />
                            <p>
                                <img src='/images/file-solid.svg' alt='' /><br />
                                <span>{this.state.files.name}</span>  
                            </p><br />
                            <span 
                                className='delete_file'
                                onClick={this.removeFile}
                            >
                                Удалить файл
                            </span>
                        </Auxiliary>
                    }
                </label>
            </div>
        )
    }   

    renderButtonCreate() {
        const cls = []
        const createTask = {
            task: {}
        }

        if (
            this.state.subjectId !== null && 
            this.state.typeId !== null && 
            this.state.groupId !== null &&
            this.state.studentIds.length !== 0 &&
            this.state.titleInput.trim() !== '' &&
            this.state.descriptionInput.trim() !== '' &&
            this.state.beginDate !== null &&
            this.state.finishDate !== null      
            ) {
                createTask.task.subjectId = +this.state.subjectId 
                createTask.task.typeId = +this.state.typeId 
                createTask.task.groupId = +this.state.groupId
                createTask.task.name = this.state.titleInput 
                createTask.task.contentText = this.state.descriptionInput
                createTask.task.studentIds = this.state.studentIds
                createTask.task.beginDate = this.state.beginDate
                createTask.task.finishDate = this.state.finishDate

                createTask.file = this.state.files
                cls.push('blue_big')
        } else 
            cls.push('disactive_big')

        return (
            <Button 
                typeButton={cls.join(' ')}
                onClickButton={() => this.props.onSendCreate(createTask)}
                value='Добавить задачу'
            />
        )
    }

    renderContainTask() {
        const all = this.props.taskAdditionData.solutions.map((item, index) => {
            return (
                <Answer 
                    key={index}
                    source='user.svg'
                    data={item}
                    role='student'
                />
            )
        })

        const teacherObject = {
            contentText: this.props.taskAdditionData.contentText,
            creationDate: this.props.taskAdditionData.beginDate,
            fileURI: this.props.taskAdditionData.fileURI,
            // isExpired: false,
            student: {
                id: 1,
                name: this.props.taskAdditionData.teacherName,
                surname: this.props.taskAdditionData.teacherSurname
            }    
        }                
        
        const students = this.props.taskAdditionData.students.map((item) => {
            return (
                <li key={item.id}>
                    Добавлен {item.surname} {item.name}
                </li>
            )
        })

        return ( 
            this.props.loading ? 
                <Loader /> :
                <div className='contain_task'>
                    <Answer
                        source='user-tie-solid.svg' 
                        data={teacherObject}
                        role='teacher'
                    />
                    <ul>
                        {students}
                    </ul>
                    {localStorage.getItem('role') === 'teacher' ? all : null}
                    {localStorage.getItem('role') === 'student' && this.props.taskAdditionData.isOpen ? this.renderAnswerField() : null}
                </div>
        )
    }

    renderAnswerField() {
        const clsForFile = ['label_file']
        if (this.state.files !== null) clsForFile.push('ready_file')
        let cls = 'blue_big'
        if (this.state.descriptionInput.trim() === '')
            cls = 'disactive_big'

        return (
            <div className='contain_create'>
                <textarea
                    type='text' 
                    className='description_textarea text_block' 
                    placeholder='Добавьте описание решения...'
                    defaultValue={this.state.descriptionInput}
                    onChange={event => this.onChangeDescription(event)}
                />
                <label 
                    className={clsForFile.join(' ')}
                >
                    {this.state.files === null ?
                        <Auxiliary>
                            <span className='title_file'>
                                Перетащите файл в это поле или кликните сюда для загрузки
                            </span><br />
                            <input 
                                type='file' 
                                accept='application/msword,text/plain,application/pdf,image/jpeg,image/pjpeg' 
                                onChange={event => this.onLoadFile(event)}
                            />
                        </Auxiliary> :
                        <Auxiliary>
                            <span className='title_file'>
                                Файл успешно загружен
                            </span><br />
                            <p>
                                <img src='/images/file-solid.svg' alt='' /><br />
                                <span>{this.state.files.name}</span>  
                            </p><br />
                            <span 
                                className='delete_file'
                                onClick={this.removeFile}
                            >
                                Удалить файл
                            </span>
                        </Auxiliary>
                    }
                </label>
                {localStorage.getItem('role') === 'student' ? 
                    <Button 
                        typeButton={cls}
                        value='Отправить решение'
                        onClickButton={this.sendSolution}
                    /> : 
                    null
                }
            </div>
        )
    }
   
    renderContain() {        
        if (this.props.typeTask === 'create') {
            return (
                <Auxiliary>
                    {this.renderContainCreate()}
                    {this.renderButtonCreate()}
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

    renderHeader() {
        return (
            <div
                className='each_tasks' 
            >
                { this.props.loading ? 
                    <Loader /> : 
                    <Auxiliary>
                        <div className='tasks_left'>
                            <span className='subject_for_lab'>{this.props.taskAdditionData.subject}</span>
                            <span>{this.props.taskAdditionData.type} </span>
                            <span>{this.props.taskAdditionData.name}</span>
                            <p className='small_text'>
                                Открыта {this.props.taskAdditionData.beginDate}
                                <span className='small_text'>
                                    Кол-во ответов:
                                    {' ' + this.props.taskAdditionData.solutionsCount}
                                </span>
                            </p>
                        </div>
                
                        {localStorage.getItem('role') === 'teacher' ?
                            <Button 
                                typeButton='blue_big'
                                value='Изменить'
                            /> :
                            null
                        }
                    </Auxiliary>
                }
            </div>
        )
    }

    render() {
        return (
            <Frame>
                <div className='big_title_task'>
                    <Link
                        className='back_task'
                        to='/tasks'
                    >
                        Ко всем задачам
                    </Link>
                    { this.renderTitle() }
                </div>
                <div className='header_task'>
                    { this.props.typeTask !== 'create' ? 
                        this.renderHeader() :
                        null
                    }
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
                            { this.renderDate() }
                        </div>
                    </aside>
                </div>
            </Frame>
        )
    }
}

export default OneTaskComponent
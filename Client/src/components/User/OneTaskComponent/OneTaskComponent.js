import React from 'react'
import './OneTaskComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Select from '../../UI/Select/Select'
import Button from '../../UI/Button/Button'
import { Link } from 'react-router-dom'
import Answer from '../../UI/Answer/Answer'
import Loader from '../../UI/Loader/Loader'

// Компонент отображения отдельного окна задач(создание и редактирование задачи для преподавателя + открытая задача для студента)
class OneTaskComponent extends React.Component {
    state = {
        subjectId: null,
        groupId: null,
        typeId: null,
        studentIds: [],
        titleInput: Object.keys(this.props.taskAdditionData).length !== 0 ? this.props.taskAdditionData.name : '',
        descriptionInput: Object.keys(this.props.taskAdditionData).length !== 0 ? this.props.taskAdditionData.contentText : '',
        descriptionTextarea: '',
        files: null,
        beginDate: '',
        finishDate: '',
        checkAll: false,
        editAnswer: false
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

    onChangeDescription = (event, type) => {
        if (type === 'textarea') {
            const descriptionTextarea = event.target.value
            this.setState({
                descriptionTextarea
            })
        } else {
            const descriptionInput = event.target.value
            this.setState({
                descriptionInput
            })
        }
    }

    onLoadFile = event => {
        this.setState({
            files: event.target.files[0]
        })
    }

    changeDate = (event, type) => {
        if (type === 'begin') {
            let beginDate = ''
            if (event.target.value !== ''  && event.target.validity.valid)
                beginDate = event.target.value
            this.setState({beginDate})
        } else {
            let finishDate = ''
            if (event.target.value !== ''  && event.target.validity.valid)
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

    sendSolution = async(path) => {
        const createSolution = {
            task: {}
        }
        createSolution.task.contentText = this.state.descriptionTextarea
        if (path === 'update') 
            createSolution.task.id = +this.props.taskAdditionData.solution.id
        else 
            createSolution.task.taskId = +this.props.idTask
        createSolution.file = this.state.files
        await this.props.onSendSolution(createSolution, this.props.idTask, path)

        this.setState({
            files: null
        })
    }

    onEditAnswer = () => {
        if (this.state.editAnswer)
            this.sendSolution('update')

        this.setState({
            editAnswer: !this.state.editAnswer,
            descriptionTextarea: this.props.descriptionTextarea
        })
    }

    onEditAnswerFalse = () => {
        this.setState({
            editAnswer: false
        })
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
                            <img src='/images/card.svg' className='card_img' alt='' />
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
        const users = 'students' in this.props.taskAdditionData ?
            this.props.taskAdditionData.students.map((item, index)=>{
                return (
                    <li
                        key={index}
                        className='student_li'
                    >
                        <img src='/images/card.svg' className='card_img' alt='' />
                        <span>{item.surname} {item.name}</span>
                    </li>
                )
            }) : 
            null

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
        if (this.props.typeTask === 'create' && Object.keys(this.props.taskAdditionData).length === 0 ) {
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
                    {Object.keys(this.props.taskAdditionData).length === 0 ? <span className='need_field'>*</span> : null}
                </h4>
                <p className='date_p_create'>Дата начала:</p>
                <input 
                    type='datetime-local'
                    min={(new Date()).toJSON().substr(0, 16)}
                    max={this.state.finishDate}
                    required
                    value={this.state.beginDate}
                    readOnly={Object.keys(this.props.taskAdditionData).length !== 0}
                    className='date_input_create' 
                    onChange={(event) => this.changeDate(event, 'begin')}
                />
                <p className='date_p_create'>Дата сдачи:</p>
                <input 
                    type='datetime-local'
                    min={this.state.beginDate || (new Date()).toJSON().substr(0, 16)}
                    required
                    value={this.state.finishDate}
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
                        <span>
                            {this.props.taskAdditionData.finishDate}
                        </span>
                    </p>
                    {this.props.taskAdditionData.isOpen ?
                        <p className='date_p_create'>
                            Осталось времени: 
                            <span className='time_bar' style={{background: `linear-gradient(90deg, rgb(81, 163, 201) ${timeBar}%, white ${timeBar}%)`}}>
                                {timeBar} %
                            </span>
                        </p> : 
                    null}

                    {this.props.taskAdditionData.isOpen ?
                        this.props.role === 'teacher' ?
                            <Button 
                                typeButton='close'
                                onClickButton={this.props.onCloseTask}
                                value='Закрыть задачу'
                            /> :
                            null :
                        <p className='close_task'>Задача закрыта</p>       
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

        return (
            <div className='contain_create'>
                <h4>Предмет{Object.keys(this.props.taskAdditionData).length === 0 ? <span className='need_field'>*</span> : null}</h4>
                {Object.keys(this.props.taskAdditionData).length !== 0 ? 
                    <span className='simple_text'>
                        {this.props.taskAdditionData.subject}
                    </span> : 
                    <Select
                        onChangeSelect={event => this.choiceSubject(event)}
                    >
                        {selectSubject}
                    </Select>
                }
                <br />
                <h4>Тип задачи{Object.keys(this.props.taskAdditionData).length === 0 ? <span className='need_field'>*</span> : null}</h4>
                {Object.keys(this.props.taskAdditionData).length !== 0 ? 
                    <span className='simple_text'>
                        {this.props.taskAdditionData.type}
                    </span> : 
                    <Select
                        onChangeSelect={event => this.choiceType(event)}
                    >
                        {selectType}
                    </Select>
                }
                <br />
                <h4>Заголовок{Object.keys(this.props.taskAdditionData).length === 0 ? <span className='need_field'>*</span> : null}</h4>
                <input 
                    type='text' 
                    className='title_input' 
                    value={this.state.titleInput}
                    onChange={event => this.onChangeTitle(event)}
                /><br />
                <h4>Описание:{Object.keys(this.props.taskAdditionData).length === 0 ? <span className='need_field'>*</span> : null}</h4>
                <textarea
                    type='text' 
                    className='description_textarea' 
                    placeholder='Добавьте описание задачи...'
                    defaultValue={this.state.descriptionInput}
                    onChange={event => this.onChangeDescription(event, 'input')}
                />
                {this.renderFile()}
            </div>
        )
    }   

    renderFile() {
        const clsForFile = ['label_file']
        if (this.state.files !== null) {
            clsForFile.push('ready_file')
            return (
                <label 
                    className={clsForFile.join(' ')}
                >
                    <span className='title_file'>
                        Файл успешно загружен
                    </span><br />
                    <p>
                        <img src='/images/file-solid.svg' alt='' /><br />
                        <span className='file_name'>{this.state.files.name}</span>  
                    </p><br />
                    <span 
                        className='delete_file'
                        onClick={this.removeFile}
                    >
                        Удалить файл
                    </span>
                </label>
            )
        } else {
            if (Object.keys(this.props.taskAdditionData).length !== 0) {
                if (this.props.taskAdditionData.fileName !== null) {
                    clsForFile.push('ready_file')
                    return (
                        <label 
                            className={clsForFile.join(' ')}
                        >
                            <span className='title_file'>
                                Ваш файл
                            </span><br />
                            <p>
                                <img src='/images/file-solid.svg' alt='' /><br />
                                <span className='file_name'>{this.props.taskAdditionData.fileName}</span>  
                            </p><br />
                            <span 
                                className='delete_file'
                                onClick={this.removeFile}
                            >
                                Заменить файл
                                <input 
                                    type='file' 
                                    accept='application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.openxmlformats-officedocument.wordprocessingml.template,application/pdf,image/jpeg,image/pjpeg' 
                                    onChange={event => this.onLoadFile(event)}
                                />
                            </span>
                        </label>
                    )
                } else {
                    return (
                        <label 
                            className={clsForFile.join(' ')}
                        >
                            <span className='title_file'>
                                Перетащите файл в это поле или кликните сюда для загрузки
                            </span><br />
                            <input 
                                type='file' 
                                accept='application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.openxmlformats-officedocument.wordprocessingml.template,application/pdf,image/jpeg,image/pjpeg' 
                                onChange={event => this.onLoadFile(event)}
                            />
                        </label>
                    )
                }
            } else {
                return (
                    <label 
                        className={clsForFile.join(' ')}
                    >
                        <span className='title_file'>
                            Перетащите файл в это поле или кликните сюда для загрузки
                        </span><br />
                        <input 
                            type='file' 
                            accept='application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.openxmlformats-officedocument.wordprocessingml.template,application/pdf,image/jpeg,image/pjpeg' 
                            onChange={event => this.onLoadFile(event)}
                        />
                    </label>
                )
            }
        }
    }

    renderButtonCreate() {
        const cls = []
        const createTask = {
            task: {}
        }

        if (Object.keys(this.props.taskAdditionData).length !== 0) {
            if (
                this.state.titleInput.trim() !== '' &&
                this.state.descriptionInput.trim() !== '' &&
                this.state.finishDate !== ''      
                ) {
                    createTask.task.id = +this.props.taskAdditionData.id
                    createTask.task.subjectId = 0 
                    createTask.task.typeId = 0 
                    createTask.task.groupId = 0 
                    createTask.task.name = this.state.titleInput 
                    createTask.task.contentText = this.state.descriptionInput
                    createTask.task.studentIds = null
                    createTask.task.beginDate = this.state.beginDate
                    createTask.task.finishDate = this.state.finishDate
    
                    createTask.file = this.state.files
                    cls.push('blue_big')
            } else 
                cls.push('disactive_big')

            return (
                <Button 
                    typeButton={cls.join(' ')}
                    onClickButton={() => this.props.onSendCreate(createTask, 'update')}
                    value='Изменить задачу'
                />
            )
        } else {
            if (
                this.state.subjectId !== null && 
                this.state.typeId !== null && 
                this.state.groupId !== null &&
                this.state.studentIds.length !== 0 &&
                this.state.titleInput.trim() !== '' &&
                this.state.descriptionInput.trim() !== '' &&
                this.state.beginDate !== '' &&
                this.state.finishDate !== ''      
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
                    onClickButton={() => this.props.onSendCreate(createTask, 'add')}
                    value='Добавить задачу'
                />
            )
        }
    }

    renderContainTask() {
        const all = this.props.role === 'student' ? 
            'solution' in this.props.taskAdditionData && this.props.taskAdditionData.solution !== null ?
                !this.state.editAnswer ?
                    <Answer 
                        source='user.svg'
                        data={this.props.taskAdditionData.solution}
                        role='student'
                    /> : 
                    this.renderAnswerField() :
                null :
            'solutions' in this.props.taskAdditionData ?
                this.props.taskAdditionData.solutions.map((item, index) => {
                    return (
                        <Answer 
                            key={index}
                            source='user.svg'
                            data={item}
                            role='student'
                        />
                    )
                }) :
                null

        const teacherObject = {
            contentText: this.props.taskAdditionData.contentText,
            creationDate: this.props.taskAdditionData.beginDate,
            fileURI: this.props.taskAdditionData.fileURI,
            fileName: this.props.taskAdditionData.fileName,
            studentName: this.props.taskAdditionData.teacherName,
            studentSurname: this.props.taskAdditionData.teacherSurname  
        }                
        
        const students = 'students' in this.props.taskAdditionData ? 
            this.props.taskAdditionData.students.map((item) => {
                return (
                    <li key={item.id}>
                        Добавлен {item.surname} {item.name}
                    </li>
                )
            }) : 
            null

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
                    {all}
                    {this.props.role === 'student' && this.props.taskAdditionData.isOpen ? 
                        this.props.taskAdditionData.solution === null ? 
                            this.renderAnswerField() : 
                            <Auxiliary>
                                <Button typeButton='blue' value='Изменить' onClickButton={this.onEditAnswer} /> 
                                {this.state.editAnswer ? <Button typeButton='grey' value='Отмена' onClickButton={this.onEditAnswerFalse} /> : null}
                            </Auxiliary> : 
                        null}
                </div>
        )
    }

    renderAnswerField() {
        let cls = 'blue_big'
        if (this.state.descriptionTextarea.trim() === '')
            cls = 'disactive_big'

        return (
            <div className='contain_create'>
                <textarea
                    type='text' 
                    className='description_textarea text_block' 
                    placeholder='Добавьте описание решения...'
                    defaultValue={this.state.descriptionTextarea}
                    onChange={event => this.onChangeDescription(event, 'textarea')}
                />
                {this.renderFileAnswer()}
                {this.props.role === 'student' && !this.state.editAnswer ? 
                    <Button 
                        typeButton={cls}
                        value='Отправить решение'
                        onClickButton={() => this.sendSolution('add')}
                    /> : 
                    null
                }
            </div>
        )
    }

    renderFileAnswer() {
        const clsForFile = ['label_file']
        if (this.state.files !== null) {
            clsForFile.push('ready_file')
            return (
                <label 
                    className={clsForFile.join(' ')}
                >
                    <span className='title_file'>
                        Файл успешно загружен
                    </span><br />
                    <p>
                        <img src='/images/file-solid.svg' alt='' /><br />
                        <span className='file_name'>{this.state.files.name}</span>  
                    </p><br />
                    <span 
                        className='delete_file'
                        onClick={this.removeFile}
                    >
                        Удалить файл
                    </span>
                </label>
            )
        } else {
            if (Object.keys(this.props.taskAdditionData).length !== 0 && this.props.taskAdditionData.solution !== null) {
                if (this.props.taskAdditionData.solution.fileName !== null) {
                    clsForFile.push('ready_file')
                    return (
                        <label 
                            className={clsForFile.join(' ')}
                        >
                            <span className='title_file'>
                                Ваш файл
                            </span><br />
                            <p>
                                <img src='/images/file-solid.svg' alt='' /><br />
                                <span className='file_name'>{this.props.taskAdditionData.solution.fileName}</span>  
                            </p><br />
                            <span 
                                className='delete_file'
                                onClick={this.removeFile}
                            >
                                Заменить файл
                                <input 
                                    type='file' 
                                    accept='application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.openxmlformats-officedocument.wordprocessingml.template,application/pdf,image/jpeg,image/pjpeg' 
                                    onChange={event => this.onLoadFile(event)}
                                />
                            </span>
                        </label>
                    )
                } else {
                    return (
                        <label 
                            className={clsForFile.join(' ')}
                        >
                            <span className='title_file'>
                                Перетащите файл в это поле или кликните сюда для загрузки
                            </span><br />
                            <input 
                                type='file' 
                                accept='application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.openxmlformats-officedocument.wordprocessingml.template,application/pdf,image/jpeg,image/pjpeg' 
                                onChange={event => this.onLoadFile(event)}
                            />
                        </label>
                    )
                }
            } else {
                return (
                    <label 
                        className={clsForFile.join(' ')}
                    >
                        <span className='title_file'>
                            Перетащите файл в это поле или кликните сюда для загрузки
                        </span><br />
                        <input 
                            type='file' 
                            accept='application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/vnd.openxmlformats-officedocument.wordprocessingml.template,application/pdf,image/jpeg,image/pjpeg' 
                            onChange={event => this.onLoadFile(event)}
                        />
                    </label>
                )
            }
        }
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
                <h2>{Object.keys(this.props.taskAdditionData).length !== 0 ? 'Изменение задачи' : 'Создание задачи' }</h2>
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
                                {this.props.taskAdditionData.updateDate !== null && this.props.taskAdditionData.isOpen ?
                                    `Обновлено ${this.props.taskAdditionData.updateDate}` :
                                    null    
                                }
                                <span className='small_text'>
                                    Кол-во ответов:
                                    {' ' + this.props.taskAdditionData.solutionsCount}
                                </span>
                            </p>
                        </div>
                
                        {this.props.role === 'teacher' && this.props.taskAdditionData.isOpen ?
                            <Link to='/create_task'>
                                <Button
                                    typeButton='blue_big'
                                    value='Изменить'
                                />
                            </Link> : 
                            null
                        }
                    </Auxiliary>
                }
            </div>
        )
    }

    renderLinkBack() {
        if (Object.keys(this.props.taskAdditionData).length !== 0 && this.props.typeTask === 'create') {
            return (
                <Link
                    className='back_task'
                    to={`/tasks/${this.props.taskAdditionData.id}`}
                >
                    Назад к задаче
                </Link>
            )
        } else {
            return (
                <Link
                    className='back_task'
                    to='/tasks'
                >
                    Ко всем задачам
                </Link>
            )
        }
    }

    componentDidMount() {
        if (Object.keys(this.props.taskAdditionData).length !== 0) {
            let t
            const parseBegin = this.props.taskAdditionData.beginDate.split('.')
            t = parseBegin[1]
            parseBegin[1] = parseBegin[0]
            parseBegin[0] = t
            const beginDate = new Date(parseBegin.join('.')).toJSON().substr(0, 16)

            const parseFinish = this.props.taskAdditionData.finishDate.split('.')
            t = parseFinish[1]
            parseFinish[1] = parseFinish[0]
            parseFinish[0] = t
            const finishDate = new Date(parseFinish.join('.')).toJSON().substr(0, 16)

            this.setState({
                beginDate,
                finishDate
            })
        }
    }

    render() {
        return (
            <Frame>
                <div className='big_title_task'>
                    { this.renderLinkBack()}
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
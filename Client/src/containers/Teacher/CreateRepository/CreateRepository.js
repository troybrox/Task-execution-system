import React from 'react'
import './CreateRepository.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Input from '../../../components/UI/Input/Input'
import Button from '../../../components/UI/Button/Button'
import { Link, Redirect } from 'react-router-dom'
import { connect } from 'react-redux'
import { fetchCreateRepository, sendCreateRepository } from '../../../store/actions/teacher'

class CreateRepository extends React.Component {
    state = {
        fields: [
            { value: null, label: 'Предмет', type: 'select', serverName: 'SubjectId', valid: false },
			{ value: '', label: 'Имя репозитория', type: 'text', serverName: 'Name', valid: false },
            { value: '', label: 'Описание', type: 'textarea', serverName: 'ContentText', valid: false },
        ],
        files: null
    }

    onLoadFile = event => {
        this.setState({
            files: event.target.files[0]
        })
    }

    removeFile = () => {
        this.setState({
            files: null
        })
    }

    onChangeField = (event, index) => {
        const fields = [...this.state.fields]
        fields[index].value = event.target.value
        fields[index].valid = fields[index].value.trim() !== '' ? true : false
        this.setState({
            fields
        })
    }

    onSelectSubject = (event) => {
        const fields = [...this.state.fields]
        const index = event.target.options.selectedIndex
        let subjectId = event.target.options[index].getAttribute('index')
        if (subjectId === null) {
            fields[0].value = subjectId
            fields[0].valid = false
        } else {
            fields[0].value = +subjectId
            fields[0].valid = true
        }

        this.setState({
            fields
        })
    }

    selectShow = () => {
        const cls = ['select']

        const select = (
            <Auxiliary key='select'>
                <select 
                        className={cls.join(' ')} 
                        onChange={event => this.onSelectSubject(event)} 
                        required
                >
                    { this.renderOptionRole() }
                </select><br />
            </Auxiliary>
        )
        return select
    }

    createRepositoryHandler = () => {
        const filters = {
            repo: {},
        }
        this.state.fields.forEach(el => {
            filters.repo[el.serverName] = el.value
        })
        filters.file = this.state.files
    
        this.props.sendCreateRepository(filters)
    }

	renderLabels() {
		return this.state.fields.map((item, index) => {
			return (
                <Auxiliary key={index}>
                    <label className='label'>{item.label}</label>
                    <span className='need_field'>*</span>
                    <br />
                </Auxiliary>
            )
		})
    }

	renderOptionRole() {
		return this.props.createRepository.map((item) => {
			return (
				<option 
                    key={item.id}
                    index={item.id} 
				>
					{item.name}
				</option>
			)
		})
	}

    renderInputs() {
        return this.state.fields.map((item, index) => {
            switch (item.type) {
                case 'select':
                    return this.selectShow()
                case 'text':
                    return <Input 
                        key={index} 
                        type={item.type}
                        value={item.value}
                        valid={true}
                        onChange={event => this.onChangeField(event, index)}
                    />
                case 'textarea':
                    return <textarea  
                        key={index}
                        type='text' 
                        className='description_textarea' 
                        placeholder='Добавьте описание репозитория...'
                        defaultValue={item.value}
                        onChange={event => this.onChangeField(event, index)}
                    />
                default:
                    return null;
            }
        })
    }

    renderFileField() {
        const clsForFile = ['label_file']
        if (this.state.files !== null) clsForFile.push('ready_file')
        return (
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
        )
    }

    componentDidMount() {
        this.props.fetchCreateRepository()
    }

    render() {
        let cls = 'blue'
        this.state.fields.forEach(item => {
            return !item.valid ? cls = 'disactive' : null 
        })

        const redirect = (
            <Redirect to={'/repository'} />
        )

        return (
            this.props.repoActive ? redirect :
            <Frame active_index={3}>
                <div className='create'>
                    <h2>Новый репозиторий</h2>
                    <div className='form'>
                        <div className='labels'>
                            { this.renderLabels() }
                        </div>
                        <div className='inputs'>
                            { this.renderInputs() }
                        </div>
                    </div>
                    { this.renderFileField() }
                    <div className='buttons'>
                        <Button 
                            typeButton={cls} 
                            value='Создать репозиторий'
                            onClickButton={() => this.createRepositoryHandler()}
                        />
                        <Link to={'/repository'}>
                            <Button typeButton='grey' value='Отмена' />
                        </Link>
                    </div>
                </div>
            </Frame>
        )
    }
}

function mapStateToProps(state) {
    return {
        createRepository: state.teacher.createRepository,
        repoActive: state.teacher.repoActive
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchCreateRepository: () => dispatch(fetchCreateRepository()),
        sendCreateRepository: (filters) => dispatch(sendCreateRepository(filters))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(CreateRepository)
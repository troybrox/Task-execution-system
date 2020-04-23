import React from 'react'
import Layout from '../../hoc/Layout/Layout'
import { connect } from 'react-redux'
import { registr, loadingFilters } from '../../store/actions/auth'
import Success from '../Success/Success'


class Registration extends React.Component {
	state = {
		roles: [
			{id: null, name: 'Выберите роль'},
			{id: 1, name: 'Преподаватель'},
			{id: 2, name: 'Студент'}
		],
		fields: [
			{ value: '', label: 'Имя', type: 'text', serverName: 'Name', valid: true },
			{ value: '', label: 'Отчество', type: 'text', serverName: 'Patronymic', valid: true },
			{ value: '', label: 'Фамилия', type: 'text', serverName: 'Surname', valid: true },
			{ value: '', label: 'Логин', type: 'text', serverName: 'UserName', valid: true },
			{ value: '', label: 'Адрес эл. почты', type: 'email', serverName: 'Email', valid: true },
			{ value: '', label: 'Роль', type: 'select', valid: true },
			{ value: '', label: 'Факультет', type: 'select', valid: true },
			{ specialId:null, value: '', label: 'Кафедра', type: 'select', serverName: 'Department', invisible: true, valid: true },
			{ value: '', label: 'Предмет', type: 'text', serverName: 'Discipline', invisible: true, valid: true },
			{ value: '', label: 'Должность', type: 'text', serverName: 'Position', invisible: true, valid: true },
			{ specialId:null, value: '', label: 'Группа', type: 'select', serverName: 'Group', invisible: true, valid: true },
			{ value: '', label: 'Пароль', type: 'password', serverName: 'Password', valid: true },
			{ value: '', label: 'Введите пароль еще раз', type: 'password', valid: true },
		]
	}

	// Проверка на корректность данных при регистрации
	onSubmitHandler = event => {
		event.preventDefault()
		
		let success = true   
		const password = []
		this.state.fields.forEach(el => {
			if (el.invisible) return
			if (el.type === 'password') {
				password.push(el.value)
			}
			success = !!el.value && success
		})

		if (password[0] !== password[1]) {
			success = false
			this.checkPasswordsHandler(false)
		} else {
			this.checkPasswordsHandler(true)
		}
		
		if (success) {
			this.registerHandler()
		} else {
			this.emptyFieldsHandler()
		}
	}

	// Процесс регистрации
	registerHandler = async() => {
		let role = ''
		const data = {}
		this.state.fields.forEach(item => {
			if (item.label === 'Роль') {
				if (item.value === 'Студент') role = 'student'
				else role = 'teacher'
			}
			if (!item.hasOwnProperty('serverName')) return
			if (item.hasOwnProperty('invisible')) {
				if (!item.invisible) data[item.serverName] = item.value
			} else {
				if (item.label === 'Группа' || item.label === 'Кафедра')
					data[item.serverName] = item.specialId
				else
					data[item.serverName] = item.value
			}
		})
		const url = `https://localhost:44303/api/account/register/${role}`

		this.props.registr(url, data)
	}

	// Отслеживаем изменение каждого input поля
	onChangeHandler = (event, index) => {
        let fields = [...this.state.fields]
		let control = fields[index]

		control.value = event.target.value
		control.type === 'password' ? 
			fields[index].valid = control.value !== '' : 
			fields[index].valid = control.value.trim() !== ''
        fields[index] = control

        this.setState({
            fields
        })
    }
	
	// Функция для динамического появления/скрытия дополнительных полей
	// при выборе роли и факультета пользователя
	// + изменение поля select
	selectRole = (target, label, facultyId, roleId) => {
		const fields = [...this.state.fields]
		let index;

		if (label === 'Роль' || label === 'Факультет') { 
			fields.forEach((el, number) => {
				if (el.hasOwnProperty('invisible')) {
					el.invisible = true
					index = number
				} 
			})

			if (facultyId !== null && roleId !== null)
				switch (roleId) {
					case '1':
						for (let i = 1; i < 4; i++)	{
							fields[index - i].invisible = false
						}
						break;
					case '2':
						fields[index].invisible = false
						break;
					default:
						break;
				} 
		}
		
		fields.forEach((el, number) => {
			if (el.label === label) {
				index = number
			} 
		})
		const control = fields[index]
		control.value = target.value

		const selectedIndex = target.options.selectedIndex
		const id = target.options[selectedIndex].getAttribute('index')
		if (control.label === 'Кафедра' || control.label === 'Группа') control.specialId = +id
		control.valid = id !== null
		fields[index] = control

		this.setState({
			fields
		})
	}

	// Смена валидации паролей, в случае их совпадения или несовпадения
	checkPasswordsHandler = result => {
		const fields = [...this.state.fields]
		fields.forEach(el => {
			if (el.type === 'password')	el.valid = result
		})

		this.setState({fields})
	}

	// Отображение всех пустых полей
	emptyFieldsHandler = () => {
		const fields = [...this.state.fields]
		fields.forEach(el => {
			if (el.value === '') el.valid = false
		})

		this.setState({fields})
	}

	componentDidMount() {
		if (!this.props.successPage) this.props.loadingFilters()
	}

	render() {
        return (
			this.props.successPage ? 
				<Success /> : 
				<Layout
					head='Вход'
					hTitle='Регистрация'
					link='Уже есть аккаунт? Авторизируйтесь!'
					to='/auth'
					img='images/reg.svg'
					fields={this.state.fields}
					roles={this.state.roles}
					faculties={this.props.faculties}				
					groups={this.props.groups}
					departments={this.props.departments}				
					onChange={this.onChangeHandler}
					onSelect={this.selectRole}
					onSubmit={this.onSubmitHandler}
				>
					<input className='submit input_fields' type='submit' value='Регистрация пользователя' />
				</Layout>
        )
    }
}

function mapStateToProps(state) {
	return {
		faculties: state.auth.faculties,
		groups: state.auth.groups,
		departments: state.auth.departments,
		successPage: state.auth.successPage
	}
}

function mapDispatchToProps(dispatch) {
	return {
		registr: (url, data) => dispatch(registr(url, data)),
		loadingFilters: () => dispatch(loadingFilters())
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(Registration)
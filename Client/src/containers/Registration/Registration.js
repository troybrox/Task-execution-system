import React from 'react'
import axios from 'axios'
import './Registration.scss'
import Layout from '../../hoc/Layout/Layout'

class Registration extends React.Component {
	state = {
		roles: ['Выберите роль','Преподаватель','Студент'],
		fields: [
			{ value: '', label: 'Имя', type: 'text', serverName: 'Name', valid: true },
			{ value: '', label: 'Отчество', type: 'text', serverName: 'Patronymic', valid: true },
			{ value: '', label: 'Фамилия', type: 'text', serverName: 'Surname', valid: true },
			{ value: '', label: 'Логин', type: 'text', serverName: 'UserName', valid: true },
			{ value: '', label: 'Адрес эл. почты', type: 'email', serverName: 'Email', valid: true },
			{ value: '', label: 'Роль', type: 'select', valid: true },
			{ value: '', label: 'Кафедра', type: 'text', serverName: 'Department', invisible: true, valid: true },
			{ value: '', label: 'Предмет', type: 'text', serverName: 'Discipline', invisible: true, valid: true },
			{ value: '', label: 'Должность', type: 'text', serverName: 'Position', invisible: true, valid: true },
			{ value: '', label: 'Группа', type: 'text', serverName: 'Group', invisible: true, valid: true },
			{ value: '', label: 'Пароль', type: 'password', serverName: 'Password', valid: true },
			{ value: '', label: 'Введите пароль еще раз', type: 'password', valid: true },
		]
	}

	// функция для отправки формы на сервер с проверкой на корректность данных
	onSubmitHandler = event => {
		event.preventDefault()
		
		let success = true // изначально проверка на валидность со значением true   
		const password = []
			
		// если поле заполнено, валидно и предыдущие поля такие же, значит success = true
		this.state.fields.forEach(el => {
			if (el.invisible) return
			if (el.type === 'password') {
				password.push(el.value)
			}
			success = !!el.value && success
		})

		// если пароли не совпадают, то делаем их валидацию success = false
		// и показываем это на полях
		if (password[0] !== password[1]) {
			success = false
			this.checkPasswordsHandler(false)
		} else {
			this.checkPasswordsHandler(true)
		}
		
		// если все поля валидны, то есть success = true
		if (success) {
			this.registerHandler()
			// window.location.pathname = '/success'	
		} else {
			// если success = false, то показываем какие поля невалидны
			this.emptyFieldsHandler()
		}
	}

	registerHandler = async() => {
		let role = ''
		const data = {}
		this.state.fields.forEach(item => {
			if (item.type === 'select') {
				if (item.value === 'Студент') role = 'student'
				else role = 'teacher'
			}
			if (!item.hasOwnProperty('serverName')) return
			if (item.hasOwnProperty('invisible')) {
				if (!item.invisible) data[item.serverName] = item.value
			} else {
				data[item.serverName] = item.value
			}
		})
		const url = `https://localhost:44303/api/account/register/${role}`

		try {
			await axios.post(url, data)
		} catch (error) {
			console.log(error)
		}
		
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
	// при выборе роли пользователя
	// + изменение поля Роли
	selectRole = event => {
		const fields = [...this.state.fields]
		let index;
		
		// вычисляем индекс скрытых полей
		fields.forEach((el, number) => {
			if (el.hasOwnProperty('invisible')) {
				index = number
			} 
		})

	 	// номер элемента в state.fields, который мы будем, либо показывать, либо скрывать
		for (let i = 0; i < 4; i++) {
			fields[index - i].invisible = true
		}
		
		// В зависимости от роли отображаем нужное, либо ничего не меняем
		switch (event.target.value) {
			case 'Преподаватель': 
				for (let i = 1; i < 4; i++)	{
					fields[index - i].invisible = false
				}
				break;
			case 'Студент': 
				fields[index].invisible = false
				break;
			default: break;
		}

		// Меняем поле выбора Роли
		fields.forEach((el, number) => {
			if (el.type === 'select') {
				index = number 
			} 
		})

		let control = fields[index]

		control.value = event.target.value
		control.valid = control.value !== 'Выберите роль'
		
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

	// Layout - компонент высшего порядка для аутентификации
	// (универсальный для регистрации, авторизации и восстановления пароля)
	// поэтому вносим необходимые данные компонента регистрации в props 
	// и редерим компонент Layout 
	render() {
        return (
			<Layout
				head='Вход'
				hTitle='Регистрация'
				link='Уже есть аккаунт? Авторизируйтесь!'
				to='/auth'
				img='images/reg.png'
				fields={this.state.fields}
				roles={this.state.roles}
				onChange={this.onChangeHandler}
				onSelect={this.selectRole}
				onSubmit={this.onSubmitHandler}
			>
				<input className='submit input_fields' type='submit' value='Регистрация пользователя' />
			</Layout>
        )
    }
}

export default Registration
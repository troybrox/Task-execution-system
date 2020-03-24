import React from 'react'
import './Registration.scss'
import Layout from '../../hoc/Layout/Layout'

class Registration extends React.Component {
	state = {
		roles: ['Выберите роль','Преподаватель','Студент'],
		fields: [
			{ value: '', label: 'Имя пользователя', type: 'text', valid: true },
			{ value: '', label: 'Адрес эл. почты', type: 'email', valid: true },
			{ value: '', label: 'Роль', type: 'select', valid: true },
			{ value: '', label: 'Кафедра', type: 'text', valid: true },
			{ value: '', label: 'Факультет', type: 'text', valid: true },
			{ value: '', label: 'Должность', type: 'text', invisible: true, valid: true },
			{ value: '', label: 'Группа', type: 'text', invisible: true, valid: true },
			{ value: '', label: 'Пароль', type: 'password', valid: true },
			{ value: '', label: 'Введите пароль еще раз', type: 'password', valid: true },
		]
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
	
	// Функция для динамического появления/скрытия дополнительного поля
	// при выборе роли пользователя
	// + изменение поля Роли
	selectRole = event => {
		const fields = [...this.state.fields]
		let index;
		
		// вычисляем индекс скрытых полей
		fields.forEach((el, number) => {
			if (el.hasOwnProperty('invisible')) {
				index = number - 1
				return
			} 
		})
	 	// номер элемента в state.fields, который мы будем, либо показывать, либо скрывать
		fields[index].invisible = true // изначально скрываем поле Должность
		fields[index + 1].invisible = true // так же скрываем поле Группа
		
		// В зависимости от роли отображаем нужное, либо ничего не менчем
		switch (event.target.value) {
			case 'Преподаватель': 
				fields[index].invisible = false
				break;
			case 'Студент': 
				fields[index + 1].invisible = false
				break;
			default: break;
		}

		// Меняем поле выбора Роли
		fields.forEach((el, number) => {
			if (el.type === 'select') {
				index = number 
				return
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
				checkPasswords={this.checkPasswordsHandler}
				emptyFields={this.emptyFieldsHandler}
			>
				<input className='submit input_fields' type='submit' value='Регистрация пользователя' />
			</Layout>
        )
    }
}

export default Registration
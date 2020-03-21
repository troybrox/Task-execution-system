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
        control.valid = control.value !== ''
        
        fields[index] = control

        this.setState({
            fields
        })
    }
	
	// Функция для динамического появления/скрытия дополнительного поля
	// при выборе роли пользователя
	selectRole = event => {
		const fields = [...this.state.fields]
		const index = 5 // номер элемента, в state.fields, который мы будем, либо показывать, либо скрывать
		fields[index].invisible = true // изначально скрываем поле Должность
		fields[index + 1].invisible = true // так же скрываем поле Группа
		
		// В зависимости от роли отображаем нужное, либо ничего не менчем
		switch (event.target.value) {
			case 'Преподаватель': 
				fields[index].invisible = false; 
				break;
			case 'Студент': 
				fields[index + 1].invisible = false; 
				break;
			default: break;
		}

		this.setState({
			fields
		})
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
			>
				<input type='submit' className='submit input_fields' value='Регистрация пользователя' />
			</Layout>
        )
    }
}

export default Registration
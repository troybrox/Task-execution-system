import React from 'react'
import './Registration.scss'
import LayoutAuth from '../../hoc/LayoutAuth/LayoutAuth'
import Label from '../../components/UI/Label/Label'
import Input from '../../components/UI/Input/Input'
import Auxiliary from '../../hoc/Auxiliary/Auxiliary'

class Registration extends React.Component {
	state = {
		roles: ['Выберите роль','Преподаватель','Студент'],
		fields: [
			{ label: 'Имя пользователя', type: 'text', visible: true },
			{ label: 'Адрес эл. почты', type: 'email', visible: true },
			{ label: 'Роль', type: 'select', visible: true },
			{ label: 'Кафедра', type: 'text', visible: true },
			{ label: 'Факультет', type: 'text', visible: true },
			{ label: 'Должность', type: 'text', visible: false },
			{ label: 'Группа', type: 'text', visible: false },
			{ label: 'Пароль', type: 'password', visible: true },
			{ label: 'Введите пароль еще раз', type: 'password', visible: true },
		]
	}
	
	// Функция для динамического появления/скрытия дополнительного поля
	// при выборе роли пользователя
	selectRole = role => {
		const fields = [...this.state.fields]
		const index = 5 // номер элемента, в state.fields, который мы будем, либо показывать, либо скрывать
		fields[index].visible = false // изначально скрываем поле Должность
		fields[index + 1].visible = false // так же скрываем поле Группа
		
		// В зависимости от роли отображаем нужное, либо ничего не менчем
		switch (role) {
			case 'Преподаватель': 
				fields[index].visible = true; 
				break;
			case 'Студент': 
				fields[index + 1].visible = true; 
				break;
			default: break;
		}

		this.setState({
			fields
		})
	}

	// Рендерим поля для select(для выбора роли), данные о полях берем из массива state.roles
	renderOptionRole() {
		return this.state.roles.map((role, index) => {
			return (
				<option 
					key={index} 
				>
					{role}
				</option>
			)
		})
	}

	// Рендерим колону с label с помощью универсального компонента Label
	renderLabels() {
		return this.state.fields.map((item, index) => {
			return item.visible ? <Label key={index} label={item.label} /> : null
		})
	}

	// Рендерим колону с input, с помощью универсального компонента Input
	// но так как у нас есть поле select, то делаем проверку на type
	// и в случае type='select' выводим select
	renderInputs() {
		const select = (
			<Auxiliary key='select'>
				<select className='select' onChange={(event) => this.selectRole(event.target.value)} required>
					{ this.renderOptionRole() }
				</select><br />
			</Auxiliary>
		)
		return this.state.fields.map((item, index) => {
			return item.type === 'select' ? select : 
				item.visible ? <Input key={index} type={item.type} /> : null
		})
	}

	// LayoutAuth - компонент высшего порядка для аутентификации
	// (универсальный для регистрации и авторизации)
	// поэтому вносим необходимые данные компонента регистрации в props 
	// и редерим компонент LayoutAuth 
	render() {
        return (
			<LayoutAuth
				head='Вход'
				hTitle='Регистрация'
				link='Уже есть аккаунт? Авторизируйтесь!'
				to='/auth'
				img='images/reg.png'
			>
				<div className='all_labels'>
					{ this.renderLabels() }
				</div>

				<div className='all_inputs'>
					{ this.renderInputs() }
					<input type='submit' className='submit input_fields' value='Регистрация пользователя' />
				</div>
			</LayoutAuth>
        )
    }
}

export default Registration
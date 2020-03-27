import React from 'react'
import { NavLink } from 'react-router-dom'
import axios from 'axios'
import './Auth.scss'
import Layout from '../../hoc/Layout/Layout'

class Auth extends React.Component {
    state = {
        fields: [
            { value: '', label: 'Логин/Email', type: 'text', serverName: 'Login', valid: true },
            { value: '', label: 'Пароль', type: 'password', serverName: 'Password', valid: true }
        ]
    }

    // функция для отправки формы на сервер с проверкой на корректность данных
    onSubmitHandler = event => {
        event.preventDefault()
        
        let success = true // изначально проверка на валидность со значением true

        // проверка полей авторизации
        this.state.fields.forEach(el => {
            success = el.valid && !!el.value && success
        })
        
        // если все поля валидны, то есть success = true
        if (success) {  
            this.loginHandler()  
            //window.location.pathname = '/'
        } else {
            // если success = false, то показываем какие поля невалидны
            this.emptyFieldsHandler()
        }
    }

    loginHandler = async() => {
        const url = 'https://localhost:44303/api/account/login'
        const data = {}
        this.state.fields.forEach(item => {
            data[item.serverName] = item.value
        })

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

    // Выявление невалидных полей
    emptyFieldsHandler = () => {
		const fields = [...this.state.fields]
		fields.forEach(el => {
			if (el.value === '') el.valid = false
		})

		this.setState({fields})
	}

	// Layout - компонент высшего порядка для аутентификации
	// (универсальный для регистрации, авторизации и восстановления пароля)
	// поэтому вносим необходимые данные компонента авторизации в props 
	// и редерим компонент Layout 
    render() {
        return (
            <Layout
				head='Регистрация'
				hTitle='Авторизация'
                link='Нужен аккаунт? Зарегистрируйтесь!'
                to='/registration'
                img='images/user.png'
                fields={this.state.fields}
                onChange={this.onChangeHandler}
                onSubmit={this.onSubmitHandler}
			>
                <input type='checkbox' id='checkbox' className='any_types_inputs' />
                <label className='label check_label' htmlFor='checkbox'>Запомнить меня</label><br />
                <input className='submit any_types_inputs' type='submit' value='Вход' />
                <NavLink to='/forget' className='forgot_pass'>
                    <span>Забыли пароль?</span>
                </NavLink>
			</Layout>
        )
    }
}

export default Auth
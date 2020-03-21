import React from 'react'
import './Auth.scss'
import Layout from '../../hoc/Layout/Layout'
import { NavLink } from 'react-router-dom'

class Auth extends React.Component {
    state = {
        fields: [
            { value: '', label: 'Имя пользователя/Email', type: 'text', valid: true },
            { value: '', label: 'Пароль', type: 'password', valid: true }
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
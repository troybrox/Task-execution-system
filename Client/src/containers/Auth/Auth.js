import React from 'react'
import './Auth.scss'
import LayoutAuth from '../../hoc/LayoutAuth/LayoutAuth'
import Label from '../../components/UI/Label/Label'
import Input from '../../components/UI/Input/Input'

class Auth extends React.Component {
    state = {
        fields: [
            { label: 'Имя пользователя/Email', type: 'text'},
            { label: 'Пароль', type: 'password'}
        ]
    }

    // Рендерим колону с label с помощью универсального компонента Label
    renderLabels() {
        return this.state.fields.map((item, index) => {
			return <Label key={index} label={item.label}/>
		})
    }

    // Рендерим колону с input с помощью универсального компонента Input
    renderInputs() {
        return this.state.fields.map((item, index) => {
			return <Input key={index} type={item.type}/>
		})
    }
    

	// LayoutAuth - компонент высшего порядка для аутентификации
	// (универсальный для регистрации и авторизации)
	// поэтому вносим необходимые данные компонента авторизации в props 
	// и редерим компонент LayoutAuth 
    render() {
        return (
            <LayoutAuth
				head='Регистрация'
				hTitle='Авторизация'
                link='Нужен аккаунт? Зарегистрируйтесь!'
                to='/registration'
                img='images/user.png'
			>
                <div className='all_labels'>
                    { this.renderLabels() }
                </div>

                <div className='all_inputs'>
                    { this.renderInputs() }<br />
                    <input type='checkbox' id='checkbox' className='any_types_inputs' />
                    <label className='label check_label' htmlFor='checkbox'>Запомнить меня</label><br />
                    <input className='submit any_types_inputs' type='submit' value='Вход' />
                    <a className='forgot_pass' href='http://'>Забыли пароль?</a>
                </div>
			</LayoutAuth>
        )
    }
}

export default Auth
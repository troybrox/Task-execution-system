import React from 'react'
import './Auth.scss'
import LayoutAuth from '../../hoc/LayoutAuth/LayoutAuth'

class Auth extends React.Component {
    render() {
        return (
            <div className='auth'>
                <LayoutAuth
					head='Регистрация'
					hTitle='Авторизация'
                    link='Нужен аккаунт? Зарегистрируйтесь!'
                    to='/registration'
                    img='user.png'
				>

                    <div className='labels'>
                        <label>Имя пользователя/Email</label><br />
                        <label>Пароль</label>
                    </div>

                    <div className='inputs'>
                        <input className='input_fields' type='text' /><br />
                        <input className='input_fields' type='password' /><br />
                        <input type='checkbox' id='checkbox' />
                        <label className='check_label' htmlFor='checkbox'>Запомнить меня</label><br />
                        <input className='submit' type='submit' value='Вход' />
                        <a className='forgot_pass' href='http://'>Забыли пароль?</a>
                    </div>
				</LayoutAuth>
            </div>
        )
    }
}

export default Auth
import React from 'react'
import './Registration.scss'
import LayoutAuth from '../../hoc/LayoutAuth/LayoutAuth'

class Registration extends React.Component {
    render() {
        return (
            <div className='registration'>
				<LayoutAuth
					head='Вход'
					hTitle='Регистрация'
					link='Уже есть аккаунт? Авторизируйтесь!'
					to='/auth'
					img='reg.png'
				>
					<div className='labels'>
						<label>Имя пользователя</label><br />
						<label>Адрес эл. почты</label><br />
						<label>Факультет</label><br />
						<label>Кафедра</label><br />
						<label>Должность</label><br />
						<label>Пароль</label><br />
						<label>Введите пароль еще раз</label>
					</div>

					<div className='inputs'>
						<input className='input_fields' type='text' /><br />
						<input className='input_fields' type='email' /><br />
						<input className='input_fields' type='text' /><br />
						<input className='input_fields' type='text' /><br />
						<input className='input_fields' type='text' /><br />
						<input className='input_fields' type='password' /><br />
						<input className='input_fields' type='password' /><br />
						<input type='submit' className='submit' value='Регистрация пользователя' />
					</div>
				</LayoutAuth>
            </div>
        )
    }
}

export default Registration
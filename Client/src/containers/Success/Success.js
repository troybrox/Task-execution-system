import React from 'react'
import './Success.scss'
import { NavLink } from 'react-router-dom'

const Success = () => {
    return (
        <div className='success'>
            <h2>Успешно</h2>
            <main>
                <p>
                    Действие прошло успешно! 
                    Дождитесь, пока администратор проверит информацию.
                    Как только это произойдет, Вам на почту придет сообщение с подтверждением или отказом.
                    Спасибо.
                </p>
                <NavLink to='/auth' className='link_registration'>
                    Вернуться на вход
                </NavLink>
            </main>
        </div>
    )
}

export default Success
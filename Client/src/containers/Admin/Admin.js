import React from 'react'
import './Admin.scss'
import Action from '../../components/Action/Action'

class Admin extends React.Component {
    state = {
        aside: [
            'Добавить преподавателя', 
            'Добавить студента', 
            'Удалить преподавателя', 
            'Удалить студента'
        ]
    }

    renderSideBar() {
        const side = this.state.aside.map((item, index) => {
            return (
                <li key={index}>{ item }</li>
            )
        })

        return (
            <ul>
                { side }
            </ul>
        )
    }

    render() {
        return (
            <div className='admin'>
                <header>
                    <h2>Преподаватели</h2>
                    <span>
                        <h3>Администратор</h3>
                        <img src='images/login.png' alt='login' />
                    </span>
                </header>
                <aside>
                    { this.renderSideBar() }
                </aside>
                <main>
                    <Action />
                </main>
            </div>
        )
    }
}

export default Admin
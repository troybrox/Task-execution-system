import React from 'react'
import './Admin.scss'
import Action from '../../components/Action/Action'

class Admin extends React.Component {
    state = {
        aside: [
            {value: 'Добавить преподавателя', active: false}, 
            {value: 'Добавить студента', active: false},
            {value: 'Удалить преподавателя', active: false},
            {value: 'Удалить студента', active: false}
        ]
    }

    chooseHandler = (index) => {
        const aside = [...this.state.aside]
        aside.forEach(el => {
            el.active = false
        })
        
        aside[index].active = true

        this.setState({
            aside
        })
    }

    renderSideBar() {
        const side = this.state.aside.map((item, index) => {
            const cls = ['list']
            if (item.active) cls.push('active')
            return (
                <li 
                    key={index}
                    className={cls.join(' ')}
                    onClick={this.chooseHandler.bind(this, index)}
                >
                    { item.value }
                </li>
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
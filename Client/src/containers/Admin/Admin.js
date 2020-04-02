import React from 'react'
import './Admin.scss'
import Action from '../../components/Action/Action'
// import axios from 'axios'

class Admin extends React.Component {
    state = {
        aside: [
            {value: 'Преподаватели', forSelect: 'Кафедра', active: true}, 
            {value: 'Студенты', forSelect: 'Группа', active: false},
        ],
        hTitle: 'Преподаватели',

        tabTitles: [
            {title: 'Существующие', active: true}, 
            {title: 'Заявки', active: false}
        ],
        selects: [
            {
                title: 'Факультет', 
                options: ['Все', 'Информатики', 'Политологии'], 
                show: true
            },
            {
                title: 'Кафедра',  
                options: ['Все', 'Институт ракетно-космической техники', 'Институт двигателей и энергетических установок'], 
                show: true
            },
            {
                title: 'Группа',  
                options: ['Все', '6213-010201D', '2402-020502A'], 
                show: false
            }
        ]
    }
    // https://localhost:44303/api/admin/student/regrequest
    // https://localhost:44303/api/admin/student/add
    // https://localhost:44303/api/admin/student/delete

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

    chooseHandler = index => {
        const aside = [...this.state.aside]
        const selects = [...this.state.selects]

        aside.forEach(el => {
            el.active = false
        })
        aside[index].active = true

        const hTitle = aside[index].value

        selects.forEach(el => {
            if (el.title === aside[index].forSelect || el.title === 'Факультет') el.show = true
            else el.show = false
        })

        this.setState({
            aside,
            hTitle,
            selects
        })

        // let url = 'https://localhost:44303/api/admin/teacher'
        // if (aside[index].value === 'Студент') url = 'https://localhost:44303/api/admin/student'
        // const response = axios.get(url)
    }

    renderTab() {
        return this.state.tabTitles.map((item, index) => {
            const cls = ['tab']
            if (item.active) cls.push('active_tab')
            return (
                <h4
                    key={index}
                    className={cls.join(' ')}
                    onClick={this.changeTab.bind(this, index)}
                >
                    {item.title}
                </h4>
            )
        })
    }

    changeTab = index => {
        // запрос на сервер для юзеров нужных
        // const response = axios.get('https://localhost:44303/api/admin/student/reqrequest')
        const tabTitles = [...this.state.tabTitles]
        tabTitles.forEach(el => {
            el.active = false
        })
        tabTitles[index].active = true

        this.setState({
            tabTitles
        })
    }

    renderSelect() {
        return this.state.selects.map((item, index) => {
            if (item.show)
                return (
                    <div key={index} className='sort_item'>
                        <p>{item.title}</p>
                        <select>
                            { this.renderOptions(item.options) }
                        </select>
                    </div>
                )
            else return null
        })
    }

    renderOptions(options) {
        return options.map((item, index) => {
            return (
                <option key={index}>
                    {item}
                </option>
            )
        })
    }

    render() {
        return (
            <div className='admin'>

                <header>
                    <h2>{ this.state.hTitle }</h2>
                    <span>
                        <h3>Администратор</h3>
                        <img src='images/login.png' alt='login' />
                    </span>

                    {/* <span> */}
                        {/* <h3>Выход</h3> */}
                    {/* </span> */}
                </header>

                <main>
                    <aside>
                        { this.renderSideBar() }
                    </aside>
                    
                    <div className='handle'>
                        <div className='nav'>
                            { this.renderTab() }
                        </div>

                        <div className='sort'>
                            { this.renderSelect() }
                            <button className='rm_button'>Удалить группу</button>
                        </div>

                        <div className='search'>
                            <input type='search' placeholder='Поиск...' />
                            <button>Поиск</button>
                        </div>

                        <Action
                            
                        />
                        <button className='rm_button bottom_button'>Удалить выбранные</button>
                    </div>
                </main>

            </div>
        )
    }
}

export default Admin
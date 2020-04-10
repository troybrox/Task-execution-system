import React from 'react'
import './Admin.scss'
import Action from '../../components/Action/Action'
import { connect } from 'react-redux'
import { Link } from 'react-router-dom'

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
        ],

        showButton: false,
        buttonAction: false,

        showUsers: this.props.users
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
        const showButton = false
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
            selects,
            showButton
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
        const showButton = false
        // запрос на сервер для юзеров нужных
        // const response = axios.get('https://localhost:44303/api/admin/student/reqrequest')
        const tabTitles = [...this.state.tabTitles]
        let buttonAction = false
        tabTitles.forEach(el => {
            el.active = false
        })
        tabTitles[index].active = true
        if (tabTitles[index].title === 'Заявки') buttonAction = true

        this.setState({
            tabTitles,
            buttonAction,
            showButton
        })
    }

    checkSelect = event => {
        let showButton = false
        if (event.target.value !== 'Все') showButton = true && !this.state.buttonAction

        this.setState({
            showButton
        })
    }

    renderSelect() {
        return this.state.selects.map((item, index) => {
            if (item.show)
                return (
                    <div key={index} className='sort_item'>
                        <p>{item.title}</p>
                        <select
                            onChange = {item.title === 'Группа' ? event => this.checkSelect(event) : null }
                        >
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

    componentDidMount() {
        // запрос на сервер для получения данных
    }

    searchHandler = () => {
        const showUsers = [...this.props.users]
        // const find
        // if (find.trim() !== '') {
            // сделать map'ом
            // showUsers.forEach(el => {
                // if (el.name.indexOf(find) === -1)
                // удаляем из showUsers
            // })
        // }

        this.setState({
            showUsers
        })
    }

    // ДЛЯ ПОДСВЕТКИ ЭЛЕМЕНТОВ ПОИСКА(В LOCAL STATE ХРАНИТЬ СТРОКУ ПОИСКА)
    // И ФУНКЦИЮ ИСПОЛЬЗОВАТЬ ДЛЯ ACTION(ТАК ЖЕ СДЕЛАТЬ ПРОВЕРКУ НА ПУСТУЮ СТРОКУ)
    // highlight = text => {
    //     return text.id + 
    //     '. <b>' + 
    //     text.name.replace(new RegExp(this.query, 'gi'), '<span class="highlighted">$&</span>') + 
    //     '</b> - <em>' + 
    //     text.city.replace(new RegExp(this.query, 'gi'), '<span class="highlighted">$&</span>') + 
    //     '</em>';
    // }

    render() {
        return (
            <div className='admin'>

                <header>
                    <span className='header_items admin'>
                        <img src='images/login.svg' alt='login' />
                        <h3>Администратор</h3>
                    </span>

                    <h2 className='header_items head'>{ this.state.hTitle }</h2>

                    <span className='header_items'>
                        <Link className='exit' to='/logout'>Выход</Link>
                    </span>
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
                            { this.state.showButton ? <button className='rm_button'>Удалить группу</button> : null}
                        </div>

                        <div className='search'>
                            <input type='search' placeholder='Поиск...' />
                            <button 
                                // onClick={this.searchHandler}
                            >
                                Поиск
                            </button>
                        </div>

                        <Action
                            showUsers={this.state.showUsers}
                        />
                        <button className='rm_button bottom_button'>Удалить выбранные</button>
                        {this.state.buttonAction ? <button className='rm_button bottom_button'>Добавить выбранные</button> : null}
                    </div>
                </main>

            </div>
        )
    }
}

function mapStateToProps(state) {
    return {
        users: state.admin.users
    }
}

function mapDispatchToProps(dispatch) {
    return {
        
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Admin)
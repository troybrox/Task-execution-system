import React from 'react'
import './Admin.scss'
import Action from '../../components/Action/Action'
import { connect } from 'react-redux'
import { Link } from 'react-router-dom'
import { loadingUsers, loadingLists, searchUsers } from '../../store/actions/admin'

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

        showButton: false,
        buttonAction: false,

        search: ''
    }

    chooseHandler = index => {
        const showButton = false
        const aside = [...this.state.aside]
        const selects = [...this.props.selects]

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
            showButton
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

    searchChange = event => {
        const search = event.target.value

        this.setState({
            search
        })
    }

    searchUsersHandler = () => {   
        const search = this.state.search     
        if (search.trim() !== '') {
            this.props.searchUsers(search)
        }
    }

    componentDidMount() {
        // let roleForURL = 'teachers'
        // let typeForURL = 'exist'
        
        // if (this.state.hTitle === 'Студенты') {
        //     roleForURL = 'students'
        // }
        // if (this.state.tabTitles[1].active) {
        //     typeForURL = 'reg'
        // }

        // const urlUser = `https://localhost:44303/api/admin/${typeForURL}_${roleForURL}`
        // this.props.loadingUsers(urlUser)
        
        // const urlList = `https://localhost:44303/api/admin/filters/${typeForURL}_${roleForURL}`
        // this.props.loadingLists(urlList)
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

    renderSelect() {
        return this.props.selects.map((item, index) => {
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
                            <input 
                                type='search' 
                                placeholder='Поиск...' 
                                onChange={event => this.searchChange(event)}
                            />
                            <button 
                                onClick={this.searchUsersHandler}
                            >
                                Поиск
                            </button>
                        </div>

                        <Action />
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
        selects: state.admin.selects
    }
}

function mapDispatchToProps(dispatch) {
    return {
        loadingUsers: url => dispatch(loadingUsers(url)),
        loadingLists: url => dispatch(loadingLists(url)),
        searchUsers: search => dispatch(searchUsers(search))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Admin)
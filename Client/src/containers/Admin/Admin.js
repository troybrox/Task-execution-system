import React from 'react'
import './Admin.scss'
import Action from '../../components/Action/Action'
import Error from '../../components/Error/Error'
import { connect } from 'react-redux'
import { Link } from 'react-router-dom'
import { loadingUsers, loadingLists } from '../../store/actions/admin'

class Admin extends React.Component {
    state = {
        aside: [
            {value: 'Преподаватели', forSelect: 'Кафедра', active: true}, 
            {value: 'Студенты', forSelect: 'Группа', active: false},
        ],
        tabTitles: [
            {title: 'Существующие', active: true}, 
            {title: 'Заявки', active: false}
        ],
        hTitle: 'Преподаватели',

        showButton: false,
        buttonAction: false,

        groupId: null,
        facultyId: null,
        departmentId: null,
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
        }, () => {
            this.requestUserHandler()
            this.requestListHandler()
        })
    }

    changeTab = index => {
        const showButton = false
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
        }, () => {
            this.requestUserHandler()
            this.requestListHandler()
        })
    }

    checkSelect = (event, title) => {
        const index = event.target.options.selectedIndex
        const id = event.target.options[index].getAttribute('index')

        let showButton = this.state.showButton
        if (title === 'Группа') {
            showButton = false
            if (id !== null) showButton = true && !this.state.buttonAction
        }

        let facultyId = this.state.facultyId
        let groupId = this.state.groupId
        let departmentId = this.state.departmentId
        switch (title) {
            case 'Факультет':
                facultyId = id
                break;
            case 'Группа':
                groupId = id
                break;
            case 'Кафедра':
                departmentId = id
                break;
            default:
                break;
        }

        this.setState({
            showButton,
            facultyId,
            groupId,
            departmentId
        }, () => {this.requestUserHandler()})
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
            this.requestUserHandler()
        }
    }

    pathHandler = () => {
        let roleForURL = 'teachers'
        let typeForURL = 'exist'
        
        if (this.state.aside[1].active) {
            roleForURL = 'students'
        }
        if (this.state.tabTitles[1].active) {
            typeForURL = 'reg'
        }

        return typeForURL + '_' + roleForURL
    }

    requestUserHandler = () => {
        const path = this.pathHandler()

        const facultyId = this.state.facultyId
        const groupId = this.state.groupId
        const departmentId = this.state.departmentId
        const search = this.state.search

        const urlUser = `https://localhost:44303/api/admin/${path}`
        this.props.loadingUsers(urlUser, facultyId, groupId, departmentId, search)
    }

    requestListHandler = () => {
        const path = this.pathHandler()
        const roleActive = this.state.aside[0].active

        const urlList = `https://localhost:44303/api/admin/filters/${path}`
        this.props.loadingLists(urlList, roleActive)
    }

    componentDidMount() {
        this.requestUserHandler()
        this.requestListHandler()
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
                            onChange = {event => this.checkSelect(event, item.title) }
                        >
                            { this.renderOptions(item.options, item.title) }
                        </select>
                    </div>
                )
            else return null
        })
    }

    renderOptions(options, title) {
        return options.map((item) => {
            const option  = (
                <option 
                    key={item.id}
                    index={item.id}
                >
                    {item.name}
                </option>
            )
            if (this.state.facultyId === null || item.id === null || title === 'Факультет') {
                return option
            } else {
                if (item.facultyId === +this.state.facultyId) {
                    return option
                } else {
                    return null
                }
            }
        })
    }

    render() {
        return (
            <div className='admin'>
                {this.props.errorShow ? <Error /> : null}

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
        selects: state.admin.selects,
        errorShow: state.admin.errorShow
    }
}

function mapDispatchToProps(dispatch) {
    return {
        loadingUsers: (url, facultyId, groupId, departmentId, searchString) => 
            dispatch(loadingUsers(url, facultyId, groupId, departmentId, searchString)),
        loadingLists: (url, facultyId, groupId, departmentId, searchString) => 
            dispatch(loadingLists(url, facultyId, groupId, departmentId, searchString))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Admin)
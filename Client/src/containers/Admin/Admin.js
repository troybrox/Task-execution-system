import React from 'react'
import './Admin.scss'
import Action from '../../components/Action/Action'
import Error from '../../components/Error/Error'
import Auxiliary from '../../hoc/Auxiliary/Auxiliary'
import { connect } from 'react-redux'
import { Link } from 'react-router-dom'
import { loadingUsers, loadingLists, errorWindow, actionUsersHandler, deleteGroupHandler } from '../../store/actions/admin'
import Condition from '../../components/UI/Condition/Condition'
import Button from '../../components/UI/Button/Button'

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

        showButtonAdd: false,
        buttonRemoveGroup: false,
        buttonActiveAction: false,

        groupId: null,
        facultyId: null,
        departmentId: null,
        search: ''
    }

    chooseHandler = index => {
        const showButtonAdd = false
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
            showButtonAdd
        }, () => {
            this.requestUserHandler()
            this.requestListHandler()
        })
    }

    changeTab = index => {
        const showButtonAdd = false
        const tabTitles = [...this.state.tabTitles]
        let buttonRemoveGroup = false
        tabTitles.forEach(el => {
            el.active = false
        })
        tabTitles[index].active = true
        if (tabTitles[index].title === 'Заявки') buttonRemoveGroup = true

        this.setState({
            tabTitles,
            buttonRemoveGroup,
            showButtonAdd
        }, () => {
            this.requestUserHandler()
            this.requestListHandler()
        })
    }

    checkSelect = (event, title) => {
        const index = event.target.options.selectedIndex
        const id = event.target.options[index].getAttribute('index')

        let showButtonAdd = this.state.showButtonAdd
        if (title === 'Группа') {
            showButtonAdd = false
            if (id !== null) showButtonAdd = true && !this.state.buttonRemoveGroup
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
            showButtonAdd,
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

        const url = `https://localhost:44303/api/admin/${path}`
        this.props.loadingUsers(url, facultyId, groupId, departmentId, search)
    }

    requestListHandler = () => {
        const path = this.pathHandler()
        const roleActive = this.state.aside[0].active

        const url = `https://localhost:44303/api/admin/filters/${path}`
        this.props.loadingLists(url, roleActive)
    }

    removeUsers = async() => {
        let success = window.confirm('Подтвердите ваше действие!');
        if (success) {
            const path = this.pathHandler()
            const url = `https://localhost:44303/api/admin/delete_${path}`

            await this.props.actionUsersHandler(url)
            this.requestUserHandler()
            this.requestListHandler()
        }
    }

    addUsers = async() => {
        const success = window.confirm('Подтвердите ваше действие!')
        if (success) {       
            const path = this.pathHandler()
            const url = `https://localhost:44303/api/admin/add_${path}`

            await this.props.actionUsersHandler(url)
            this.requestUserHandler()
            this.requestListHandler()
        }
    }

    removeGroup = async() => {
        const success = window.confirm('Подтвердите ваше действие!')
        if (success) {
            const url = 'https://localhost:44303/api/admin/delete_group'

            await this.props.deleteGroupHandler(url)
            this.requestUserHandler()
            this.requestListHandler()
        }
    }

    onChangeCheck = () => {
        let buttonActiveAction = false
        this.props.users.forEach((el) => {
            if (el.check) buttonActiveAction = true
        })

        this.setState({
            buttonActiveAction
        })
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

    renderButtons() {
        let cls = 'blue_big'
        const active = this.state.buttonActiveAction
        if (!active) cls = 'disactive'

        return (
            <Auxiliary>
                <Button 
                    typeButton={cls}
                    onClickButton={this.removeUsers}
                    value='Удалить выбранные'
                />

                {this.state.buttonRemoveGroup ? 
                    <Button 
                        typeButton={cls}
                        onClickButton={this.addUsers}
                        value='Добавить выбранные'
                    /> : 
                    null
                }
            </Auxiliary>
        )
    }

    render() {
        return (
            <div className='admin'>
                {this.props.errorShow ? 
                    <Error 
                        errorMessage={this.props.errorMessage}
                        errorWindow={() => this.props.errorWindow(false, [])}
                        /> : 
                    null
                }

                {this.props.actionCondition !== null ?
                    <Condition 
                        actionCondition={this.props.actionCondition}
                    /> :
                    null
                }

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
                            { this.state.showButtonAdd ? 
                                <Button
                                    typeButton='blue'
                                    onClickButton={this.removeGroup}
                                    value='Удалить группу'
                                /> : null
                            }
                        </div>

                        <div className='search'>
                            <input 
                                type='search' 
                                placeholder='Поиск...' 
                                onChange={event => this.searchChange(event)}
                            />
                            <Button 
                                typeButton='grey'
                                onClickButton={this.searchUsersHandler}
                                value='Поиск'
                            />
                        </div>

                        <Action 
                            onChangeCheck={this.onChangeCheck}
                        />
                        <div className='bottom_buttons'>
                            { this.renderButtons() }
                        </div>
                    </div>
                </main>

            </div>
        )
    }
}

function mapStateToProps(state) {
    return {
        users: state.admin.users,
        selects: state.admin.selects,
        errorShow: state.admin.errorShow,
        errorMessage: state.admin.errorMessage,
        actionCondition: state.admin.actionCondition
    }
}

function mapDispatchToProps(dispatch) {
    return {
        loadingUsers: (url, facultyId, groupId, departmentId, searchString) => 
            dispatch(loadingUsers(url, facultyId, groupId, departmentId, searchString)),
        loadingLists: (url, facultyId, groupId, departmentId, searchString) => 
            dispatch(loadingLists(url, facultyId, groupId, departmentId, searchString)),
        errorWindow: (errorShow, errorMessage) => 
            dispatch(errorWindow(errorShow, errorMessage)),
        actionUsersHandler: (url) => 
            dispatch(actionUsersHandler(url)),
        deleteGroupHandler: (url) =>
            dispatch(deleteGroupHandler(url))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Admin)
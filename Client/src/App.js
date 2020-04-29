import React from 'react'
import {Route, Switch, Redirect} from 'react-router-dom'
import Auth from './containers/Auth/Auth'
import Registration from './containers/Registration/Registration'
import Admin from './containers/Admin/Admin'
import Forget from './containers/Forget/Forget'
import Success from './containers/Success/Success'
import Logout from './components/Logout/Logout'

import ProfileStudent from './containers/Student/Profile/Profile'
import TasksStudent from './containers/Student/Tasks/Tasks'
import RepositoryStudent from './containers/Student/Repository/Repository'

import MainTeacher from './containers/Teacher/Main/Main'
import ProfileTeacher from './containers/Teacher/Profile/Profile'
import TasksTeacher from './containers/Teacher/Tasks/Tasks'
import RepositoryTeacher from './containers/Teacher/Repository/Repository'
import CreateRepository from './containers/Teacher/CreateRepository/CreateRepository'

import { connect } from 'react-redux'

class App extends React.Component {
	
	// Отображение нужных страниц относительно роли
	renderLinks = () => {
		switch (this.props.role) {
			case 'success':
				return (
					<Switch>
						<Route path='/success' component={Success} />
						<Redirect to={'/success'} />
					</Switch>
				)
			
			case 'administrator':
				return (
					<Switch>
						<Route path='/logout' component={Logout} />
						<Route path='/' exact component={Admin} />
						<Redirect to={'/'} />
					</Switch>
				)
			case 'student':
				return (
					<Switch>
						<Route path='/logout' component={Logout} />
						<Route path='/profile' component={ProfileStudent} />
						<Route path='/tasks' component={TasksStudent} />
						<Route path='/repository' component={RepositoryStudent} />
						<Redirect to={'/tasks'} />
					</Switch>
				)
			case 'teacher':
				return (
					<Switch>
						<Route path='/logout' component={Logout} />
						<Route path='/profile' component={ProfileTeacher} />
						<Route path='/tasks' component={TasksTeacher} />
						<Route path='/repository' component={RepositoryTeacher} />
						<Route path='/create_repository' component={CreateRepository} />
						<Route path='/' exact component={MainTeacher} />
						<Redirect to={'/'} />
					</Switch>
				)
			default:
				return (
					<Switch>
						<Route path='/registration' component={Registration} />
						<Route path='/forget' component={Forget} />
						<Route path='/auth' component={Auth} />
						<Redirect to={'/auth'} />
					</Switch>
				)
		}
	}
	
	render() {
		return (
    		<div className='app'>
				{ this.renderLinks() }
    		</div>
  		)
	}
}

function mapStateToProps(state) {
	return {
		role: state.auth.role
	}
}


export default connect(mapStateToProps)(App)

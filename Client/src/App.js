import React from 'react'
import {Route, Switch, Redirect} from 'react-router-dom'
import Auth from './containers/Auth/Auth'
import Registration from './containers/Registration/Registration'
import Admin from './containers/Admin/Admin'
import Forget from './containers/Forget/Forget'
import Success from './containers/Success/Success'
import Logout from './components/Logout/Logout'
import { connect } from 'react-redux'

class App extends React.Component {
	renderLinks = () => {
		switch (this.props.role) {
			case 'success':
				return (
					<Switch>
						<Route path='/success' component={Success} />
						<Redirect to={'/success'} />
					</Switch>
				)
			
			case 'admin':
				return (
					<Switch>
						<Route path='/logout' component={Logout} />
						<Route path='/admin' component={Admin} />
						<Redirect to={'/admin'} />
					</Switch>
				)
			// case 'student':
			// 	return (
			// 		<Switch>
			//			<Route path='/logout' component={Logout} />
			// 			<Route path='/student' component={Student} />
			// 			<Redirect to={'/student'} />
			// 		</Switch>
			// 	)
			// case 'teacher':
			// 	return (
			// 		<Switch>
			//			<Route path='/logout' component={Logout} />
			// 			<Route path='/teacher' component={Teacher} />
			// 			<Redirect to={'/teacher'} />
			// 		</Switch>
			// 	)
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

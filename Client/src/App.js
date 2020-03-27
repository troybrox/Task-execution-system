import React from 'react'
import {Route, Switch} from 'react-router-dom'
import Auth from './containers/Auth/Auth'
import Registration from './containers/Registration/Registration'
import Admin from './containers/Admin/Admin'
import Forget from './containers/Forget/Forget'
import Success from './containers/Success/Success'

class App extends React.Component {
	render() {
		return (
    		<div className='app'>
				<Switch>
					<Route path='/registration' component={Registration} />
					<Route path='/forget' component={Forget} />
					<Route path='/auth' component={Auth} />
					<Route path='/success' component={Success} />
					<Route path='/' exact component={Admin} />
				</Switch>
    		</div>
  		)
	}
}

export default App

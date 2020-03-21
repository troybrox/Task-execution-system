import React from 'react'
import {Route, Switch} from 'react-router-dom'
import Auth from './containers/Auth/Auth'
import Registration from './containers/Registration/Registration'
import Main from './containers/Main/Main'
import Forget from './containers/Forget/Forget'

class App extends React.Component {
	render() {
		return (
    		<div className='app'>
				<Switch>
					<Route path='/registration' component={Registration} />
					<Route path='/forget' component={Forget} />
					<Route path='/auth' component={Auth} />
					<Route path='/' exact component={Main} />
				</Switch>
    		</div>
  		)
	}
}

export default App

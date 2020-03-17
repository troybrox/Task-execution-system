import React from 'react'
import {Route, Switch} from 'react-router-dom'
import Auth from './containers/Auth/Auth'
import Registration from './containers/Registration/Registration'
import Main from './containers/Main/Main'

class App extends React.Component {
	render() {
		return (
    		<div className='app'>
				<Switch>
					<Route path='/registration' component={Registration} />
					<Route path='/auth' component={Auth} />
					<Route path='/' exact component={Main} />
				</Switch>
    		</div>
  		)
	}
}

export default App

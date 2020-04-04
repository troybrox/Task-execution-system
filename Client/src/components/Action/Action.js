import React from 'react'
import './Action.scss'

class Action extends React.Component {
    state = {
        users: [
            {name: 'Студент 1', check: false},
            {name: 'Студент 2', check: false},
            {name: 'Студент 3', check: true},
            {name: 'Студент 4', check: false}
        ]
    }

    changeChecked = index => {
        const users = [...this.state.users]
        users[index].check = !users[index].check
        this.setState({
            users
        })
    }

    renderUsers = () => {
        const list = this.state.users.map((item, index) => {
            return (
                <li 
                    key={index} 
                    className='user_list_admin'
                    onClick={this.changeChecked.bind(this, index)}
                >
                    <img src='images/card.png' alt='' />
                    <span>{item.name}</span>
                    {/* <input type='checkbox'  checked={item.check} /> */}
                </li>
            )
        })
        return <ul>{list}</ul>
    }
    
    render() {
        return (
            <div className='action'>
                { this.renderUsers() }
            </div>
        )
    }
}

export default Action
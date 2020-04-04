import React from 'react'
import './Action.scss'
import { connect } from 'react-redux'
import { changeCheckedHandler } from '../../store/actions/admin'

class Action extends React.Component {
    renderUsers = () => {
        const list = this.props.showUsers.map((item, index) => {
            return (
                <li 
                    key={index} 
                    className='user_list_admin'
                    onClick={this.props.changeChecked.bind(this, index)}
                >
                    <img src='images/card.png' alt='' />
                    <label htmlFor={`check-${index}`}>{item.name}</label>
                    <input 
                        type='checkbox' 
                        id={`check-${index}`}
                        // checked={item.check} 
                    />
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

// function mapStateToProps(state) {
//     return {
//         users: state.admin.users
//     }
// }

function mapDispatchToProps(dispatch) {
    return {
        changeChecked: index => dispatch(changeCheckedHandler(index))
    }
}

export default connect(null, mapDispatchToProps)(Action)
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
                >
                    <input 
                        type='checkbox' 
                        id={`check-${index}`}
                        className='check_list_input'
                        // checked={item.check} 
                    />

                    <label 
                        htmlFor={`check-${index}`} 
                        className='user_list_admin check_list_label'
                        onClick={this.props.changeChecked.bind(this, index)}
                    >
                        <img src='images/card.svg' alt='' />
                        <p>{item.name}</p>
                    </label>
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
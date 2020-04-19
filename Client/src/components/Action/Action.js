import React from 'react'
import './Action.scss'
import { connect } from 'react-redux'
import { changeCheckedHandler } from '../../store/actions/admin'

class Action extends React.Component {
    changeCheckedHandler = index => {
        this.props.changeChecked(index)
        this.props.onChangeCheck()
    }

    renderUsers = () => {
        const list = this.props.users.map((item, index) => {
            return (
                <li
                    key={item.id}
                >
                    <input 
                        type='checkbox' 
                        id={`check-${index}`}
                        className='check_list_input'
                        defaultChecked={item.check} 
                    />
                    
                    <label 
                        htmlFor={`check-${index}`} 
                        className='user_list_admin check_list_label'
                        onClick={this.changeCheckedHandler.bind(this, index)}
                    >
                        <img src='images/card.svg' alt='' />
                        <p className='name'>
                            {item.name}
                            <span className='additional'>{item.position}</span>
                        </p>
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

function mapStateToProps(state) {
    return {
        users: state.admin.users
    }
}

function mapDispatchToProps(dispatch) {
    return {
        changeChecked: index => dispatch(changeCheckedHandler(index))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Action)
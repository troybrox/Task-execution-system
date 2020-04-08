import React from 'react'
import './Success.scss'
import { connect } from 'react-redux'
import { success } from '../../store/actions/auth'

class Success extends React.Component {
    render() {
        return (
            <div className='success'>
                <h2>{this.props.title}</h2>
                <main>
                    <p>
                        {this.props.message}
                    </p>
                    <button
                        className='link_return'
                        onClick={this.props.success.bind(this, null, '', '')}
                    >
                        Вернуться на вход
                    </button>
                </main>
            </div>
        )
    }
}

function mapStateToProps(state) {
    return {
        title: state.auth.title,
        message: state.auth.message 
    }
}

function mapDispatchToProps(dispatch) {
    return {
        success: (role, title, message) => dispatch(success(role, title, message))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Success)
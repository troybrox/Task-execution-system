import React from 'react'
import './Error.scss'
import { connect } from 'react-redux'
import { errorWindow } from '../../store/actions/admin'

class Error extends React.Component {
    renderMessage() {
        return this.props.errorMessage.map((item, index) => {
            return <p key={index}>{item}</p>
        })
    }

    render() {
        return (
            <div className='error_field'>
                <div className='error_window'>
                    <h2>Ошибка</h2>
                    <div>
                        {this.renderMessage()}
                    </div>
                    <button onClick={() => this.props.errorWindow(false, [])}>Закрыть</button>
                </div>
            </div>
        )
    }
}

function mapStateToProps(state) {
    return {
        errorMessage: state.admin.errorMessage
    }
}

function mapDispatchToProps(dispatch) {
    return {
        errorWindow: (errorShow, errorMessage) => dispatch(errorWindow(errorShow, errorMessage))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Error)
import React from 'react'
import './Success.scss'
import { Link } from 'react-router-dom'
import { connect } from 'react-redux'
import { success } from '../../store/actions/auth'

class Success extends React.Component {    
    render() {
        return (
            <div className='success'>
                <h2>Успешно</h2>
                <main>
                    <p>
                        Действие прошло успешно! 
                        Дождитесь, пока администратор проверит информацию. 
                        Как только это произойдет, 
                        Вам на почту придет сообщение с подтверждением или отказом. 
                        Спасибо.
                    </p>
                    <Link
                        to={'auth'}
                        className='link_return'
                        onClick={() => this.props.success(false)}
                    >
                        Вход
                    </Link>
                </main>
            </div>
        )
    }
}

function mapDispatchToProps(dispatch) {
    return {
        success: (successPage) => dispatch(success(successPage))
    }
}

export default connect(null, mapDispatchToProps)(Success)
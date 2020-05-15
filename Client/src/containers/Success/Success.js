import React from 'react'
import './Success.scss'
import { Link } from 'react-router-dom'
import { connect } from 'react-redux'
import { success } from '../../store/actions/auth'

// Окно отображения успеха при регистрации
class Success extends React.Component {    
    render() {
        return (
            <div className='success'>
                <h2>Успешно</h2>
                <main>
                    <p>
                        Регистрация прошла успешно! 
                        Дождитесь, когда администратор проверит информацию и добавит вас в систему. 
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
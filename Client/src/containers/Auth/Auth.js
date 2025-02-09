import React from 'react'
import './Auth.scss'
import Layout from '../../hoc/Layout/Layout'
import { connect } from 'react-redux'
import { auth } from '../../store/actions/auth'
import Loader from '../../components/UI/Loader/Loader'
import Button from '../../components/UI/Button/Button'

class Auth extends React.Component {
    state = {
        fields: [
            { value: '', label: 'Логин/Email', type: 'text', serverName: 'UserName', valid: true },
            { value: '', label: 'Пароль', type: 'password', serverName: 'Password', valid: true }
        ],

        errorMessages: null
    }

    // функция для отправки формы на сервер с проверкой на корректность данных
    onSubmitHandler = event => {
        event.preventDefault()
        
        let success = true

        this.state.fields.forEach(el => {
            success = el.valid && !!el.value && success
        })
        
        if (success) {  
            this.loginHandler()  
        } else {
            this.emptyFieldsHandler()
        }
    }

    // Функция для авторизации
    loginHandler = () => {
        const data = {}
        this.state.fields.forEach(item => {
            data[item.serverName] = item.value
        })

        this.props.auth(data)
    }

    // Отслеживаем изменение каждого input поля
    onChangeHandler = (event, index) => {
        let fields = [...this.state.fields]
        let control = fields[index]

        control.value = event.target.value
		control.type === 'password' ? 
			fields[index].valid = control.value !== '' : 
			fields[index].valid = control.value.trim() !== ''
        
        fields[index] = control

        this.setState({
            fields
        })
    }

    // Выявление невалидных полей
    emptyFieldsHandler = () => {
		const fields = [...this.state.fields]
		fields.forEach(el => {
			if (el.value === '') el.valid = false
		})

		this.setState({fields})
    }
    
    render() {
        return (
            <Layout
				head='Регистрация'
				hTitle='Авторизация'
                link='Нужен аккаунт? Зарегистрируйтесь!'
                to='/registration'
                img='/images/user.svg'
                fields={this.state.fields}
                onChange={this.onChangeHandler}
                onSubmit={this.onSubmitHandler}
			>                
                {this.props.loading ? <Loader />: null}
                <Button 
                    typeButton='auth'
                    onClickButton={event => this.onSubmitHandler(event)}
                    value='Вход'
                />
			</Layout>
        )
    }
}

function mapStateToProps(state) {
    return {
        loading: state.auth.loading
    }
}

function mapDispatchToProps(dispatch) {
    return {
        auth: (data) => dispatch(auth(data))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Auth)
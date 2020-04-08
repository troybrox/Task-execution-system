import React from 'react'
import Layout from '../../hoc/Layout/Layout'
import { connect } from 'react-redux'
import { success } from '../../store/actions/auth'

class Forget extends React.Component {
    state = {
        fields: [
            { value: '', label: 'Имя', type: 'text', valid: true },
            { value: '', label: 'Адрес эл. почты', type: 'email', valid: true },
            { value: '', label: 'Телефон', type: 'text', valid: true },
        ]
    }

    // функция для отправки формы на сервер с проверкой на корректность данных
    onSubmitHandler = event => {
        event.preventDefault()
        let success = true
        this.state.fields.forEach(el => {
            success = el.valid && !!el.value && success
        })
        
        if (success) {
            const role = 'success'
            const title = 'Успешно'
            const message = 'Действие прошло успешно! Дождитесь, пока администратор проверит информацию. Как только это произойдет, Вам на почту придет сообщение с подтверждением или отказом. Спасибо.'
            this.props.success(role, title, message)
        } else {
            this.emptyFieldsHandler()
        }
    }

    // Отслеживаем изменение каждого input поля
    onChangeHandler = (event, index) => {
        let fields = [...this.state.fields]
        let control = fields[index]

        control.value = event.target.value
        control.valid = control.value.trim() !== ''
        
        fields[index] = control

        this.setState({
            fields
        })
    }

    // Отображение невалидных полей
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
				head='Вход'
				hTitle='Восстановление пароля'
                link='Вернуться'
                to='/auth'
                img='images/reg.png'
                fields={this.state.fields}
                onChange={this.onChangeHandler}
                onSubmit={this.onSubmitHandler}
			>
                <input className='submit any_types_inputs' type='submit' value='Восстановление' />
			</Layout>
        )
    }
}

function mapDispatchToProps(dispatch) {
    return {
        success: (role, title, message) => dispatch(success(role, title, message))
    }
}

export default connect(null, mapDispatchToProps)(Forget)
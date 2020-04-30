import React from 'react'
import Layout from '../../hoc/Layout/Layout'
import Success from '../../containers/Success/Success'
import { connect } from 'react-redux'
import { success } from '../../store/actions/auth'
import Button from '../../components/UI/Button/Button'

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
            this.props.success(success)
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
            this.props.successPage ? 
                <Success /> :
                <Layout
			    	head='Вход'
			    	hTitle='Восстановление пароля'
                    link='Вернуться'
                    to='/auth'
                    img='images/reg.svg'
                    fields={this.state.fields}
                    onChange={this.onChangeHandler}
                    onSubmit={this.onSubmitHandler}
			    >
                    <Button 
                        typeButton='auth'
                        onClickButton={event => this.onSubmitHandler(event)}
                        value='Восстановление'
                    />
			    </Layout>
        )
    }
}

function mapStateToProps(state) {
    return {
        successPage: state.auth.successPage
    }
}

function mapDispatchToProps(dispatch) {
    return {
        success: (successPage) => dispatch(success(successPage))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Forget)
import React from 'react'
import Layout from '../../hoc/Layout/Layout'
import { Redirect } from 'react-router-dom'

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

            let success = true // изначально проверка на валидность со значением true

            // проверка полей авторизации или страницы забытого пароля
            this.state.fields.forEach(el => {
                success = el.valid && !!el.value && success
            })
    
            // если все поля валидны, то есть success = true
            if (success) {
                // window.location.pathname = '/success'
                return (
                    <Redirect to={'/success'} />
                ) 
            } else {
                // если success = false, то показываем какие поля невалидны
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

    emptyFieldsHandler = () => {
		const fields = [...this.state.fields]
		fields.forEach(el => {
			if (el.value === '') el.valid = false
		})

		this.setState({fields})
	}

	// Layout - компонент высшего порядка для аутентификации
	// (универсальный для регистрации, авторизации и восстановления пароля)
	// поэтому вносим необходимые данные компонента восстановления пароля в props 
	// и редерим компонент Layout 
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

export default Forget
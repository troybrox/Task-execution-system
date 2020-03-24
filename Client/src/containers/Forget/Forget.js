import React from 'react'
// import './Forget.scss'
import Layout from '../../hoc/Layout/Layout'

class Forget extends React.Component {
    state = {
        fields: [
            { value: '', label: 'Имя', type: 'text', valid: true },
            { value: '', label: 'Адрес эл. почты', type: 'email', valid: true },
            { value: '', label: 'Телефон', type: 'text', valid: true },
        ]
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
                emptyFields={this.emptyFieldsHandler}
			>
                <input className='submit any_types_inputs' type='submit' value='Восстановление' />

			</Layout>
        )
    }
}

export default Forget
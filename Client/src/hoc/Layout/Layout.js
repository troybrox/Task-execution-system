import React from 'react'
import './Layout.scss'
import {Link} from 'react-router-dom'
import Auxiliary from '../Auxiliary/Auxiliary'
import Label from '../../components/UI/Label/Label'
import Input from '../../components/UI/Input/Input'

class Layout extends React.Component {
   
    // функция для отправки формы на сервер с проверкой на корректность данных
    submitHandler = event => {
        event.preventDefault()
        
        let success = true // изначально проверка на валидность со значением true
        // проверка полей Регистрации
        if (this.props.hTitle === 'Регистрация') {   
            const password = []
            
            // если поле заполнено, валидно и предыдущие поля такие же, значит success = true
            this.props.fields.forEach(el => {
                // скрытые поля не рассматриваем
                if (el.invisible) return

                // запоминаем значения двух паролей
                if (el.type === 'password') {
                    password.push(el.value)
                }
                success = !!el.value && success
            })

            // если пароли не совпадают, то делаем их валидацию success = false
            // и показываем это на полях
            if (password[0] !== password[1]) {
                success = false
                this.props.checkPasswords(false)
            } else {
                this.props.checkPasswords(true)
            }
        } else {
            // проверка полей авторизации или страницы забытого пароля
            this.props.fields.forEach(el => {
                success = el.valid && !!el.value && success
            })
        }

        // если все поля валидны, то есть success = true
        if (success) {
            if (this.props.hTitle === 'Авторизация') {
                // при авторизации на сервер отправляем данные и ждем ответа, 
                // после чего переходим на основную страницу

// const request = new XMLHttpRequest()   // axios
// const url = 'index.php'
// const data = {
//     page: this.props.hTitle, 
//     fields: this.props.fields
// }
// request.open('POST', url)
// request.send(data)

// request.onload = function() {
//     console.log(this.response)
// }

                window.location.pathname = '/'
            } else {
                // при регистрации на сервер отправляем данные
                if (this.props.hTitle === 'Регистрация') {

                }
                // но если это была страница забытого пароля, то ничего не делаем,
                // но можем и что-то сделать, если захотим))

                // после чего перенаправляем на страницу успешной отправки данных 
                // и просим пользователя ждать ответа на почту от администратора
                window.location.pathname = '/success'
            }
        } else {
            // если success = false, то показываем какие поля невалидны
            this.props.emptyFields()
        }
        
    }

    // Рендерим поля для select(для выбора роли), данные о полях берем из массива state.roles
	renderOptionRole() {
		return this.props.roles.map((role, index) => {
			return (
				<option 
					key={index} 
				>
					{role}
				</option>
			)
		})
	}

    // Рендерим колону с label с помощью универсального компонента Label
	renderLabels() {
		return this.props.fields.map((item, index) => {
			return item.invisible ? null : <Label key={index} label={item.label} />
		})
    }
    
    selectShow = item => {
        const cls = ['select']
        if (!item.valid) cls.push('invalid')

        const select = (
            <Auxiliary key='select'>
                <select 
                        className={cls.join(' ')} 
                        onChange={event => this.props.onSelect(event)} 
                        required
                >
                    { this.renderOptionRole() }
                </select><br />
            </Auxiliary>
        )
        return select
    }

    // Рендерим колону с input с помощью универсального компонента Input
    // но так как у нас есть поле select в регистрации, 
    // то проверяем для регистрации ли этот компонент
    // и если это так, то потом делаем проверку на type
	// и в случае type='select' выводим select
    renderInputs() {
        if (this.props.hTitle === 'Регистрация') {

            return this.props.fields.map((item, index) => {
                return item.type === 'select' ? 
                    this.selectShow(item) : 
                    item.invisible ? null :
                        <Input 
                            key={index} 
                            type={item.type} 
                            value={item.value}
                            valid={item.valid}
                            onChange={event => this.props.onChange(event, index)}
                        />
            })
        } else {
            return this.props.fields.map((item, index) => {
                return <Input 
                    key={index} 
                    type={item.type} 
                    value={item.value}
                    valid={item.valid}
                    onChange={event => this.props.onChange(event, index)}
                    />
            })
        }
    }

    // Рендерим компонент аутентификации
    // так же используем Link из библиотеки реакта для роутинга
    render() {
        return (
            <Auxiliary>
                <header className='header'>
                    <Link to={this.props.to} className='top_link'>
                        <img src={this.props.img} alt={this.props.head}/>
                        <span>{this.props.head}</span>
                    </Link>
                </header>
            
                <main className="form_box">
                    <h2>{this.props.hTitle}</h2>

                    <form onSubmit={this.submitHandler}>
                        <div className='all_labels'>
					        { this.renderLabels() }
				        </div>

				        <div className='all_inputs'>
					        { this.renderInputs() }
                            { this.props.children }
				        </div>
                    </form>

                    <Link to={this.props.to} className='link_registration'>
                        {this.props.link}
                    </Link>
                </main>
            </Auxiliary>
        )
    }
}

export default Layout

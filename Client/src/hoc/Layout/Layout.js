import React from 'react'
import './Layout.scss'
import {NavLink} from 'react-router-dom'
import Auxiliary from '../Auxiliary/Auxiliary'
import Label from '../../components/UI/Label/Label'
import Input from '../../components/UI/Input/Input'

class Layout extends React.Component {
    // функция для отправки формы на сервер с некоторой проверкой на корректность данных
    submitHandler = event => {
        event.preventDefault()

        // сделать отправку
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
    
    // Рендерим колону с input с помощью универсального компонента Input
    // но так как у нас есть поле select в регистрации, 
    // то проверяем для регистрации ли этот компонент
    // и если это так, то потом делаем проверку на type
	// и в случае type='select' выводим select
    renderInputs() {
        if (this.props.hTitle === 'Регистрация') {
            const select = (
                <Auxiliary key='select'>
                    <select className='select' onChange={event => this.props.onSelect(event)} required>
                        { this.renderOptionRole() }
                    </select><br />
                </Auxiliary>
            )
            return this.props.fields.map((item, index) => {
                return item.type === 'select' ? 
                    select : 
                    item.invisible ? null :
                        <Input 
                            key={index} 
                            type={item.type} 
                            value={item.value}
                            valid={item.valid}
                            onChange={event => this.onChange(event, index)}
                        />
            })
        } else {
            return this.props.fields.map((item, index) => {
                return <Input 
                    key={index} 
                    type={item.type} 
                    value={item.value}
                    valid={item.valid}
                    onChange={event => this.onChangeHandler(event, index)}
                    />
            })
        }
    }

    // Рендерим компонент аутентификации
    // так же используем NavLink из библиотеки реакта для роутинга
    render() {
        return (
            <Auxiliary>
                <header className='header'>
                    <NavLink to={this.props.to} className='top_link'>
                        <img src={this.props.img} alt={this.props.hTitle}/>
                        <span>{this.props.head}</span>
                    </NavLink>
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

                    <NavLink to={this.props.to} className='link_registration'>
                        {this.props.link}
                    </NavLink>
                </main>
            </Auxiliary>
        )
    }
}

export default Layout
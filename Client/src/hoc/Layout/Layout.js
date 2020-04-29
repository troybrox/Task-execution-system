import React from 'react'
import './Layout.scss'
import {Link} from 'react-router-dom'
import Auxiliary from '../Auxiliary/Auxiliary'
import Label from '../../components/UI/Label/Label'
import Input from '../../components/UI/Input/Input'

class Layout extends React.Component {
   
    // Поля для select(для выбора роли) в регистрации
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

    // Рендерим колону с label
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

    // Рендерим колону с input
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

                    <form onSubmit={this.props.onSubmit}>
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

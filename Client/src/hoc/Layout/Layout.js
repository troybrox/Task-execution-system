import React from 'react'
import './Layout.scss'
import {Link} from 'react-router-dom'
import Auxiliary from '../Auxiliary/Auxiliary'
import Label from '../../components/UI/Label/Label'
import Input from '../../components/UI/Input/Input'
import { connect } from 'react-redux'
import { errorWindow } from '../../store/actions/auth'
import Error from '../../components/Error/Error'

class Layout extends React.Component {
    state = {
        facultyId: null,
        roleId: null
    }
   
    // Поля для select(для выбора роли) в регистрации
	renderOption(options) {
		return options.map((role) => {
			return (
				<option 
                    key={role.id}
                    index={role.id}
				>
					{role.name}
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

    onSelectHandler = (target, label) => {
        const index = target.options.selectedIndex
        const id = target.options[index].getAttribute('index')
        let roleId = this.state.roleId
        let facultyId = this.state.facultyId
        switch (label) {
            case 'Факультет':
                facultyId = id
                break;
            case 'Роль':
                roleId = id
                break;
            default:
                break;
        }

        this.setState({
            roleId,
            facultyId
        }, () => {this.props.onSelect(target, label, this.state.facultyId, this.state.roleId)})
    }
    
    selectShow = item => {
        const cls = ['select']
        if (!item.valid) cls.push('invalid')
        let options = this.props.roles

        switch (item.label) {
            case 'Факультет':
                options = this.props.faculties
                break;
            case 'Группа':
                options = []
                this.props.groups.forEach(el => {
                    if (el.id === null) options.push(el)
                    else if (el.facultyId === +this.state.facultyId) options.push(el)
                })
                break;
            case 'Кафедра':
                options = []
                this.props.departments.forEach(el => {
                    if (el.id === null) options.push(el)
                    else if (el.facultyId === +this.state.facultyId) options.push(el)
                })
                break;
            default:
                break;
        }

        const select = (
            <Auxiliary key={item.label}>
                <select 
                        className={cls.join(' ')} 
                        onChange={event => this.onSelectHandler(event.target, item.label)} 
                        required
                >
                    { this.renderOption(options) }
                </select><br />
            </Auxiliary>
        )
        return select
    }

    // Рендерим колону с input
    renderInputs() {
        if (this.props.hTitle === 'Регистрация') {

            return this.props.fields.map((item, index) => {
                return item.invisible ? null :
                    item.type === 'select' ? 
                        this.selectShow(item) :
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
                { this.props.catchError ? 
					<Error
						errorMessage={this.props.catchErrorMessage}
						errorWindow={() => this.props.errorWindow(false, [])}
					/>: null}

                <header className='header'>
                    <Link to={this.props.to} className='top_link'>
                        <img src={this.props.img} alt={this.props.head}/>
                        <span>{this.props.head}</span>
                    </Link>
                </header>
                <main className='form_box'>
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

function mapStateToProps(state) {
	return {
		catchError: state.auth.catchError,
		catchErrorMessage: state.auth.catchErrorMessage,
	}
}

function mapDispatchToProps(dispatch) {
	return {
		errorWindow: (catchError, catchErrorMessage) => dispatch(errorWindow(catchError, catchErrorMessage))
	}
}

export default connect(mapStateToProps, mapDispatchToProps)(Layout)

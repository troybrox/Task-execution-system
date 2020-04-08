import React from 'react'
import './CreateRepository.scss'
import Frame from '../../../hoc/Frame/Frame'
import Label from '../../../components/UI/Label/Label'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Input from '../../../components/UI/Input/Input'

class CreateRepository extends React.Component {
    state = {
        leads: ['Преподаватель1', 'Преподаватель2', 'Преподаватель3'],
        fields: [
            { value: '', label: 'Владелец*', type: 'select', serverName: 'Lead', valid: true },
			{ value: '', label: 'Предмет*', type: 'text', serverName: 'Subject', valid: true },
			{ value: '', label: 'Имя репозитория*', type: 'text', serverName: 'Name', valid: true },
            { value: '', label: 'Описание', type: 'text', serverName: 'Description', valid: true },
        ]
    }

	renderLabels() {
		return this.state.fields.map((item, index) => {
			return <Label key={index} label={item.label} />
		})
    }

	renderOptionRole() {
		return this.state.leads.map((role, index) => {
			return (
				<option 
					key={index} 
				>
					{role}
				</option>
			)
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

    renderInputs() {
        return this.state.fields.map((item, index) => {
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
    }



    render() {
        return (
            <Frame active_index={3}>
                <div className='create'>
                    <h2>Новый репозиторий</h2>
                    <div className='form'>
                        <div className='labels'>
                            { this.renderLabels() }
                        </div>
                        <div className='inputs'>
                            { this.renderInputs() }
                        </div>
                    </div>
                    <div className='buttons'>
                        <button>Создать репозиторий</button>
                        <button className='cancel'>Отмена</button>
                    </div>
                </div>
            </Frame>
        )
    }

}

export default CreateRepository
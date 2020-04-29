import React from 'react'
import './ProfileComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Label from '../../UI/Label/Label'
import Input from '../../UI/Input/Input'

class ProfileComponent extends React.Component {
    state = {
        data_password: [
            {value: '', label: 'Старый пароль', type: 'password', serverName: 'OldPassword', valid: true},
            {value: '', label: 'Новый пароль', type: 'password', serverName: 'NewPassword', valid: true},
            {value: '', label: 'Подтвердите новый пароль', type: 'password', serverName: 'RepeatPassword', valid: true},
        ]
    }

    renderLabels(fields) {
		return fields.map((item, index) => {
			return <Label key={index} label={item.label} />
		})
    }

    renderOptionRole(options) {
		return options.map((el, index) => {
			return (
				<option 
					key={index} 
				>
					{el}
				</option>
			)
		})
	}

    selectShow = (item, index) => {
        const cls = ['select', 'hide']
        if (!item.valid) cls.push('invalid')

        let options = this.props.groups
        switch (item.serverName) {
            case 'Faculty':
                options = this.props.faculties
                break;
            case 'Department':
                options = this.props.departments               
                break;
            default:
                break;
        }

        const select = (
            <Auxiliary key={index}>
                <select 
                        className={cls.join(' ')} 
                        onChange={event => this.props.onSelect(event)} 
                        required
                >
                    { this.renderOptionRole(options) }
                </select><br />
            </Auxiliary>
        )
        return select
    }

    renderInputs(fields, hide) {
        return fields.map((item, index) => {
            return item.type === 'select' ? 
                this.selectShow(item, index) : 
                <Input
                    key={index} 
                    type={item.type} 
                    value={item.value}
                    valid={item.valid}
                    classUser={hide}
                    onChange={event => this.props.onChange(event, index)}
                />
        })
    }
    
    
    renderInfo() {
        const fields = []
        this.props.fields.forEach(el => {
            if (el.type !== 'email') fields.push(el)
        })

        return (
            <div className='info'>
                <div className='label_info'>
                    {this.renderLabels(fields)}
                </div>
                <div className='input_info'>
                    {this.renderInputs(fields, true)}
                </div>
            </div>
        )
    }

    renderContact() {
        const fields = []
        this.props.fields.forEach(el => {
            if (el.type === 'email') fields.push(el)
        })

        return (
            <div className='info'>
                <div className='label_info'>
                    {this.renderLabels(fields)}
                </div>
                <div className='input_info'>
                    {this.renderInputs(fields, 'hide')}
                </div>
            </div>
        )
    }

    renderPasswords() {
        const fields = [...this.state.data_password]
        
        return (
            <div className='info'>
                <div className='label_info'>
                    {this.renderLabels(fields)}
                </div>
                <div className='input_info'>
                    {this.renderInputs(fields)}
                </div>
            </div>
        )
    }

    render() {
        return (
            <Frame active_index={0}>
                <div className='profile'>
                    <div className='photo_side'>
                        <img src='images/profile_user.svg' alt='' />
                    </div>
                    <div className='info_side'>
                        <h3>Общая информация</h3>
                            {this.renderInfo()}
                        <h3>Контактная информация</h3>
                            {this.renderContact()}
                        <h3>Изменить пароль</h3>
                            {this.renderPasswords()}
                    </div>
                </div>
            </Frame>
        )
    }
}

export default ProfileComponent
import React from 'react'
import './ProfileComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Label from '../../UI/Label/Label'
import Input from '../../UI/Input/Input'
import Button from '../../UI/Button/Button'

class ProfileComponent extends React.Component {
    state = {
        dataPassword: [
            {value: '', label: 'Старый пароль', type: 'password', serverName: 'OldPassword', valid: true},
            {value: '', label: 'Новый пароль', type: 'password', serverName: 'NewPassword', valid: true},
            {value: '', label: 'Подтвердите новый пароль', type: 'password', serverName: 'RepeatPassword', valid: true},
        ],
        flagPassword: false,
        flagInfo: false
    }

    renderLabels(fields) {
		return fields.map((item, index) => {
            if ('element' in item)
			    return <Label key={index} label={item.element.label} />
            else 
                return <Label key={index} label={item.label} />
		})
    }

    renderProfileInputs(fields) {
        return fields.map(item => {
            if ('type' in item.element) {
                return (
                    <Input
                        key={item.index} 
                        type={item.element.type} 
                        value={item.element.value}
                        valid={item.element.valid}
                        classUser={true}
                        onChange={event => this.onChangeProfileHandler(event, item.index)}
                    />
                )
            } else {
                return (
                    <p 
                        key={item.index}
                        className='static_field'
                    >
                        {item.element.value}
                    </p>
                )
            }
        })
    }

    onChangeProfileHandler = (event, index) => {
        this.props.onChangeProfile(event.target.value, index)

        this.setState({
            flagInfo: true
        })
    }

    renderPasswordInputs(fields, hide) {
        return fields.map((item, index) => {
                return <Input
                    key={index} 
                    type={item.type} 
                    value={item.value}
                    valid={item.valid}
                    classUser={hide}
                    onChange={event => this.onChangePasswords(event, index)}
                />
        })
    }

    onChangePasswords = (event, index) => {
        const dataPassword = [...this.state.dataPassword]
        let flagPassword = true
        dataPassword[index].value = event.target.value
        dataPassword.forEach(el => {
            el.valid = true
            flagPassword = flagPassword && el.value !== ''
        })

        this.setState({
            dataPassword,
            flagPassword
        })
    }
    
    
    renderInfo() {
        const fields = []
        this.props.fields.forEach((el, num) => {
            if (el.type !== 'email') fields.push({element: el, index: num})
        })

        return (
            <div className='info'>
                <div className='label_info'>
                    {this.renderLabels(fields)}
                </div>
                <div className='input_info'>
                    {this.renderProfileInputs(fields)}
                </div>
            </div>
        )
    }

    renderContact() {
        const fields = []
        this.props.fields.forEach((el, num) => {
            if (el.type === 'email') fields.push({element: el, index: num})
        })

        return (
            <div className='info'>
                <div className='label_info'>
                    {this.renderLabels(fields)}
                </div>
                <div className='input_info'>
                    {this.renderProfileInputs(fields)}
                </div>
            </div>
        )
    }

    renderPasswords() {
        const fields = [...this.state.dataPassword]
        
        return (
            <div className='info'>
                <div className='label_info'>
                    {this.renderLabels(fields)}
                </div>
                <div className='input_info'>
                    {this.renderPasswordInputs(fields, false)}
                </div>
            </div>
        )
    }

    updatePasswordHandler = () => {
        const dataPassword = [...this.state.dataPassword]
        const data = {}
        let newPassNum, repeatPassNum

        dataPassword.forEach((el, number) => {
            if (el.serverName === 'NewPassword') newPassNum = number
            if (el.serverName === 'RepeatPassword') {
                repeatPassNum = number
                return
            }
            
            data[el.serverName] = el.value
        })

        if (dataPassword[newPassNum].value === dataPassword[repeatPassNum].value) {
            this.props.updateData(data, 'updatepassword')
        } else {
            dataPassword[newPassNum].valid = false
            dataPassword[repeatPassNum].valid = false

            this.setState({
                dataPassword
            })
        }
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
                        <div className='buttons_profile'>
                            <Button
                                typeButton={this.state.flagInfo ? 'blue' : 'disactive'}
                                onClickButton={this.props.updateProfile}
                                value='Изменить профиль' 
                            />
                        </div>
                        <h3>Изменить пароль</h3>
                            {this.renderPasswords()}
                        <div className='buttons_profile'>
                            <Button
                                typeButton={this.state.flagPassword ? 'blue' : 'disactive'}
                                onClickButton={this.updatePasswordHandler}
                                value='Изменить пароль' 
                            />
                        </div>
                    </div>
                </div>
            </Frame>
        )
    }
}

export default ProfileComponent
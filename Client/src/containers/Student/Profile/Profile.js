import React from 'react'
import ProfileComponent from '../../../components/User/ProfileComponent/ProfileComponent'

class ProfileTeacher extends React.Component {
    state = {
        faculties: ['Информатики','Математики','Социологии'],
        groups: ['6100-010909A', '6202-020302F', '6002-010201D'],
		fields: [
			{ value: 'Студен 1', label: 'Имя пользователя', type: 'text', serverName: 'UserName', valid: true },
			{ value: 'Фамилия Имя Отчество', label: 'ФИО', type: 'text', serverName: 'FullName', valid: true },
            { value: '6100-010909A', label: 'Группа', type: 'select', serverName: 'Group', valid: true },
            { value: 'Информатики', label: 'Факультет', type: 'select', serverName: 'Faculty', valid: true },
            { value: 'user@domain.com', label: 'Адрес эл. почты', type: 'email', serverName: 'Email', valid: true }
		]
    }
    
    render() {
        return (
            <ProfileComponent 
                faculties={this.state.faculties}
                groups={this.state.groups}
                fields={this.state.fields}
            />
        )
    }
}

export default ProfileTeacher
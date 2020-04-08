import React from 'react'
import ProfileComponent from '../../../components/User/ProfileComponent/ProfileComponent'

class ProfileTeacher extends React.Component {
    state = {
        faculties: ['Информатики','Математики','Социологии'],
        departments: ['Программных систем', 'Ракетно-космической техники'],
		fields: [
			{ value: 'Преподаватель 1', label: 'Имя пользователя', type: 'text', serverName: 'UserName', valid: true },
			{ value: 'Фамилия Имя Отчество', label: 'ФИО', type: 'text', serverName: 'FullName', valid: true },
			{ value: 'Информатики', label: 'Факультет', type: 'select', serverName: 'Faculty', valid: true },
			{ value: 'Программных систем', label: 'Кафедра', type: 'select', serverName: 'Department', valid: true },
            { value: 'Заведующий кафедрой', label: 'Должность', type: 'text', serverName: 'Position', valid: true },
            { value: 'user@domain.com', label: 'Адрес эл. почты', type: 'email', serverName: 'Email', valid: true }
		]
    }
    
    render() {
        return (
            <ProfileComponent
                faculties={this.state.faculties}
                departments={this.state.departments}
                fields={this.state.fields}
            />
        )
    }
}

export default ProfileTeacher
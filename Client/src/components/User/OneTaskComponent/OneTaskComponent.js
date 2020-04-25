import React from 'react'
import './OneTaskComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Select from '../../UI/Select/Select'

class OneTaskComponent extends React.Component {
    state = {
        groupId: null,
        studentsId: []
    }

    choiceGroup = event => {
        const index = event.target.options.selectedIndex
        const groupId = event.target.options[index].getAttribute('index')

        this.setState({
            groupId
        })
    }
    
    renderMemberCreate() {
        const select = this.props.groups.map(item => {
            return (
                <option
                    key={item.id}
                    index={item.id}
                >
                    {item.name}
                </option>
            )
        })

        const users = this.state.groupId !== null ? 
            this.props.groups[this.state.groupId].students.map((item)=>{
                return (
                    <li
                        key={item.id}
                        index={item.id}
                        className='student_li'
                    >
                        <input 
                            type='checkbox' 
                            className='student_checkbox' 
                            id={`student_${item.id}`}
                        />
                        <label 
                            className='student_label' 
                            htmlFor={`student_${item.id}`}
                        >
                            <img src='/images/card.svg' alt='' />
                            {item.name}
                        </label>
                    </li>
                )
            }) : 
            null

        return (
            <Auxiliary>
                <h4>Назначить</h4>
                <Select
                    typeSelect={'create'}
                    onChangeSelect={event => this.choiceGroup(event)}
                >
                    {select}
                </Select>
                <ul>
                    {users}
                </ul>
            </Auxiliary>
        )
    }

    renderMemberTask() {
        return (
            <div>Wait..</div>
        )
    }
    
    renderMembers() {
        if (this.props.typeTask === 'create') {
            return this.renderMemberCreate()
        } else {
            return this.renderMemberTask()
        }
    }

    renderDateCreate() {
        return (
            <div>Wait create...</div>
        )
    }

    renderDateTask() {
        return (
            <div>Wait Task...</div>
        )
    }

    renderDate() {
        if (this.props.typeTask === 'create') {
            return this.renderDateCreate()
        } else {
            return this.renderDateTask()
        }
    }

    render() {
        return (
            <Frame active_index={2}>
                <div className='main_one_task'>
                    <div className='task_options'>

                    </div>
                    <aside className='aside_one_task'>
                        <div className='memebers'>
                            { this.renderMembers() }
                        </div>
                        <div className='date_create'>
                            <h4>Срок выполнения</h4>
                            { this.renderDate() }
                        </div>
                    </aside>
                </div>
            </Frame>
        )
    }
}

export default OneTaskComponent
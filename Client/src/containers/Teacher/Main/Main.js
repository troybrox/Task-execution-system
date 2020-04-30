import React from 'react'
import './Main.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import { Link } from 'react-router-dom'
import { connect } from 'react-redux'
import { fetchMain } from '../../../store/actions/teacher'
import { choiceGroupHandler, choiceSubjectHandler, choiceStudentHandler } from '../../../store/actions/teacher'

class Main extends React.Component {
    state = {
        activeSubjectIndex: null,
        activeGroupIndex: null,
        title: ''
    }

    choiceSubject = indexSubject => {
        this.props.choiceSubjectHandler(indexSubject)

        this.setState({
            activeSubjectIndex: indexSubject,
        })
    }

    choiceGroup = (indexSubject, indexGroup) => {
        this.props.choiceGroupHandler(indexSubject, indexGroup)

        const nameSubject = this.props.mainData[indexSubject].value
        const nameGroup = this.props.mainData[indexSubject].groups[indexGroup].value

        const title = nameSubject + '. Группа ' + nameGroup
        
        this.setState({
            activeSubjectIndex: indexSubject,
            activeGroupIndex: indexGroup,
            title
        })
    }

    renderMiniList(groups, indexSubject) {
        return groups.map((item, index) => {
            const cls = ['small_items']
            if (item.open) cls.push('active_small')
            return (
                <li 
                    key={index}
                    className={cls.join(' ')}
                    onClick={this.choiceGroup.bind(this, indexSubject, index)}
                >
                    <img src='images/folder-regular.svg' alt='' />
                    {item.value}
                </li>
            )
        })
    }
    
    renderList() {
        const list = this.props.mainData.map((item, index) => {
            const cls = ['big_items']
            let src = 'images/angle-right-solid.svg'
            if (item.open) {
                src = 'images/angle-down-solid.svg'
            }
            return (
                <Auxiliary key={index}>
                    <li 
                        className={cls.join(' ')}
                        onClick={() => this.choiceSubject(index)}
                    >
                        {<img src={src} alt='' />}
                        {item.value}
                    </li>

                    {item.open && 'groups' in item ? 
                        <ul className='small_list'>
                            {this.renderMiniList(item.groups, index)}
                        </ul> : null
                    }
                </Auxiliary>
            )
        })

        return (
            <ul className='big_list'>{list}</ul>
        )
    }

    renderLabs = labs => {
        return labs.map((item, index) => {
            let src = 'images/times-solid.svg'
            if (item.end) src = 'images/check-square-regular.svg'
            return (
                <div key={index} className='labs'>
                    <Link 
                        className='title'
                        to={`task/${item.id}`}
                    >
                        {item.name}
                    </Link>
                    <span className='time'>
                        <span>Открыта {item.begin}</span><br />
                        {item.end ? <span>Закрыта {item.end}</span> : null}
                    </span>
                    <img src={src} alt='' />
                </div>
            )
        })
    }

    renderStudents() {
        const indexSubject = this.state.activeSubjectIndex
        const indexGroup = this.state.activeGroupIndex
        const group = this.props.mainData[indexSubject].groups[indexGroup]

        if ('students' in group)
            return group.students.map((item, index) => {
                const cls = ['each_student']
                if (item.open) cls.push('active_student')
                return (
                    <Auxiliary key={index}>
                        <div 
                            className={cls.join(' ')} 
                            onClick={() => this.props.choiceStudentHandler(indexSubject, indexGroup, index)}
                        >
                            <img src='images/card.svg' alt='' />
                            <p>{item.name}</p>
                        </div>
                        {item.open ? this.renderLabs(item.labs) : null}
                    </Auxiliary>
                )
            })
        else 
            return null
    }

    render() {
        return (
            <Frame active_index={1}>
                <div className='value_subject'>{this.state.title}</div>
                <div className='main_subject'>
                    <aside className='aside_subject'>
                        {this.renderList()}
                    </aside>
                    <div className='persons_group'>
                        { this.state.activeGroupIndex !== null ? this.renderStudents() : null}
                    </div>
                </div>
            </Frame>
        )
    }
}

function mapStateToProps(state) {
    return {
        mainData: state.teacher.mainData
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchMain: () => dispatch(fetchMain()),
        choiceSubjectHandler: (indexSubject) => 
            dispatch(choiceSubjectHandler(indexSubject)),
        choiceGroupHandler: (indexSubject, indexGroup) => 
            dispatch(choiceGroupHandler(indexSubject, indexGroup)),
        choiceStudentHandler: (indexSubject, indexGroup, indexStudent) => 
            dispatch(choiceStudentHandler(indexSubject, indexGroup, indexStudent)),
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Main)
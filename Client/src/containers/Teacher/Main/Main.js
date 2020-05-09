import React from 'react'
import './Main.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Loader from '../../../components/UI/Loader/Loader'
import { Link } from 'react-router-dom'
import { connect } from 'react-redux'
import { fetchMain } from '../../../store/actions/teacher'
import { choiceGroupMain, choiceSubjectMain, choiceStudentHandler } from '../../../store/actions/teacher'

class Main extends React.Component {
    state = {
        activeSubjectIndex: 0,
        activeGroupIndex: 0,
        title: ''
    }

    choiceSubject = indexSubject => {
        this.props.choiceSubjectMain(indexSubject)
    }

    choiceGroup = (indexSubject, indexGroup) => {
        this.props.choiceGroupMain(indexSubject, indexGroup)

        const nameSubject = this.props.mainData[indexSubject].name
        const nameGroup = this.props.mainData[indexSubject].groups[indexGroup].name

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
                    {item.name}
                </li>
            )
        })
    }
    
    renderList() {
        const list = this.props.mainData.length === 0 ? 
            <p className='empty_field'>
                <Link to='/create_task'>Создайте задачу</Link>,
                чтобы видеть предметы и группы по созданным задачам
            </p> : 
            this.props.mainData.map((item, index) => {
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
                            {item.name}
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
            <ul className='big_list'>{this.props.loading ? <Loader /> : list}</ul>
        )
    }

    renderTasks = (tasks, solution) => {
        return tasks.map((item, index) => {
            let src = 'images/times-solid.svg'
            
            if (solution !== null) 
                src = 'images/check-square-regular.svg'
        
            return (
                <div key={index} className='tasks'>
                    <Link 
                        className='title'
                        to={`tasks/${item.id}`}
                    >
                        {item.name}
                    </Link>
                    <span className='time'>
                        <span>Открыта {item.beginDate}</span><br />
                        {!item.isOpen ? <span>Закрыта {item.finishDate}</span> : null}
                    </span>
                    <img src={src} alt='' />
                </div>
            )
        })
    }

    renderStudents() {
        const indexSubject = this.state.activeSubjectIndex
        const indexGroup = this.state.activeGroupIndex
        let group

        if (this.props.mainData.length !== 0)
            if ('groups' in this.props.mainData[indexSubject])
                group = this.props.mainData[indexSubject].groups[indexGroup]
            else
                return null
                
        else 
            return null
            
        const students = 'students' in group ? 
            group.students.map((item, index) => {
                const cls = ['each_student']
                if (item.open) cls.push('active_student')
                return (
                    <Auxiliary key={index}>
                        <div 
                            className={cls.join(' ')} 
                            onClick={() => this.props.choiceStudentHandler(indexSubject, indexGroup, index)}
                        >
                            <img src='images/card.svg' alt='' />
                            <p>{item.name} {item.surname} <span>({item.tasks.length})</span></p>
                        </div>
                        {item.open ? this.renderTasks(item.tasks, item.solution) : null}
                    </Auxiliary>
                )
            }) : null

            return (
                this.props.loading ? <Loader /> : students
            )
    }

    async componentDidMount() {
        await this.props.fetchMain()
        
        let activeSubjectIndex = null
        let activeGroupIndex = null
        let title = ''

        if (this.props.mainData.length !== 0)
            this.props.mainData.forEach((element, num) => {
                if (num === 0)
                    if ('groups' in element) {
                        activeSubjectIndex = 0
                        activeGroupIndex = 0
                        title = element.name + '. Группа ' + element.groups[0].name
                    }
            })


        this.setState({
            activeSubjectIndex,
            activeGroupIndex,
            title
        })
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
        mainData: state.teacher.mainData,
        loading: state.teacher.loading
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchMain: () => dispatch(fetchMain()),
        choiceSubjectMain: (indexSubject) => 
            dispatch(choiceSubjectMain(indexSubject)),
        choiceGroupMain: (indexSubject, indexGroup) => 
            dispatch(choiceGroupMain(indexSubject, indexGroup)),
        choiceStudentHandler: (indexSubject, indexGroup, indexStudent) => 
            dispatch(choiceStudentHandler(indexSubject, indexGroup, indexStudent)),
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Main)
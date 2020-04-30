import React from 'react'
import './TasksComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'
import Button from '../../UI/Button/Button'
import { Link } from 'react-router-dom'
import Select from '../../UI/Select/Select'

class TasksComponent extends React.Component {
    state = {
        title: '',
        tabTitles: [
            {title: 'Существующие', active: true}, 
            {title: 'Заявки', active: false}
        ],
    }

    

    choiceSubject = (value, index) => {
        this.props.choiceSubject(index)

        const title = value
        this.setState({title})
    }

    renderList() {
        const list = this.props.subjects.map((item, index) => {
            const cls = ['big_items']
            let src = 'images/folder-regular.svg'
            if (item.open) {
                cls.push('active_big')
            }
            return (
                <Auxiliary key={index}>
                    <li 
                        className={cls.join(' ')}
                        onClick={() => this.choiceSubject(item.value, index)}
                    >
                        {<img src={src} alt='' />}
                        {item.value}
                    </li>
                </Auxiliary>
            )
        })

        return (
            <ul className='big_list'>{list}</ul>
        )
    }

    renderLabs() {
        const subject = this.state.title.split(' ')
        return this.props.labs.map((item, index) => {
            return (
                    <Link
                        to={`/tasks/${index}`}
                        key={index}
                        className='each_labs' 
                    >
                        <div className='labs_left'>
                            <span className='subject_for_lab'>{subject[0]}</span>
                            <span>{item.name}</span><br />
                            <span className='small_text'>Открыта {item.lastOpen[0]} назад {item.lastOpen[1]}</span>
                        </div>
                        <div className='labs_right'>
                            <img src='images/comment-regular.svg' alt='' />
                            <span>{item.countComments}</span>
                        </div>
                    </Link>
            )
        })
    }

    changeTab = index => {
        const tabTitles = [...this.state.tabTitles]
        tabTitles.forEach(el => {
            el.active = false
        })
        tabTitles[index].active = true

        this.setState({
            tabTitles,
        }, () => {
            // this.requestUserHandler()
            // this.requestListHandler()
            console.log('swap')
        })
    }

    renderTab() {
        return this.state.tabTitles.map((item, index) => {
            const cls = ['tab']
            if (item.active) cls.push('active_tab')
            return (
                <h4
                    key={index}
                    className={cls.join(' ')}
                    onClick={this.changeTab.bind(this, index)}
                >
                    {item.title}
                </h4>
            )
        })
    }
    
    render() {
        const main = (
            <div className='labs_group'>
                <div className='nav'>
                    { this.renderTab() }
                </div> 
                
                <div className='search'>
                    <input type='search' placeholder='Поиск...' />
                    <Button 
                        // onClickButton={this.searchHandler}
                        typeButton='grey'
                        value='Поиск'
                    />
                </div>

                <div className='some_functions'>
                    <div className='sort'>
                        <span className='small_text'>Сортировать по</span>
                        <Select
                            onChangeSelect={console.log('ok')}
                        >
                            <option>Лабораторная работа</option>
                            <option>Контрольная работа</option>
                            <option>Домашняя работа</option>
                        </Select>
                    </div>
                    <div className='new_task'>
                        <Link
                            to={'/create_task'}
                        >
                            <Button 
                                typeButton='blue'
                                value='Новая задача'
                            />
                        </Link>
                    </div>
                </div>
            
                {this.renderLabs()}
            </div>
        )

        return (
            <Frame active_index={2}>
                <div className='value_subject'>{this.state.title}</div>
                <div className='main_subject'>
                    <aside className='aside_subject'>
                        {this.renderList()}
                    </aside>                       
                    { this.state.title ? main : null}
                </div>
            </Frame>
        )
    }
}

export default TasksComponent
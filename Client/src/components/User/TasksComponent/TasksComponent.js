import React from 'react'
import './TasksComponent.scss'
import Frame from '../../../hoc/Frame/Frame'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'

class TisksComponent extends React.Component {
    state = {
        title: ''
    }

    choiceSubject = (value, index) => {
        this.props.choiceSubjectHandler(index)

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
                        onClick={this.choiceSubject.bind(this, item.value, index)}
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
                <div 
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
                </div>
            )
        })
    }
    
    render() {
        const main = (
            <div className='labs_group'>
                <div className='search'>
                    <input type='search' placeholder='Поиск...' />
                    <button 
                        // onClick={this.searchHandler}
                    >
                        Поиск
                    </button>
                </div>

                <div className='some_functions'>
                    <div className='sort'>
                        <span className='small_text'>Сортировать по</span>
                        <select>
                            <option>Лабораторная работа</option>
                            <option>Контрольная работа</option>
                            <option>Домашняя работа</option>
                        </select>
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

export default TisksComponent
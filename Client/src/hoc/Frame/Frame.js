import React from 'react'
import './Frame.scss'
import Error from '../../components/Error/Error'
import { Link } from 'react-router-dom'
import { connect } from 'react-redux'
import { errorWindow } from '../../store/actions/teacher'

// Компонент высшего порядка - отображение хедера для страниц препода и студента
class Frame extends React.Component {
    state = {
        headerItems: [
            {title: 'Профиль', img: '/images/user-regular.svg', path: '/profile', show: true},
            {title: 'Главная', path: '/', show: true},
            {title: 'Задачи', img: '/images/file-alt-regular.svg', path: '/tasks', show: true},
            {title: 'Репозиторий', img: '/images/book-solid.svg', path: '/repository', show: true},
        ]
    }

    renderHeader() {
        const headerItems = [...this.state.headerItems]
        if (this.props.role === 'student')
            headerItems.forEach(el => {
                if (el.title === 'Главная') el.show = false
            })
        
        return headerItems.map((item, index) => {
            if (!item.show) return null
            const cls = ['frame_header_items']
            if (this.props.active_index === index) cls.push('active_frame_header')
            return (
                <Link
                    key={index}
                    className={cls.join(' ')}
                    to={item.path}
                >
                    <span>{item.title}</span>
                    {item.img ? <img src={item.img} alt='' /> : null}
                </Link>
            )
        })
    }

    render() {
        return (
            <div className='frame'>
                { this.props.errorShow ?
                    <Error
                        errorMessage={this.props.errorMessage}
                        errorWindow={() => this.props.errorWindow(false, [])}
                    /> : 
                    null
                }
                <header className='frame_header'>
                    {this.renderHeader()}
                    <Link className='frame_header_items exit_frame' to='/logout'>Выход</Link>
                </header>

                <main>
                    {this.props.children}
                </main>

            </div>
        )
    }
}

function mapStateToProps(state) {
    return {
        errorShow: state.teacher.errorShow,
        errorMessage: state.teacher.errorMessage,
        role: state.auth.role
    }
}

function mapDispatchToProps(dispatch) {
    return {
        errorWindow: (errorShow, errorMessage) => dispatch(errorWindow(errorShow, errorMessage))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Frame)
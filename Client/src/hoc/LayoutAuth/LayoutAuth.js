import React from 'react'
import './LayoutAuth.scss'
import {NavLink} from 'react-router-dom'
import Auxiliary from '../Auxiliary/Auxiliary'

class LayoutAuth extends React.Component {
    submitHandler = event => {
        event.preventDefault()
    }

    // Рендерим компонент аутентификации
    // так же используем NavLink из библиотеки реакта для роутинга
    render() {
        return (
            <Auxiliary>
                <header className='header'>
                    <NavLink to={this.props.to} className='top_link'>
                        <img src={this.props.img} alt={this.props.hTitle}/>
                        <span>{this.props.head}</span>
                    </NavLink>
                </header>
            
                <main className="form_box">
                    <h2>{this.props.hTitle}</h2>

                    <form onSubmit={this.submitHandler}>
                        {this.props.children}
                    </form>

                    <NavLink to={this.props.to} className='link_registration'>
                        {this.props.link}
                    </NavLink>
                </main>
            </Auxiliary>
        )
    }
}

export default LayoutAuth
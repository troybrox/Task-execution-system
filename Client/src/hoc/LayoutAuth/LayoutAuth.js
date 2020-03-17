import React from 'react'
import './LayoutAuth.scss'
import {NavLink} from 'react-router-dom'
import Auxiliary from '../Auxiliary/Auxiliary'

class LayoutAuth extends React.Component {
    render() {
        return (
        <Auxiliary>
            <div className='header'>
                <NavLink to={this.props.to} className='top_link'>
                    <img src={this.props.img} />
                    <span>{this.props.head}</span>
                </NavLink>
            </div>
    
            <div className="form_box">
                <h2>{this.props.hTitle}</h2>
                
                <form>
                    {this.props.children}
                </form>

                <NavLink to={this.props.to} className='link_registration'>
                    {this.props.link}
                </NavLink>
            </div>
        </Auxiliary>
        )
    }
}

export default LayoutAuth
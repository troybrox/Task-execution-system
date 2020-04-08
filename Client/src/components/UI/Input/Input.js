import React from 'react'
import './Input.scss'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'

const Input = props => {
    const cls = ['input_fields']

    if (!props.valid) cls.push('invalid')
    if (props.classUser) cls.push('hide')
    
    return (
        <Auxiliary>
            <input
                className={cls.join(' ')}
                type={props.type}
                value={props.value}
                onChange={props.onChange}
            />
            {props.classUser ? <img className='pen' src='images/pen-square-solid.svg' alt='' /> : null}
            <br />
        </Auxiliary>

    )
}

export default Input
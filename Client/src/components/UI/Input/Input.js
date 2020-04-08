import React from 'react'
import './Input.scss'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'

const Input = props => {
    const cls = ['input_fields']

    if (!props.valid) cls.push('invalid')
    
    return (
        <Auxiliary>
            <input
                className={cls.join(' ')}
                type={props.type}
                value={props.value}
                onChange={props.onChange}
            /><br />
        </Auxiliary>

    )
}

export default Input
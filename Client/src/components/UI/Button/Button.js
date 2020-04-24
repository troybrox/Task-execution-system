import React from 'react'
import './Button.scss'

const Button = props => {
    const cls = ['button']

    switch (props.typeButton) {
        case 'auth':
            cls.push('auth_button')
            break;
        case 'blue':
            cls.push('active_blue_button')
            break;
        case 'grey':
            cls.push('active_grey_button')
            break;
        case 'disactive':
            cls.push('disactive_button')
            break;
        default:
            break;
    }
    
    return (
        <button
            className={cls.join(' ')}
            disabled={props.typeButton === 'disactive'}
            onClick={props.onClickButton}
        >
            {props.value}
        </button>
    )
}

export default Button
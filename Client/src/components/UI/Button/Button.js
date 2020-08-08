import React from 'react'
import './Button.scss'

// Компонент отображения кнопок разных видов
const Button = props => {
    const cls = ['button']
    let disable = false

    switch (props.typeButton) {
        case 'auth':
            cls.push('auth_button')
            break;
        case 'blue':
            cls.push('active_blue_button')
            break;
        case 'blue_big':
            cls.push('active_blue_button', 'big_button')
            break;
        case 'grey':
            cls.push('active_grey_button')
            break;
        case 'disactive':
            cls.push('disactive_button')
            disable = true
            break;
        case 'disactive_big':
            cls.push('disactive_button', 'big_button')
            disable = true
            break;
        case 'close':
            cls.push('close_button')
            break;
        case 'download':
            cls.push('download')
            break;
        case 'delete':
            cls.push('delete_button')
            break;
        default:
            break;
    }
    
    return (
        <button
            className={cls.join(' ')}
            disabled={disable}
            onClick={props.onClickButton}
        >
            {props.value}
            {props.typeButton === 'download' ? <img src='/images/download-solid.svg' alt='' /> : null}
            {props.children}
        </button>
    )
}

export default Button
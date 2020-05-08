import React from 'react'
import './Select.scss'

// Компонент выпадающего меню
const Select = props => {
    const cls = ['select']

    switch (props.typeSelect) {
        case 'blue':
            cls.push('blue_select')
            break;
        case 'create':
            cls.push('create_select')
            break;
        default:
            break;
    }

    return (
        <select 
            className={cls.join(' ')}
            onChange={props.onChangeSelect}
            required
        >
            {props.children}
        </select>
    )
}

export default Select
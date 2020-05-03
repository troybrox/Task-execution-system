import React from 'react'
import Auxiliary from '../../../hoc/Auxiliary/Auxiliary'

//  Компонент отображения название для поля ввода
const Label = props => {
    return (
        <Auxiliary>
            <label className='label'>
                {props.label}
            </label><br />
        </Auxiliary>
    )
}

export default Label
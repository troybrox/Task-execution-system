import React from 'react'
import './Loader.scss'

// Компонент лоадера
const Loader = () => (
    <div className="center_loader">
        <div className="lds-ellipsis">
            <div></div>
            <div></div>
            <div></div>
            <div></div>
        </div>
    </div>
)

export default Loader
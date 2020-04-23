import React from 'react'
import './Loader.scss'

const Loader = () => (
    <div className="center_loader">
        <div class="lds-ellipsis">
            <div></div>
            <div></div>
            <div></div>
            <div></div>
        </div>
    </div>
)

export default Loader
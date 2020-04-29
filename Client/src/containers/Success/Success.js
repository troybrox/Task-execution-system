import React from 'react'
import './Success.scss'
import { connect } from 'react-redux'

class Success extends React.Component {
    // backHandler = () => {
    //     window.history.back()
    // }
    
    render() {
        return (
            <div className='success'>
                <h2>{this.props.title}</h2>
                <main>
                    <p>
                        {this.props.message}
                    </p>
                    <div>
                        <button
                            className='link_return'
                            onClick={this.backHandler}
                        >
                            Назад
                        </button>

                        <button
                            className='link_return'
                            onClick={this.onAuthContinue}
                        >
                            Вход
                        </button>
                    </div>
                </main>
            </div>
        )
    }
}

function mapStateToProps(state) {
    return {
        title: state.auth.title,
        message: state.auth.message 
    }
}

export default connect(mapStateToProps)(Success)
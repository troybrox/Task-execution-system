import React from 'react'
import OneTaskComponent from '../../../components/User/OneTaskComponent/OneTaskComponent'
// import { connect } from 'react-redux'
// import {fetchTaskById} from '../../store/actions/student'

class TasksComponent extends React.Component {    
    // componentDidMount() {
    //     this.props.fetchTaskById(this.props.match.params.id)
    // }

    render() {
        return (
            <OneTaskComponent>
                <div>Оппаааа</div>
            </OneTaskComponent>
        )
    }
}

// function mapStateToProps(state) {

// }

// function mapDispatchToProps(dispatch) {
//     return {
//         fetchTaskById: id => dispatch(fetchTaskById(id))
//     }
// }

// export default connect(mapStateToProps, mapDispatchToProps)(TasksComponent)
export default TasksComponent
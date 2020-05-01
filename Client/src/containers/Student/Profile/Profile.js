import React from 'react'
import ProfileComponent from '../../../components/User/ProfileComponent/ProfileComponent'
import { connect } from 'react-redux'
import { fetchProfile, onChangeProfile, updateProfile, updateData } from '../../../store/actions/student'

class ProfileTeacher extends React.Component {
    componentDidMount() {
        this.props.fetchProfile()
    }
    
    render() {
        return (
            <ProfileComponent 
                fields={this.props.profileData}
                onChangeProfile={this.props.onChangeProfile}
                updateData={this.props.updateData}
                updateProfile={this.props.updateProfile}
            />
        )
    }
}

function mapStateToProps(state) {
    return {
        profileData: state.student.profileData
    }
}

function mapDispatchToProps(dispatch) {
    return {
        fetchProfile: () => dispatch(fetchProfile()),
        onChangeProfile: (value, index) => dispatch(onChangeProfile(value, index)),
        updateProfile: () => dispatch(updateProfile()),
        updateData: (data, path) => dispatch(updateData(data, path))
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(ProfileTeacher)
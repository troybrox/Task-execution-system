import axios from 'axios'

export function fetchTaskById(taskId) {
    return async dispatch => {
        // dispatch() ...loading

        try {
            const url = ''
            const response = await axios.post(url, taskId)
            const data = response.data
            if (data.succeeded) {
                // dispatch() ...success
            } else {
                // dispatch() ...error
            }

        } catch (e) {
            // dispatch() ...error
        }
    }
}
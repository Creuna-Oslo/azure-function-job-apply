namespace JobApplications

module Application =
    type Config = {
        slackUrl: string
        viewUrl: string
    }

     type InputModel = {
        name: string
        contact: string
        message: string
    }
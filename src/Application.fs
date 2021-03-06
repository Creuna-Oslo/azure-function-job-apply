namespace JobApplications

module Application =

    type ViewUrl = ViewUrl of string
    type SlackUrl = SlackUrl of string

    type Config = {
        slackUrl: SlackUrl
        viewUrl: ViewUrl
    }

    type ConfigData = {
        slackUrl: string
        viewUrl: string
    }

    let mkConfig (configData: ConfigData): Config = { slackUrl = SlackUrl configData.slackUrl;
                                                      viewUrl = ViewUrl configData.viewUrl
                                                    }

    type InputModel = {
        name: string
        contact: string
        message: string
        adText: string
    }

    let toSlack (input: InputModel) (ViewUrl viewUrl) fileName: string =
        sprintf "A new job application! \n title: %s \n name: %s \n contact: %s \n message: %s \n <%s%s|View Application>!"
            input.adText
            input.name
            input.contact
            input.message
            viewUrl
            fileName

    let toHtml (input: InputModel): string =
        sprintf """ <!doctype html> <html lang=en> <head> <meta charset=utf-8> <title>Application</title> </head> <body>
            <h1>an application</h1>
            <p>title: %s</p>
            <p>name: %s</p>
            <p>contact: %s</p>
            <p>message: %s</p>""" input.adText input.name input.contact input.message

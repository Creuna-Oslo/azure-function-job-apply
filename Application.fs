namespace JobApplications

open Chiron
module Application =

    type ViewUrl = ViewUrl of string
    type SlackUrl = SlackUrl of string

    type Config = {
        slackUrl: SlackUrl
        viewUrl: ViewUrl
    }

    type InputModel = {
        name: string
        contact: string
        message: string
    }
    let toSlack (input: InputModel) (ViewUrl viewUrl) fileName: string = 
        sprintf "A new job application! \n name: %s \n contact: %s \n message: %s \n <%s%s|View Application>!" 
            input.name 
            input.contact 
            input.message 
            viewUrl 
            fileName
    let toHtml (input: InputModel): string = 
        sprintf """ <!doctype html> <html lang=en> <head> <meta charset=utf-8> <title>Application</title> </head> <body> 
            <h1>an application</h1>
            <p>name: %s</p>
            <p>contact: %s</p>
            <p>message: %s</p>""" input.name input.contact input.message
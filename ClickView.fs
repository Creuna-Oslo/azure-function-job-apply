namespace JobApplications

open Microsoft.Azure.WebJobs.Host
open System.IO
open System.Net
open System.Net.Http
open System.Text

module ClickView= 
    let run (log: TraceWriter) (req: HttpRequestMessage)  (blob : Stream) (name: string) =
        log.Info("clickView! entry!")
        async {
            log.Info("clickView! async!")
            let! data = Lib.decodeStream<Application.InputModel>(blob)
            let html = sprintf """ <!doctype html> <html lang=en> <head> <meta charset=utf-8> <title>Application</title> </head> <body> 
            <h1>an application</h1>
            <p>name: %s</p>
            <p>contact: %s</p>
            <p>message: %s</p>""" data.name data.contact data.message
            log.Info(html)
            let response = req.CreateResponse(HttpStatusCode.OK)
            response.Content <- new StringContent(html, Encoding.UTF8, "text/html")
            return response
        }
        |> Async.RunSynchronously
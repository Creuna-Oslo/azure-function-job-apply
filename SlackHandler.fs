namespace JobApplications

open FSharp.Data
open Microsoft.Azure.WebJobs.Host
open System.IO

module SlackHandler =
    let run (log: TraceWriter) (blob: Stream) (fileName: string) (config: Application.Config) =
        async {            
            log.Info "running slackhandler"
            let! input = Lib.decodeStream<Application.InputModel>(blob)
            let slackMessage = sprintf "A new job application! \n name: %s \n contact: %s \n message: %s \n <%s%s|View Application>!" 
                                input.name 
                                input.contact 
                                input.message 
                                config.viewUrl 
                                fileName
            let! res =  Http.AsyncRequest(
                          config.slackUrl,
                          headers = [ HttpRequestHeaders.ContentType HttpContentTypes.Json ],
                          httpMethod = "POST",
                          body = TextRequest (sprintf "{\"text\": \"%s\"} " slackMessage))
            match res.StatusCode with
            | 200 -> log.Info "succesfull slack post!" 
            | 304 -> failwith "succesfull, but not modified??"
            | 404 -> failwith "Not found"
            | 400 -> failwith "Bad request"
            | 500 -> failwith "Error in view function"
            | _ -> failwith "something is quite wrong"

        } |> Async.RunSynchronously
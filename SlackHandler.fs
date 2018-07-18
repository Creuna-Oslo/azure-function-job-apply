namespace JobApplications

open FSharp.Data
open Microsoft.Azure.WebJobs.Host
open System.IO
open Application

module SlackHandler =
    let run (log: TraceWriter) (blob: Stream) (fileName: string) (config: Config): unit =
        async {
            log.Info "running slackhandler"
            let! input = Lib.decodeStream<InputModel>(blob)
            let! res =  match config.slackUrl with
                        | SlackUrl s -> Http.AsyncRequest(
                                            s,
                                            headers = [ HttpRequestHeaders.ContentType HttpContentTypes.Json ],
                                            httpMethod = "POST",
                                            body = TextRequest (sprintf "{\"text\": \"%s\"} " 
                                                        <| toSlack input config.viewUrl fileName))
            match res.StatusCode with
            | 200 -> log.Info "succesfull slack post!" 
            | 304 -> failwith "succesfull, but not modified??"
            | 404 -> failwith "Not found"
            | 400 -> failwith "Bad request"
            | 500 -> failwith "Error in view function"
            | _ -> failwith "something is quite wrong"
        } |> Async.RunSynchronously
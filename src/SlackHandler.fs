namespace JobApplications

open FSharp.Data
open Microsoft.Azure.WebJobs.Host
open System.IO
open Application

module SlackHandler =
    let run (log: TraceWriter) (blob: Stream) (fileName: string) (config: Config): unit =
        async {
            log.Info "running slackhandler"
            match blob with
            | null -> failwith <| "No such blob: " + fileName
            | input -> let! data = Lib.decodeStream<InputModel>(input)
                       match data with
                       | Ok input -> let! res = match config.slackUrl with
                                                | SlackUrl s -> Http.AsyncRequest(
                                                                    s,
                                                                    headers = [ HttpRequestHeaders.ContentType HttpContentTypes.Json ],
                                                                    httpMethod = "POST",
                                                                    body = TextRequest (sprintf "{\"text\": \"%s\"} "
                                                                               <| toSlack input config.viewUrl fileName))
                                     match res.StatusCode with
                                     | 200 -> log.Info "succesful slack post!"
                                     | 404 -> failwith "Not found"
                                     | 400 -> failwith "Bad request"
                                     | 500 -> failwith "Slack error"
                                     | _ -> failwith "something is quite wrong"
                       | Error e -> failwith <| "decoding error " + e.ToString()
                    } |> Async.RunSynchronously

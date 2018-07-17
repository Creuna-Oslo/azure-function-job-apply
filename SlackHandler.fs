namespace JobApplications

open FSharp.Data
open FSharp.Data.HttpRequestHeaders
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
            let res = Http.RequestString ( config.slackUrl, headers = [ ContentType HttpContentTypes.Json ], 
                        body = TextRequest (sprintf "{\"text\": \"%s\"} " slackMessage))
            log.Info("result" + res)

        } |> Async.RunSynchronously
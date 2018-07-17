namespace JobApplications

open Microsoft.Azure.WebJobs.Host
open System.IO
open Newtonsoft.Json

module SlackHandler =
    let run (blob: Stream) (log: TraceWriter) =
        async {            
            use stream = new StreamReader(blob)
            let! blob = stream.ReadToEndAsync() |> Async.AwaitTask
            let input = JsonConvert.DeserializeObject<Application.InputModel>(blob)
            log.Info(blob)
        } |> Async.RunSynchronously
namespace JobApplications

open System.Text
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs.Host
open System.IO
open Newtonsoft.Json
open System
open Application


module ApplicationHandler = 
    let run (log: TraceWriter) (req: HttpRequest)  (blob : Stream) (name: string) =
        log.Info <| "Entered applicationhandler: " + name
        async {
            let! input = Lib.decodeStream<InputModel>(req.Body)

            if (String.IsNullOrWhiteSpace input.name) || (String.IsNullOrWhiteSpace input.contact) then
                name |> sprintf "Received by input %s" |> log.Info
                return BadRequestObjectResult "Please pass a JSON object with contact and name fields" :> IActionResult
            else
                let output = JsonConvert.SerializeObject(input)
                log.Info "Received good input"
                Encoding.UTF8.GetBytes(output) |> fun bytes ->
                    blob.Write(bytes, 0, bytes.Length)
                return OkResult() :> IActionResult
        }
        |> Async.RunSynchronously
namespace JobApplications

open FsConfig
open System.Text
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs.Host
open System.IO
open Newtonsoft.Json
open System

module ApplicationHandler = 
    let run (req: HttpRequest) (log: TraceWriter) (blob : Stream) (name: string) =
        async {
            use stream = new StreamReader(req.Body)
            let! body = stream.ReadToEndAsync() |> Async.AwaitTask
            let input = JsonConvert.DeserializeObject<Application.InputModel>(body)

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
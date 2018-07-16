namespace MyFunctions

open FsConfig
open System.Text
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Blob
open FSharp.Data.HttpRequestHeaders
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Mvc
open Microsoft.Azure.WebJobs.Host
open System.IO
open Newtonsoft.Json
open System

module Config = 
    type Config = {
        Storage : string
        SlackKey : string
    }

    let values =
         match EnvConfig.Get<Config>() with
         | Ok config -> config
         | Error error -> 
           match error with
           | NotFound envVarName -> 
             failwithf "Environment variable %s not found" envVarName
           | BadValue (envVarName, value) ->
             failwithf "Environment variable %s has invalid value %s" envVarName value
           | NotSupported msg -> 
             failwith msg
    
module HelloYou = 
    type InputModel = {
        FirstName : string
        LastName : string
    }
    exception InvalidInputException of string

    let run (req: HttpRequest) (log: TraceWriter) (blob : Stream) (name: string) =
        log.Info "[Enter] HelloYou.run"
        let config = Config.values
        async {
            use stream = new StreamReader(req.Body)
            let! body = stream.ReadToEndAsync() |> Async.AwaitTask
            let input = JsonConvert.DeserializeObject<InputModel>(body)
            if (String.IsNullOrWhiteSpace input.FirstName) || (String.IsNullOrWhiteSpace input.LastName) then
                log.Info "Received by input"
                return BadRequestObjectResult "Please pass a JSON object with a FirstName and a LastName." :> IActionResult
            else
                let output = JsonConvert.SerializeObject(input)
                log.Info "Received good input"
                let bytes = Encoding.UTF8.GetBytes(output)
                blob.Write(bytes, 0, bytes.Length)
        }
        |> Async.RunSynchronously
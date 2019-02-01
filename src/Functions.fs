namespace JobApplications

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host
open System.IO
open FsConfig
open Application

module Config =
    let getValues =
        match EnvConfig.Get<ConfigData>() with
        | Ok config -> config
        | Error error ->
         match error with
         | NotFound envVarName ->
           failwithf "Environment variable %s not found" envVarName
         | BadValue (envVarName, value) ->
           failwithf "Environment variable %s has invalid value %s" envVarName value
         | NotSupported msg ->
           failwith msg

    let values:Config = mkConfig getValues

module Functions =
    [<FunctionName("JobApplication")>]
    let JobApplication
        ([<HttpTrigger(Extensions.Http.AuthorizationLevel.Anonymous, "post", Route = "application/{name}")>]
        req: HttpRequest, name: string,
        [<Blob("creunajobapplications/{sys.randguid}", FileAccess.Write)>]
        input: Stream,
        log: TraceWriter) =
           ApplicationHandler.run log req input name

    [<FunctionName("SlackNotifier")>]
    let SlackNotifier
        ([<BlobTrigger("creunajobapplications/{fileName}")>]
        blob: Stream,
        fileName: string,
        log: TraceWriter) =
           SlackHandler.run log blob fileName Config.values

    [<FunctionName("ClickView")>]
    let ClickView
        ([<HttpTrigger(Extensions.Http.AuthorizationLevel.Anonymous, "get", Route = "application/{name}")>]
        req: System.Net.Http.HttpRequestMessage, name: string,
        [<Blob("creunajobapplications/{name}", FileAccess.Read)>]
        input: Stream,
        log: TraceWriter) =
           ClickView.run log req input name

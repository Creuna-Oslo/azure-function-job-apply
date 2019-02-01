namespace JobApplications

open System.IO
open Newtonsoft.Json

module Lib =
    let deserialize<'a> str =
        try
         JsonConvert.DeserializeObject<'a> str
         |> Result.Ok
        with
        | ex -> Result.Error ex
    let decodeStream<'a> (stream: Stream): _  = async {
        use readStream = new StreamReader(stream)
        let! data = readStream.ReadToEndAsync() |> Async.AwaitTask
        return deserialize<'a>(data) 
    }

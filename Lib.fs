namespace JobApplications

open System.IO
open Newtonsoft.Json

module Lib =
    let decodeStream<'T> (stream: Stream): Async<'T>  = async {
        use readStream = new StreamReader(stream)
        let! data = readStream.ReadToEndAsync() |> Async.AwaitTask
        return JsonConvert.DeserializeObject<'T>(data) 
    }
namespace JobApplications

open Microsoft.Azure.WebJobs.Host
open System.IO
open System.Net
open System.Net.Http
open System.Text
open Application
module ClickView=
    let run (log: TraceWriter) (req: HttpRequestMessage)  (blob : Stream) (name: string): HttpResponseMessage =
        async {
            log.Info <| "Entered ClickView"
            match blob with
            | null ->
                log.Error <| "Blob doesn't exist " + name
                return req.CreateResponse(HttpStatusCode.NotFound)
            | result ->
                let! data = Lib.decodeStream<InputModel>(result)
                match data with
                | Ok input -> let response = req.CreateResponse(HttpStatusCode.OK)
                              response.Content <- new StringContent(toHtml(input), Encoding.UTF8, "text/html")
                              return response
                | Error _ -> return req.CreateResponse(HttpStatusCode.ExpectationFailed)
        }
        |> Async.RunSynchronously

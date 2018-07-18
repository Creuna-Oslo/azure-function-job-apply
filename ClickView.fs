namespace JobApplications

open Microsoft.Azure.WebJobs.Host
open System.IO
open System.Net
open System.Net.Http
open System.Text
open Application
module ClickView= 
    let run (log: TraceWriter) (req: HttpRequestMessage)  (blob : Stream) (name: string): HttpResponseMessage =
        log.Info("entered clickView: " + name)
        async {
            let! data = Lib.decodeStream<InputModel>(blob)
            let response = req.CreateResponse(HttpStatusCode.OK)
            response.Content <- new StringContent(toHtml(data), Encoding.UTF8, "text/html")
            return response
        }
        |> Async.RunSynchronously
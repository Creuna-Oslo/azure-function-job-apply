namespace MyFunctions

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host
open System.IO

module Functions =

    [<FunctionName("HelloYou")>]
    let helloYou
        ([<HttpTrigger(Extensions.Http.AuthorizationLevel.Anonymous, "post", Route = "hello/{name}")>]
        req: HttpRequest, name: string,
        [<Blob("carlanewfunction/{name}", FileAccess.Write)>]
        input: Stream,
        log: TraceWriter) =
           HelloYou.run req log input name
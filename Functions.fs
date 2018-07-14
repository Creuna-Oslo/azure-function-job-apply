namespace MyFunctions

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host

module Functions =

    [<FunctionName("HelloYou")>]
    let helloYou
        ([<HttpTrigger(Extensions.Http.AuthorizationLevel.Anonymous, "get", Route = "hello")>]
        req: HttpRequest,
        log: TraceWriter) =
            HelloYou.run req log
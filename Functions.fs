namespace JobApplications

open Microsoft.Azure.WebJobs
open Microsoft.AspNetCore.Http
open Microsoft.Azure.WebJobs.Host
open System.IO

module Functions =
    
    [<FunctionName("JobApplication")>]
    let JobApplication
        ([<HttpTrigger(Extensions.Http.AuthorizationLevel.Anonymous, "post", Route = "application/{name}")>]
        req: HttpRequest, name: string,
        [<Blob("creunajobapplications/{name}", FileAccess.Write)>]
        input: Stream,
        log: TraceWriter) =
           ApplicationHandler.run req log input name
# Creuna-job-application
An azure function that shuffles job applications to slack and blob storage

## Getting Started

clone the repo, set `SLACK_URL` and `VIEW_URL` in `local.settings.json`.
 where `SLACK_URL` is a valid slack-hook url and `VIEW_URL` is the url to whatever handles
 generating the views, e.g. "VIEW_URL": "https://creuna-oslo-job-apply.azurewebsites.net/api/application/"
 in our case.

for local development you will want to use azure-functions-core-tools, found at 
https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local
and you should be advised to point the slack url to an app that has permission to write to your personal
slackbot. 
the `local.settings.json` could look something like this
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "SLACK_URL": "https://hooks.slack.com/services/your-long-key-goes-here",
    "VIEW_URL": "localhost:7071/api/application/",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  },
  "ConnectionStrings": {}
}
```

then all you need to do is run `dotnet restore && dotnet build && dotnet publish`, copy the local settings to the publish file (why is this a thing??) and then run 
```
func start --script-root bin\\debug\\netstandard2.0\\publish
```
### Installing
This repo uses paket. run `./paket/paket.bootstrapper.exe` to download `paket.exe` and then

```sh
paket install 
```
# Creuna-job-application
An azure function that shuffles job applications to slack and blob storage

## Getting Started

clone the repo, set `SLACK_URL` and `VIEW_URL` in `local.settings.json`.
 where `SLACK_URL` is a valid slack-hook url and `VIEW_URL` is the url to whatever handles
 generating the views, e.g. "VIEW_URL": "https://creuna-oslo-job-apply.azurewebsites.net/api/application/"
 in our case.

### Installing
This repo uses paket. run `./paket/paket.bootstrapper.exe` to download `paket.exe` and then

```sh
paket install 
```
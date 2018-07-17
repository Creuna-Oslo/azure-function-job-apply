# Creuna-job-application
An azure function that shuffles jobapplications to slack and blob storage

## Getting Started

clone the repo, set `SLACK_URL` and `VIEW_URL` in `local.settings.json`.
Currently, you also need an azure storage acocunt, `creaunajobapplications`.

### Installing
This repo uses paket. run `./paket/paket.bootstrapper.exe` to download `paket.exe` and then

```sh
paket install 
```
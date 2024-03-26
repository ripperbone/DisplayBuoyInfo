## BuoyInfoFunction

### Testing locally

Create local.settings.json file with the following contents:
```
{
  "IsEncrypted": false,
  "Values": {
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "SendGridAPIKey": "<your SendGrid API key>",
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "EMAIL_FROM_ADDRESS": "<user@somewhere.com>",
    "EMAIL_FROM_NAME": "<Your Name>",
    "EMAIL_TO_ADDRESS":  "<user@somewhere.com>"
  }
}
```

Edit TimerTrigger so that `RunOnStartup = true`.

Start azurite storage emulator.

Run the function with:
```
func start
```

### Deploying

```
func azure functionapp publish <FunctionName> --csharp
```
# MyDomain.Api.Consumer

Demo consumer for https://github.com/matteus6007/MyDomain.Api.Template.

## Running the Application

```shell
$env:PROVIDER_URL="<PROVIDER_URL>";dotnet run --project src/MyDomain.Api.Consumer.App/MyDomain.Api.Consumer.App.csproj
```

_Note: you need to have the provider running at `<PROVIDER_URL>` before running the application._

## Testing

Run unit tests to produce `Pact` file:

```shell
$env:PACT_BROKER_BASE_URL="<PACT_BROKER_BASE_URL>";$env:PACT_BROKER_TOKEN="<PACT_BROKER_TOKEN>";dotnet test
```

## Contract Testing

### Pact

Publish consumer `Pact`:

```shell
docker run --rm -v ${pwd}:/api -w /api pactfoundation/pact-cli publish src/MyDomain.Api.Consumer.App.Tests/pacts --broker-base-url <PACT_BROKER_BASE_URL> --broker-token <PACT_BROKER_TOKEN> --consumer-app-version "1.0.0" --branch <BRANCH>
```

Current consumer version `1.0.1`.

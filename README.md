# min-jwt

## Introduction

Demonstrate the use of
- Minimal API
- JWT
- Integration tests
  - using WebApplicationFactory

## Docker
1. Navigate to `src` folder
2. To Build - `docker build -t keyboardapi -f ./KeyboardApi/Dockerfile .`
3. To Run `docker run -ti --rm -p 54623:80 keyboardapi`

## Local configuration
`appsettings.local.json`
- add key `hK4*bnvjfBaXC2!1Ldg3IvnW69AQPq`

## Troubleshooting

### Certificate error on starting service
Unhandled exception. System.InvalidOperationException: Unable to configure HTTPS endpoint. No server certificate was specified, and the default developer certificate could not be found or is out of date.

Keychain accces (mac)
System -> delete localhost

Re-open Rider and you will be prompted to setup the certificate again

Note - dotnet dev-certs https --clean does not always work
# Keyboard Hub
Is a collection of keyboard related services

Demonstrate the use of
- Minimal API
- JWT
- Integration tests
  - using WebApplicationFactory and mocks

## Swagger
Call `/login` to generate the token
Add token to swagger auth
Call Vendor endpoints

## Docker
1. Navigate to `src` folder
2. To Build - `docker build -t VendorApi -f ./VendorApi/Dockerfile .`
3. To Run `docker run -ti --rm -p 54623:80 VendorApi`

## Local configuration
`appsettings.local.json`
- add key `C1C6E14D94B32fdfasdfs57526E64D52181C61C`

## Troubleshooting

### Certificate error on starting service
Unhandled exception. System.InvalidOperationException: Unable to configure HTTPS endpoint. No server certificate was specified, and the default developer certificate could not be found or is out of date.

Keychain accces (mac)
System -> delete localhost

Re-open Rider and you will be prompted to setup the certificate again

Note - dotnet dev-certs https --clean does not always work
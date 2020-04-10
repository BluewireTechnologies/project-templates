# Bluewire.Webhooks.Service

TODO: Description.

## Usage

Runs as a service which hosts an HTTP(S) endpoint.

## Deployment

* Variable substitution should be applied to the `*.Octopus.config` transform file.
* AppSettings values should be set in the configuration file. 

### Package Parameters

* **Bluewire.Webhooks.Service:BaseUri**: The root URI on which the endpoint should be hosted.
  * Type: Uri
* **Bluewire.Webhooks.Service:LogDirectory**: The directory where the service should store its logs.
  * Type: directory path

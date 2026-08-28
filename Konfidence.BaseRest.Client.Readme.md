# Konfidence.BaseRest.Client

A client for basic REST service access, using the RestSharp client. Used by my ClassGenerator.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`BaseRestClient`** — a thin async wrapper (`GetAsync<T>`, `PostAsync<T>`) around RestSharp that JSON-serializes the request body, adds optional headers and deserializes the response into `T`
- **`IRestClientConfig` / `RestClientConfig`** — supplies the base URI the client is configured against, bound from configuration

Targets **net9.0** and **net10.0**.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).

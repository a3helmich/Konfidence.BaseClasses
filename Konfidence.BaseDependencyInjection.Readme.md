# Konfidence.BaseDependencyInjection

A single convention interface, so a library can register its own services without the host application needing to know what those services are.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`IKonfidenceDependencyInjection`** — one method, `AddServices(IServiceCollection services, IConfiguration configuration)`. A library implements it, the host discovers the implementation and calls it during startup

Targets **net9.0** and **net10.0**. Contract only, no implementation — it depends on nothing but `Microsoft.Extensions.DependencyInjection.Abstractions` and `Microsoft.Extensions.Configuration.Abstractions`.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).

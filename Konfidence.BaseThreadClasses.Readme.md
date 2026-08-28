# Konfidence.BaseThreadClasses

A basic manage-run-action pattern for simple threading. Consider using simpler Tasks before reaching for this library.

Part of the [Konfidence.BaseClasses](https://github.com/a3helmich/Konfidence.BaseClasses) collection of libraries.

## What is in it

- **`ThreadManager<TAction>`** — starts and stops a background `ThreadRunner`, exposing hooks (`SetInitializeAction`, `SetBeforeExecuteAction`, `SetAfterExecuteAction`) that run around each execution of a `ThreadAction`
- **`ThreadRunner<TAction>` / `ThreadAction`** — the underlying loop, sleeping between runs for a configurable interval
- **`SleepUnit`** — the interval unit: seconds, minutes, hourly, daily. Note that a `SleepUnit` outside those values falls back to a four second sleep, and that a sleep time of `0` means no delay at all

Targets **net9.0** and **net10.0**.

## Full documentation

The other libraries in the collection, and build/test instructions, are in the
[README on github.com](https://github.com/a3helmich/Konfidence.BaseClasses#readme).

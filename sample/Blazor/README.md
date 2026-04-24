## Blazor App

Scaffolded as `dotnet new blazor -int Auto`.

> Uses interactive server-side rendering while downloading the Blazor wasm runtime on the client, then uses client-side rendering with WebAssembly more info [here](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes).

## Run

```console
dotnet run --project .\Blazor\
```

Go to [http://localhost:5000](http://localhost:5000) and you can check out:
* Counter page - it uses a request to bump the counter with alternating handlers for each mode (server or client) visible with the count, more info on [Server Counter.cs](./Blazor/Components/Pages/Counter.cs) file
* Weather page - it uses a streaming request to update the UI

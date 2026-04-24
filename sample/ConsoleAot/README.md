## Console AOT App

Same as [Console](../Console/), but compiled with [Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/).

### Publish and run

To publish replace `<RID>` with your runtime (e.g. `win-x64` or `linux-x64`):

```bash
dotnet publish .\ConsoleAot.csproj -r win-x64 -o ./publish
```

and then to run (win-x64):

```bash
.\publish\ConsoleAot.exe
```

will result in:
```console
-----------------------------------
Run: ProgramRequests
-----------------------------------
1) Running logger handler
2) Running ping validator
2) Valid input!
3) Returning pong!
1) No error!
-----------------------------------
ID: 10
Request: Ping { Id = 10 }
Response: Pong { Id = 10 }
-----------------------------------
-----------------------------------
Run: ProgramStreams
-----------------------------------
1) Logger handler
2) Running ping validator
2) Valid input!
4) Returning 3 pongs!
3) Tagging pong
-----------------------------------
ID: 20
Request: Ping { Id = 20 }
Response: Pong { Id = 20, Tag = Tagger }
-----------------------------------
3) Tagging pong
-----------------------------------
ID: 20
Request: Ping { Id = 20 }
Response: Pong { Id = 20, Tag = Tagger }
-----------------------------------
3) Tagging pong
-----------------------------------
ID: 20
Request: Ping { Id = 20 }
Response: Pong { Id = 20, Tag = Tagger }
-----------------------------------
```

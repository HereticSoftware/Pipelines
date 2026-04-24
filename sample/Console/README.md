# Console App

Samples of:
- executing a request
- streaming responses using a streaming request (`IAsyncEnumerable`)

# Build and run

Use:

```bash
dotnet run
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

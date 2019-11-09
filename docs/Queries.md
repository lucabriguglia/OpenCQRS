# Queries

First, create a model to be returned and a query that implements the **IQuery<>** interface:

```C#
public class Something
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class GetSomething : IQuery<Something>
{
    public int Id { get; set; }
}
```

Next, create the handler:

```C#
public class GetSomethingQueryHandler : IQueryHandlerAsync<GetSomething, Something>
{
    public async Task<Something> HandleAsync(GetSomething query)
    {
        return await _db.Somethings.FirstOrDefaultAsync(x => x.Id == query.Id);
    }
}
```

And finally, get the result using the dispatcher:

```C#
var query = new GetSomething { Id = 123 };
var something = await _dispatcher.GetResultAsync(query);
```
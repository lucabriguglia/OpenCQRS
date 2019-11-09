# Without Event Sourcing

If you are using Kledex in a CQRS pattern remember that CQRS does **NOT** need Event Sourcing.

Event Sourcing functionalities of Kledex can be used just as an advanced logging system for your domain.
The CQRS pattern does not need the domain object to be recreated from history by loading all past events.
The single source of truth can be a SQL Server database with normalised data (or any other data provider) and use the read model to optimised queries.

First, create a domain object that inherits from **AggregateRoot**.
This is how a Product class might look like:

```C#
public class Product : AggregateRoot
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }

    public Product()
    {            
    }

    public Product(Guid id, string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new ApplicationException("Product title is required.");

        Id = id,
        Title = title
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new ApplicationException("Product title is required.");

        Title = title
    }
}
```

Next, create a command and an event:

```C#
public class CreateProduct : DomainCommand<Product>
{
    public string Title { get; set; }
}

public class ProductCreated : DomainEvent
{
    public string Title { get; set; }
}
```

Create the handler:

```C#
public class CreateProductHandler : ICommandHandlerAsync<CreateProduct>
{
    private readonly IProductRepository _repository;

    public CreateProductHandlerAsync (IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<CommandResponse> HandleAsync(CreateProduct command)
    {
        var product = new Product(command.AggregateId, command.Title);

        await _repository.CreateProduct(product);

        return new CommandResponse
        {
            Events = new List<IDomainEvent>()
            {
                new ProductCreated
                {
                    AggregateRootId = product.Id,
                    Title = product.Name
                }
            }
        };
    }
}
```

Send the command using the dispatcher:

```C#
var command = new CreateProduct
{
    AggregateId = Guid.NewGuid(),
    Title = "My brand new product"
};

await _dispatcher.SendAsync(command)
```

The read model(s) can be created the same way we would have done using Event Sourcing, using event handlers or better using a message bus.

The main difference here is that we are using a normal repository for saving our normalised data but at the same time we have the complete history of all the changes that happened to the domain (all events are automatically saved anyway by the dispatcher).

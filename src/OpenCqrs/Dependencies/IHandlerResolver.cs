namespace OpenCqrs.Dependencies
{
    public interface IHandlerResolver
    {
        THandler ResolveHandler<THandler>();
    }
}

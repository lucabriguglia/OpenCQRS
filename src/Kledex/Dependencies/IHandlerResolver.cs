namespace Kledex.Dependencies
{
    public interface IHandlerResolver
    {
        THandler ResolveHandler<THandler>();
    }
}

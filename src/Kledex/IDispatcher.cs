using System.Threading.Tasks;
using Kledex.Bus;
using Kledex.Commands;
using Kledex.Events;
using Kledex.Queries;

namespace Kledex
{
    /// <summary>
    /// IDispatcher
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Asynchronously sends the specified command.
        /// The command handler must implement Kledex.Commands.ICommandHandlerAsync&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task SendAsync(ICommand command);

        Task<TResult> SendAsync<TResult>(ICommand command);

        /// <summary>
        /// Asynchronously publishes the specified event.
        /// The event handler must implement Kledex.Events.IEventHandlerAsync&lt;TEvent&gt;.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        Task PublishAsync<TEvent>(TEvent @event) 
            where TEvent : IEvent;

        /// <summary>
        /// Asynchronously gets the result.
        /// The query handler must implement Kledex.Queries.IQueryHandlerAsync&lt;TQuery, TResult&gt;.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        Task<TResult> GetResultAsync<TResult>(IQuery<TResult> query);

        /// <summary>
        /// Dispatches the bus message asynchronously.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        Task DispatchBusMessageAsync<TMessage>(TMessage message) 
            where TMessage : IBusMessage;

        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement Kledex.Commands.ICommandHandler&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Send(ICommand command);

        TResult Send<TResult>(ICommand command);

        /// <summary>
        /// Publishes the specified event.
        /// The event handler must implement Kledex.Events.IEventHandler&lt;TEvent&gt;.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        void Publish<TEvent>(TEvent @event) 
            where TEvent : IEvent;

        /// <summary>
        /// Gets the result.
        /// The query handler must implement Kledex.Queries.IQueryHandler&lt;TQuery, TResult&gt;.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        TResult GetResult<TResult>(IQuery<TResult> query);
    }
}

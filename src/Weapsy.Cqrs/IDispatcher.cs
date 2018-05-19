using System.Threading.Tasks;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.Events;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Cqrs
{
    /// <summary>
    /// IDispatcher
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Asynchronously sends the specified command.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandlerAsync.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        Task SendAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the specified command.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandler.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Asynchronously sends the command and the events returned by the handler will be saved to the event store.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandlerWithAggregateAsync&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        Task SendAsync<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Sends the command and the events returned by the handler will be saved to the event store.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandlerWithAggregate&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        void Send<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Asynchronously sends the command and publishes the events returned by the command handler.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandlerWithEventsAsync&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        Task SendAndPublishAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Sends the command and publishes the events returned by the command handler.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandlerWithEvents&lt;TCommand&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command.</param>
        void SendAndPublish<TCommand>(TCommand command)
            where TCommand : ICommand;

        /// <summary>
        /// Asynchronously sends the command and the events returned by the handler will be published and saved to the event store.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandlerWithAggregateAsync&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        Task SendAndPublishAsync<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Sends the command and the events returned by the handler will be published and saved to the event store.
        /// The command handler must implement Weapsy.Cqrs.Commands.ICommandHandlerWithAggregate&lt;TCommand, TAggregate&gt;.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <typeparam name="TAggregate">The type of the aggregate.</typeparam>
        /// <param name="command">The command.</param>
        void SendAndPublish<TCommand, TAggregate>(TCommand command)
            where TCommand : IDomainCommand
            where TAggregate : IAggregateRoot;

        /// <summary>
        /// Asynchronously publishes the specified event.
        /// The event handler must implement Weapsy.Cqrs.Events.IEventHandlerAsync.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        Task PublishAsync<TEvent>(TEvent @event) 
            where TEvent : IEvent;

        /// <summary>
        /// Publishes the specified event.
        /// The event handler must implement Weapsy.Cqrs.Events.IEventHandler.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="event">The event.</param>
        void Publish<TEvent>(TEvent @event)
            where TEvent : IEvent;

        /// <summary>
        /// Asynchronously gets the result.
        /// The query handler must implement Weapsy.Cqrs.Queries.IQueryHandlerAsync.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        Task<TResult> GetResultAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery;

        /// <summary>
        /// Gets the result.
        /// The query handler must implement Weapsy.Cqrs.Queries.IQueryHandler.
        /// </summary>
        /// <typeparam name="TQuery">The type of the query.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="query">The query.</param>
        /// <returns>TResult</returns>
        TResult GetResult<TQuery, TResult>(TQuery query) 
            where TQuery : IQuery;
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Weapsy.Cqrs.Commands;
using Weapsy.Cqrs.Dependencies;
using Weapsy.Cqrs.Domain;
using Weapsy.Cqrs.Events;
using Weapsy.Cqrs.Tests.Fakes;

namespace Weapsy.Cqrs.Tests.Commands
{
    [TestFixture]
    public class CommandSenderAsyncTests
    {
        private ICommandSenderAsync _sut;

        private Mock<IResolver> _resolver;
        private Mock<IEventPublisherAsync> _eventPublisher;
        private Mock<IEventStore> _eventStore;
        private Mock<ICommandStore> _commandStore;
        private Mock<IEventFactory> _eventFactory;

        private Mock<ICommandHandlerAsync<CreateSomething>> _commandHandlerAsync;
        private Mock<ICommandHandlerWithEventsAsync<CreateSomething>> _commandHandlerWithEventsAsync;
        private Mock<ICommandHandlerWithAggregateAsync<CreateAggregate>> _commandHandlerWithAggregateAsync;

        private CreateSomething _createSomething;
        private SomethingCreated _somethingCreated;
        private SomethingCreated _somethingCreatedConcrete;
        private IEnumerable<IEvent> _events;

        private CreateAggregate _createAggregate;
        private AggregateCreated _aggregateCreated;
        private AggregateCreated _aggregateCreatedConcrete;
        private Aggregate _aggregate;

        [SetUp]
        public void SetUp()
        {
            _createSomething = new CreateSomething();
            _somethingCreated = new SomethingCreated();
            _somethingCreatedConcrete = new SomethingCreated();
            _events = new List<IEvent> { _somethingCreated };

            _createAggregate = new CreateAggregate();           
            _aggregateCreatedConcrete = new AggregateCreated();
            _aggregate = new Aggregate();
            _aggregateCreated = (AggregateCreated)_aggregate.Events[0];

            _eventPublisher = new Mock<IEventPublisherAsync>();
            _eventPublisher
                .Setup(x => x.PublishAsync(_somethingCreatedConcrete))
                .Returns(Task.CompletedTask);
            _eventPublisher
                .Setup(x => x.PublishAsync(_aggregateCreatedConcrete))
                .Returns(Task.CompletedTask);

            _eventStore = new Mock<IEventStore>();
            _eventStore
                .Setup(x => x.SaveEventAsync<Aggregate>(_aggregateCreatedConcrete))
                .Returns(Task.CompletedTask);

            _commandStore = new Mock<ICommandStore>();
            _commandStore
                .Setup(x => x.SaveCommandAsync<Aggregate>(_createAggregate))
                .Returns(Task.CompletedTask);

            _eventFactory = new Mock<IEventFactory>();
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_somethingCreated))
                .Returns(_somethingCreatedConcrete);
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_aggregateCreated))
                .Returns(_aggregateCreatedConcrete);

            _commandHandlerAsync = new Mock<ICommandHandlerAsync<CreateSomething>>();
            _commandHandlerAsync
                .Setup(x => x.HandleAsync(_createSomething))
                .Returns(Task.CompletedTask);

            _commandHandlerWithEventsAsync = new Mock<ICommandHandlerWithEventsAsync<CreateSomething>>();
            _commandHandlerWithEventsAsync
                .Setup(x => x.HandleAsync(_createSomething))
                .ReturnsAsync(_events);

            _commandHandlerWithAggregateAsync = new Mock<ICommandHandlerWithAggregateAsync<CreateAggregate>>();
            _commandHandlerWithAggregateAsync
                .Setup(x => x.HandleAsync(_createAggregate))
                .ReturnsAsync(_aggregate);

            _resolver = new Mock<IResolver>();
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerAsync<CreateSomething>>())
                .Returns(_commandHandlerAsync.Object);
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithEventsAsync<CreateSomething>>())
                .Returns(_commandHandlerWithEventsAsync.Object);
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithAggregateAsync<CreateAggregate>>())
                .Returns(_commandHandlerWithAggregateAsync.Object);

            _sut = new CommandSenderAsync(_resolver.Object, _eventPublisher.Object, _eventFactory.Object, _eventStore.Object, _commandStore.Object);
        }

        [Test]
        public void SendAsyncThrowsExceptionWhenCommandIsNull()
        {
            _createSomething = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAsync(_createSomething));
        }

        [Test]
        public void SendAsyncThrowsExceptionWhenCommandHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerAsync<CreateSomething>>())
                .Returns((ICommandHandlerAsync<CreateSomething>)null);
            Assert.ThrowsAsync<ApplicationException>(async () => await _sut.SendAsync(_createSomething));
        }

        [Test]
        public async Task SendAsyncSendsCommand()
        {
            await _sut.SendAsync(_createSomething);
            _commandHandlerAsync.Verify(x => x.HandleAsync(_createSomething), Times.Once);
        }

        [Test]
        public void SendWithAggregateAsyncThrowsExceptionWhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public void SendWithAggregateAsyncThrowsExceptionWhenCommandHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithAggregateAsync<CreateAggregate>>())
                .Returns((ICommandHandlerWithAggregateAsync<CreateAggregate>)null);
            Assert.ThrowsAsync<ApplicationException>(async () => await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public async Task SendWithAggregateAsyncSendsCommand()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandHandlerWithAggregateAsync.Verify(x => x.HandleAsync(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendWithAggregateAsyncSaveEvents()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEventAsync<Aggregate>(_aggregateCreatedConcrete), Times.Once);
        }

        [Test]
        public void SendAndPublishAsyncThrowsExceptionWhenCommandIsNull()
        {
            _createSomething = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAndPublishAsync(_createSomething));
        }

        [Test]
        public void SendAndPublishAsyncThrowsExceptionWhenCommandHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithEventsAsync<CreateSomething>>())
                .Returns((ICommandHandlerWithEventsAsync<CreateSomething>)null);
            Assert.ThrowsAsync<ApplicationException>(async () => await _sut.SendAndPublishAsync(_createSomething));
        }

        [Test]
        public async Task SendAndPublishAsyncSendsCommand()
        {
            await _sut.SendAndPublishAsync(_createSomething);
            _commandHandlerWithEventsAsync.Verify(x => x.HandleAsync(_createSomething), Times.Once);
        }

        [Test]
        public async Task SendAndPublishAsyncPublishesEvents()
        {
            await _sut.SendAndPublishAsync(_createSomething);
            _eventPublisher.Verify(x => x.PublishAsync(_somethingCreatedConcrete), Times.Once);
        }

        [Test]
        public void SendAndPublishWithAggregateAsyncThrowsExceptionWhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public void SendAndPublishWithAggregateAsyncThrowsExceptionWhenCommandHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithAggregateAsync<CreateAggregate>>())
                .Returns((ICommandHandlerWithAggregateAsync<CreateAggregate>)null);
            Assert.ThrowsAsync<ApplicationException>(async () => await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public async Task SendAndPublishWithAggregateAsyncSendsCommand()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandHandlerWithAggregateAsync.Verify(x => x.HandleAsync(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendAndPublishWithAggregateAsyncSaveEvents()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEventAsync<Aggregate>(_aggregateCreatedConcrete), Times.Once);
        }

        [Test]
        public async Task SendAndPublishWithAggregateAsyncPublishEvents()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Once);
        }
    }
}

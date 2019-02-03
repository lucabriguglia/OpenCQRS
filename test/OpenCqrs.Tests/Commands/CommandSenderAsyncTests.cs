using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using OpenCqrs.Abstractions.Commands;
using OpenCqrs.Abstractions.Events;
using OpenCqrs.Commands;
using OpenCqrs.Dependencies;
using OpenCqrs.Domain;
using OpenCqrs.Events;
using OpenCqrs.Tests.Fakes;

namespace OpenCqrs.Tests.Commands
{
    [TestFixture]
    public class CommandSenderAsyncTests
    {
        private ICommandSender _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IEventStore> _eventStore;
        private Mock<ICommandStore> _commandStore;
        private Mock<IEventFactory> _eventFactory;

        private Mock<ICommandHandlerAsync<CreateSomething>> _commandHandlerAsync;
        private Mock<ICommandHandlerWithEventsAsync<CreateSomething>> _commandHandlerWithEventsAsync;
        private Mock<ICommandHandlerWithDomainEventsAsync<CreateAggregate>> _commandHandlerWithDomainEventsAsync;

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

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher
                .Setup(x => x.PublishAsync(_somethingCreatedConcrete))
                .Returns(Task.CompletedTask);
            _eventPublisher
                .Setup(x => x.PublishAsync(_aggregateCreatedConcrete))
                .Returns(Task.CompletedTask);

            _eventStore = new Mock<IEventStore>();
            _eventStore
                .Setup(x => x.SaveEventAsync<Aggregate>(_aggregateCreatedConcrete, null))
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

            _commandHandlerWithDomainEventsAsync = new Mock<ICommandHandlerWithDomainEventsAsync<CreateAggregate>>();
            _commandHandlerWithDomainEventsAsync
                .Setup(x => x.HandleAsync(_createAggregate))
                .ReturnsAsync(_aggregate.Events);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveHandler<ICommandHandlerAsync<CreateSomething>>())
                .Returns(_commandHandlerAsync.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler<ICommandHandlerWithEventsAsync<CreateSomething>>())
                .Returns(_commandHandlerWithEventsAsync.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler<ICommandHandlerWithDomainEventsAsync<CreateAggregate>>())
                .Returns(_commandHandlerWithDomainEventsAsync.Object);

            _sut = new CommandSender(_handlerResolver.Object, _eventPublisher.Object, _eventFactory.Object, _eventStore.Object, _commandStore.Object);
        }

        [Test]
        public void SendAsync_ThrowsException_WhenCommandIsNull()
        {
            _createSomething = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAsync(_createSomething));
        }

        [Test]
        public async Task SendAsync_SendsCommand()
        {
            await _sut.SendAsync(_createSomething);
            _commandHandlerAsync.Verify(x => x.HandleAsync(_createSomething), Times.Once);
        }

        [Test]
        public void SendWithDomainEventsAsync_ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public async Task SendWithDomainEventsAsync_SavesCommand()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommandAsync<Aggregate>(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendWithDomainEventsAsync_SendsCommand()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandHandlerWithDomainEventsAsync.Verify(x => x.HandleAsync(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendWithDomainEventsAsync_SavesEvents()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEventAsync<Aggregate>(_aggregateCreatedConcrete, null), Times.Once);
        }

        [Test]
        public void SendAndPublishAsync_ThrowsException_WhenCommandIsNull()
        {
            _createSomething = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAndPublishAsync(_createSomething));
        }

        [Test]
        public async Task SendAndPublishAsync_SendsCommand()
        {
            await _sut.SendAndPublishAsync(_createSomething);
            _commandHandlerWithEventsAsync.Verify(x => x.HandleAsync(_createSomething), Times.Once);
        }

        [Test]
        public async Task SendAndPublishAsync_PublishesEvents()
        {
            await _sut.SendAndPublishAsync(_createSomething);
            _eventPublisher.Verify(x => x.PublishAsync(_somethingCreatedConcrete), Times.Once);
        }

        [Test]
        public void SendAndPublishWithDomainEventsAsync_ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public async Task SendAndPublishWithDomainEventsAsync_SendsCommand()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandHandlerWithDomainEventsAsync.Verify(x => x.HandleAsync(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendAndPublishWithDomainEventsAsync_SavesCommand()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommandAsync<Aggregate>(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendAndPublishWithDomainEventsAsync_SavesEvents()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEventAsync<Aggregate>(_aggregateCreatedConcrete, null), Times.Once);
        }

        [Test]
        public async Task SendAndPublishWithDomainEventsAsync_PublishesEvents()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Once);
        }
    }
}

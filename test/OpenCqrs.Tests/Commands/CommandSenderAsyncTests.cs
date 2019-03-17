using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using OpenCqrs.Commands;
using OpenCqrs.Dependencies;
using OpenCqrs.Domain;
using OpenCqrs.Events;
using OpenCqrs.Tests.Fakes;
using Options = OpenCqrs.Configuration.Options;

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

        private Mock<ICommandHandlerWithEventsAsync<CreateSomething>> _commandHandlerWithEventsAsync;
        private Mock<ICommandHandlerWithDomainEventsAsync<CreateAggregate>> _commandHandlerWithDomainEventsAsync;
        private Mock<IOptions<Options>> _optionsMock;

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
                .Setup(x => x.ResolveHandler<ICommandHandlerWithEventsAsync<CreateSomething>>())
                .Returns(_commandHandlerWithEventsAsync.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler<ICommandHandlerWithDomainEventsAsync<CreateAggregate>>())
                .Returns(_commandHandlerWithDomainEventsAsync.Object);

            _optionsMock = new Mock<IOptions<Options>>();
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options());

            _sut = new CommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _eventStore.Object,
                _commandStore.Object,
                _optionsMock.Object);
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
            _commandHandlerWithEventsAsync.Verify(x => x.HandleAsync(_createSomething), Times.Once);
        }

        [Test]
        public async Task SendAsync_PublishesEvents()
        {
            await _sut.SendAsync(_createSomething);
            _eventPublisher.Verify(x => x.PublishAsync(_somethingCreatedConcrete), Times.Once);
        }

        [Test]
        public void SendWithDomainEventsAsync_ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public async Task SendWithDomainEventsAsync_SendsCommand()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandHandlerWithDomainEventsAsync.Verify(x => x.HandleAsync(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendWithDomainEventsAsync_SavesCommand()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommandAsync<Aggregate>(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendWithDomainEventsAsync_NotSavesCommand_WhenSetInOptions()
        {
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options { SaveCommands = false });

            _sut = new CommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _eventStore.Object,
                _commandStore.Object,
                _optionsMock.Object);

            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommandAsync<Aggregate>(_createAggregate), Times.Never);
        }

        [Test]
        public async Task SendWithDomainEventsAsync_NotSavesCommand_WhenSetInCommand()
        {
            _createAggregate.SaveCommand = false;
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommandAsync<Aggregate>(_createAggregate), Times.Never);
        }

        [Test]
        public async Task SendWithDomainEventsAsync_SavesEvents()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEventAsync<Aggregate>(_aggregateCreatedConcrete, null), Times.Once);
        }

        [Test]
        public async Task SendWithDomainEventsAsync_PublishesEvents()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Once);
        }

        [Test]
        public async Task SendWithDomainEventsAsync_NotPublishesEvents_WhenSetInOptions()
        {
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options { PublishEvents = false });

            _sut = new CommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _eventStore.Object,
                _commandStore.Object,
                _optionsMock.Object);

            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Never);
        }

        [Test]
        public async Task SendWithDomainEventsAsync_NotPublishesEvents_WhenSetInCommand()
        {
            _createAggregate.PublishEvents = false;
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Never);
        }
    }
}

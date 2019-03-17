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

namespace OpenCqrs.Tests.Domain
{
    [TestFixture]
    public class DomainCommandSenderAsyncTests
    {
        private IDomainCommandSender _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IEventStore> _eventStore;
        private Mock<ICommandStore> _commandStore;
        private Mock<IAggregateStore> _aggregateStore;
        private Mock<IEventFactory> _eventFactory;

        private Mock<ICommandHandlerAsync<CreateSomething>> _commandHandlerAsync;
        private Mock<IDomainCommandHandlerAsync<CreateAggregate>> _domainCommandHandlerAsync;
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

            _aggregateStore = new Mock<IAggregateStore>();
            _aggregateStore
                .Setup(x => x.SaveAggregateAsync<Aggregate>(_createAggregate.AggregateRootId))
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
                .ReturnsAsync(_events);

            _domainCommandHandlerAsync = new Mock<IDomainCommandHandlerAsync<CreateAggregate>>();
            _domainCommandHandlerAsync
                .Setup(x => x.HandleAsync(_createAggregate))
                .ReturnsAsync(_aggregate.Events);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveHandler<ICommandHandlerAsync<CreateSomething>>())
                .Returns(_commandHandlerAsync.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler<IDomainCommandHandlerAsync<CreateAggregate>>())
                .Returns(_domainCommandHandlerAsync.Object);

            _optionsMock = new Mock<IOptions<Options>>();
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options());

            _sut = new DomainCommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _aggregateStore.Object,
                _commandStore.Object,
                _eventStore.Object,
                _optionsMock.Object);
        }

        [Test]
        public void SendAsync__ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public async Task SendAsync__SendsCommand()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _domainCommandHandlerAsync.Verify(x => x.HandleAsync(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendAsync__SavesAggregate()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _aggregateStore.Verify(x => x.SaveAggregateAsync<Aggregate>(_createAggregate.AggregateRootId), Times.Once);
        }

        [Test]
        public async Task SendAsync__SavesCommand()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommandAsync<Aggregate>(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendAsync__NotSavesCommand_WhenSetInOptions()
        {
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options { SaveCommands = false });

            _sut = new DomainCommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _aggregateStore.Object,
                _commandStore.Object,
                _eventStore.Object,
                _optionsMock.Object);

            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommandAsync<Aggregate>(_createAggregate), Times.Never);
        }

        [Test]
        public async Task SendAsync__NotSavesCommand_WhenSetInCommand()
        {
            _createAggregate.SaveCommand = false;
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommandAsync<Aggregate>(_createAggregate), Times.Never);
        }

        [Test]
        public async Task SendAsync__SavesEvents()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEventAsync<Aggregate>(_aggregateCreatedConcrete, null), Times.Once);
        }

        [Test]
        public async Task SendAsync__PublishesEvents()
        {
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Once);
        }

        [Test]
        public async Task SendAsync__NotPublishesEvents_WhenSetInOptions()
        {
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options { PublishEvents = false });

            _sut = new DomainCommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _aggregateStore.Object,
                _commandStore.Object,
                _eventStore.Object,
                _optionsMock.Object);

            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Never);
        }

        [Test]
        public async Task SendAsync__NotPublishesEvents_WhenSetInCommand()
        {
            _createAggregate.PublishEvents = false;
            await _sut.SendAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Never);
        }
    }
}

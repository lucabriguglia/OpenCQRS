using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kledex.Commands;
using Kledex.Dependencies;
using Kledex.Domain;
using Kledex.Events;
using Kledex.Tests.Fakes;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Options = Kledex.Configuration.Options;

namespace Kledex.Tests.Domain
{
    [TestFixture]
    public class CommandSenderAsyncTests
    {
        private ICommandSender _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IDomainStore> _domainStore;
        private Mock<IEventFactory> _eventFactory;

        private Mock<ICommandHandlerAsync<CreateSomething>> _commandHandlerAsync;
        private Mock<ICommandHandlerAsync<CreateAggregate>> _domainCommandHandlerAsync;
        private Mock<IOptions<Options>> _optionsMock;

        private CreateSomething _createSomething;
        private SomethingCreated _somethingCreated;
        private SomethingCreated _somethingCreatedConcrete;
        private IEnumerable<IEvent> _events;

        private CreateAggregate _createAggregate;
        private AggregateCreated _aggregateCreated;
        private AggregateCreated _aggregateCreatedConcrete;
        private Aggregate _aggregate;

        private CommandResponse _commandResponse;
        private CommandResponse _domainCommandResponse;

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

            _commandResponse = new CommandResponse { Events = _events };
            _domainCommandResponse = new CommandResponse { Events = _aggregate.Events };

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher
                .Setup(x => x.PublishAsync(_aggregateCreatedConcrete ))
                .Returns(Task.CompletedTask);

            _domainStore = new Mock<IDomainStore>();
            _domainStore
                .Setup(x => x.SaveAsync(_aggregate.GetType(), _createAggregate.AggregateRootId, _createAggregate, new List<IDomainEvent>() { _aggregateCreatedConcrete }))
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
                .ReturnsAsync(_commandResponse);

            _domainCommandHandlerAsync = new Mock<ICommandHandlerAsync<CreateAggregate>>();
            _domainCommandHandlerAsync
                .Setup(x => x.HandleAsync(_createAggregate))
                .ReturnsAsync(_domainCommandResponse);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveCommandHandler(_createSomething, typeof(ICommandHandlerAsync<>)))
                .Returns(_commandHandlerAsync.Object);
            _handlerResolver
                .Setup(x => x.ResolveCommandHandler(_createAggregate, typeof(ICommandHandlerAsync<>)))
                .Returns(_domainCommandHandlerAsync.Object);

            _optionsMock = new Mock<IOptions<Options>>();
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options());

            _sut = new CommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _domainStore.Object,
                _optionsMock.Object);
        }

        [Test]
        public void SendAsync_ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.SendAsync(_createAggregate));
        }

        [Test]
        public async Task SendAsync_HandlesCommand()
        {
            await _sut.SendAsync(_createSomething);
            _commandHandlerAsync.Verify(x => x.HandleAsync(_createSomething), Times.Once);
        }

        [Test]
        public async Task SendAsync_HandlesDomainCommand()
        {
            await _sut.SendAsync(_createAggregate);
            _domainCommandHandlerAsync.Verify(x => x.HandleAsync(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendAsync_SavesEvents()
        {
            await _sut.SendAsync(_createAggregate);
            _domainStore.Verify(x => x.SaveAsync(_aggregate.GetType(), _createAggregate.AggregateRootId, _createAggregate, new List<IDomainEvent>() { _aggregateCreated }), Times.Once);
        }

        [Test]
        public async Task SendAsync_PublishesEvents()
        {
            await _sut.SendAsync(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete ), Times.Once);
        }

        [Test]
        public async Task SendAsync_NotPublishesEvents_WhenSetInOptions()
        {
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options { PublishEvents = false });

            _sut = new CommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _domainStore.Object,
                _optionsMock.Object);

            await _sut.SendAsync(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Never);
        }

        [Test]
        public async Task SendAsync_NotPublishesEvents_WhenSetInCommand()
        {
            _createAggregate.PublishEvents = false;
            await _sut.SendAsync(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Never);
        }
    }
}

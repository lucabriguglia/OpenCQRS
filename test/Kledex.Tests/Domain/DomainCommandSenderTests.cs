using System;
using System.Collections.Generic;
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
    public class DomainCommandSenderTests
    {
        private IDomainCommandSender _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IEventStore> _eventStore;
        private Mock<ICommandStore> _commandStore;
        private Mock<IAggregateStore> _aggregateStore;
        private Mock<IEventFactory> _eventFactory;

        private Mock<ICommandHandler<CreateSomething>> _commandHandler;
        private Mock<IDomainCommandHandler<CreateAggregate>> _domainCommandHandler;
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
                .Setup(x => x.Publish(_somethingCreatedConcrete));
            _eventPublisher
                .Setup(x => x.Publish(_aggregateCreatedConcrete));

            _eventStore = new Mock<IEventStore>();
            _eventStore
                .Setup(x => x.SaveEvent<Aggregate>(_aggregateCreatedConcrete, null));

            _commandStore = new Mock<ICommandStore>();
            _commandStore
                .Setup(x => x.SaveCommand(_createAggregate));

            _aggregateStore = new Mock<IAggregateStore>();
            _aggregateStore
                .Setup(x => x.SaveAggregate<Aggregate>(_createAggregate.AggregateRootId));

            _eventFactory = new Mock<IEventFactory>();
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_somethingCreated))
                .Returns(_somethingCreatedConcrete);
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_aggregateCreated))
                .Returns(_aggregateCreatedConcrete);

            _commandHandler = new Mock<ICommandHandler<CreateSomething>>();
            _commandHandler
                .Setup(x => x.Handle(_createSomething))
                .Returns(_events);

            _domainCommandHandler = new Mock<IDomainCommandHandler<CreateAggregate>>();
            _domainCommandHandler
                .Setup(x => x.Handle(_createAggregate))
                .Returns(_aggregate.Events);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveHandler<ICommandHandler<CreateSomething>>())
                .Returns(_commandHandler.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler<IDomainCommandHandler<CreateAggregate>>())
                .Returns(_domainCommandHandler.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler(_createAggregate, typeof(IDomainCommandHandler<>)))
                .Returns(_domainCommandHandler.Object);

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
                new Mock<IDomainStore>().Object,
                _optionsMock.Object);
        }

        [Test]
        public void Send_ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Send(_createAggregate));
        }

        [Test]
        public void Send_SendsCommand()
        {
            _sut.Send(_createAggregate);
            _domainCommandHandler.Verify(x => x.Handle(_createAggregate), Times.Once);
        }

        [Test]
        public void Send_SavesAggregate()
        {
            _sut.Send(_createAggregate);
            _aggregateStore.Verify(x => x.SaveAggregate<Aggregate>(_createAggregate.AggregateRootId), Times.Once);
        }

        [Test]
        public void Send_SavesCommand()
        {
            _sut.Send(_createAggregate);
            _commandStore.Verify(x => x.SaveCommand(_createAggregate), Times.Once);
        }

        [Test]
        public void Send_SavesEvents()
        {
            _sut.Send(_createAggregate);
            _eventStore.Verify(x => x.SaveEvent<Aggregate>(_aggregateCreatedConcrete, null), Times.Once);
        }

        [Test]
        public void Send_PublishesEvents()
        {
            _sut.Send(_createAggregate);
            _eventPublisher.Verify(x => x.Publish(_aggregateCreatedConcrete), Times.Once);
        }

        [Test]
        public void Send_NotPublishesEvents_WhenSetInOptions()
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
                new Mock<IDomainStore>().Object,
                _optionsMock.Object);

            _sut.Send(_createAggregate);
            _eventPublisher.Verify(x => x.Publish(_aggregateCreatedConcrete), Times.Never);
        }

        [Test]
        public void Send_NotPublishesEvents_WhenSetInCommand()
        {
            _createAggregate.PublishEvents = false;
            _sut.Send(_createAggregate);
            _eventPublisher.Verify(x => x.Publish(_aggregateCreatedConcrete), Times.Never);
        }
    }
}

using System;
using System.Collections.Generic;
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
    public class CommandSenderTests
    {
        private ICommandSender _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IEventStore> _eventStore;
        private Mock<ICommandStore> _commandStore;
        private Mock<IEventFactory> _eventFactory;

        private Mock<ICommandHandler<CreateSomething>> _commandHandler;
        private Mock<ICommandHandlerWithEvents<CreateSomething>> _commandHandlerWithEvents;
        private Mock<ICommandHandlerWithDomainEvents<CreateAggregate>> _commandHandlerWithDomainEvents;

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
                .Setup(x => x.SaveCommand<Aggregate>(_createAggregate));

            _eventFactory = new Mock<IEventFactory>();
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_somethingCreated))
                .Returns(_somethingCreatedConcrete);
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_aggregateCreated))
                .Returns(_aggregateCreatedConcrete);

            _commandHandler = new Mock<ICommandHandler<CreateSomething>>();
            _commandHandler
                .Setup(x => x.Handle(_createSomething));

            _commandHandlerWithEvents = new Mock<ICommandHandlerWithEvents<CreateSomething>>();
            _commandHandlerWithEvents
                .Setup(x => x.Handle(_createSomething))
                .Returns(_events);

            _commandHandlerWithDomainEvents = new Mock<ICommandHandlerWithDomainEvents<CreateAggregate>>();
            _commandHandlerWithDomainEvents
                .Setup(x => x.Handle(_createAggregate))
                .Returns(_aggregate.Events);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveHandler<ICommandHandler<CreateSomething>>())
                .Returns(_commandHandler.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler<ICommandHandlerWithEvents<CreateSomething>>())
                .Returns(_commandHandlerWithEvents.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler<ICommandHandlerWithDomainEvents<CreateAggregate>>())
                .Returns(_commandHandlerWithDomainEvents.Object);

            _sut = new CommandSender(_handlerResolver.Object, _eventPublisher.Object, _eventFactory.Object, _eventStore.Object, _commandStore.Object);
        }

        [Test]
        public void Send_ThrowsException_WhenCommandIsNull()
        {
            _createSomething = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Send(_createSomething));
        }

        [Test]
        public void Send_SendsCommand()
        {
            _sut.Send(_createSomething);
            _commandHandler.Verify(x => x.Handle(_createSomething), Times.Once);
        }

        [Test]
        public void SendWithDomainEvents_ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Send<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public void SendWithDomainEvents_SavesCommand()
        {
            _sut.Send<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommand<Aggregate>(_createAggregate), Times.Once);
        }

        [Test]
        public void SendWithDomainEvents_SendsCommand()
        {
            _sut.Send<CreateAggregate, Aggregate>(_createAggregate);
            _commandHandlerWithDomainEvents.Verify(x => x.Handle(_createAggregate), Times.Once);
        }

        [Test]
        public void SendWithDomainEvents_SavesEvents()
        {
            _sut.Send<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEvent<Aggregate>(_aggregateCreatedConcrete, null), Times.Once);
        }

        [Test]
        public void SendAndPublish_ThrowsException_WhenCommandIsNull()
        {
            _createSomething = null;
            Assert.Throws<ArgumentNullException>(() => _sut.SendAndPublish(_createSomething));
        }

        [Test]
        public void SendAndPublish_SendsCommand()
        {
            _sut.SendAndPublish(_createSomething);
            _commandHandlerWithEvents.Verify(x => x.Handle(_createSomething), Times.Once);
        }

        [Test]
        public void SendAndPublish_PublishesEvents()
        {
            _sut.SendAndPublish(_createSomething);
            _eventPublisher.Verify(x => x.Publish(_somethingCreatedConcrete), Times.Once);
        }

        [Test]
        public void SendAndPublishWithDomainEvents_ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.Throws<ArgumentNullException>(() => _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public void SendAndPublishWithDomainEvents_SendsCommand()
        {
            _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate);
            _commandHandlerWithDomainEvents.Verify(x => x.Handle(_createAggregate), Times.Once);
        }

        [Test]
        public void SendAndPublishWithDomainEvents_SavesCommand()
        {
            _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate);
            _commandStore.Verify(x => x.SaveCommand<Aggregate>(_createAggregate), Times.Once);
        }

        [Test]
        public void SendAndPublishWithDomainEvents_SavesEvents()
        {
            _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEvent<Aggregate>(_aggregateCreatedConcrete, null), Times.Once);
        }

        [Test]
        public void SendAndPublishWithDomainEvents_PublishesEvents()
        {
            _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.Publish(_aggregateCreatedConcrete), Times.Once);
        }
    }
}

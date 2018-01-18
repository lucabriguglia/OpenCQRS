using System;
using System.Collections.Generic;
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
    public class CommandSenderTests
    {
        private ICommandSender _sut;

        private Mock<IResolver> _resolver;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IEventStore> _eventStore;
        private Mock<IEventFactory> _eventFactory;

        private Mock<ICommandHandler<CreateSomething>> _commandHandler;
        private Mock<ICommandHandlerWithEvents<CreateSomething>> _commandHandlerWithEvents;
        private Mock<IDomainCommandHandler<CreateAggregate>> _domainCommandHandler;

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
                .Setup(x => x.SaveEvent<Aggregate>(_aggregateCreatedConcrete));

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

            _domainCommandHandler = new Mock<IDomainCommandHandler<CreateAggregate>>();
            _domainCommandHandler
                .Setup(x => x.Handle(_createAggregate))
                .Returns(_aggregate);

            _resolver = new Mock<IResolver>();
            _resolver
                .Setup(x => x.Resolve<ICommandHandler<CreateSomething>>())
                .Returns(_commandHandler.Object);
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithEvents<CreateSomething>>())
                .Returns(_commandHandlerWithEvents.Object);
            _resolver
                .Setup(x => x.Resolve<IDomainCommandHandler<CreateAggregate>>())
                .Returns(_domainCommandHandler.Object);

            _sut = new CommandSender(_resolver.Object, _eventPublisher.Object, _eventStore.Object, _eventFactory.Object);
        }

        [Test]
        public void SendThrowsExceptionWhenCommandIsNull()
        {
            _createSomething = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Send(_createSomething));
        }

        [Test]
        public void SendThrowsExceptionWhenCommandHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<ICommandHandler<CreateSomething>>())
                .Returns((ICommandHandler<CreateSomething>)null);
            Assert.Throws<ApplicationException>(() => _sut.Send(_createSomething));
        }

        [Test]
        public void SendCommand()
        {
            _sut.Send(_createSomething);
            _commandHandler.Verify(x => x.Handle(_createSomething), Times.Once);
        }

        [Test]
        public void SendAndPublishThrowsExceptionWhenCommandIsNull()
        {
            _createSomething = null;
            Assert.Throws<ArgumentNullException>(() => _sut.SendAndPublish(_createSomething));
        }

        [Test]
        public void SendAndPublishThrowsExceptionWhenCommandHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithEvents<CreateSomething>>())
                .Returns((ICommandHandlerWithEvents<CreateSomething>)null);
            Assert.Throws<ApplicationException>(() => _sut.SendAndPublish(_createSomething));
        }

        [Test]
        public void SendAndPublishSendCommand()
        {
            _sut.SendAndPublish(_createSomething);
            _commandHandlerWithEvents.Verify(x => x.Handle(_createSomething), Times.Once);
        }

        [Test]
        public void SendAndPublishPublishEvents()
        {
            _sut.SendAndPublish(_createSomething);
            _eventPublisher.Verify(x => x.Publish(_somethingCreatedConcrete), Times.Once);
        }

        [Test]
        public void SendAndPublishWithAggregateThrowsExceptionWhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.Throws<ArgumentNullException>(() => _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public void SendAndPublishWithAggregateThrowsExceptionWhenCommandHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<IDomainCommandHandler<CreateAggregate>>())
                .Returns((IDomainCommandHandler<CreateAggregate>)null);
            Assert.Throws<ApplicationException>(() => _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public void SendAndPublishWithAggregateSendsCommand()
        {
            _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate);
            _domainCommandHandler.Verify(x => x.Handle(_createAggregate), Times.Once);
        }

        [Test]
        public void SendAndPublishWithAggregateSaveEvents()
        {
            _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEvent<Aggregate>(_aggregateCreatedConcrete), Times.Once);
        }

        [Test]
        public void SendAndPublishWithAggregatePublishEvents()
        {
            _sut.SendAndPublish<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.Publish(_aggregateCreatedConcrete), Times.Once);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Weapsy.Mediator.Commands;
using Weapsy.Mediator.Dependencies;
using Weapsy.Mediator.Domain;
using Weapsy.Mediator.Events;
using Weapsy.Mediator.Tests.Fakes;

namespace Weapsy.Mediator.Tests
{
    [TestFixture]
    public class CommandSenderAsyncTests
    {
        private ICommandSenderAsync _sut;

        private Mock<IResolver> _resolver;
        private Mock<IEventPublisherAsync> _eventPublisher;
        private Mock<IEventStore> _eventStore;
        private Mock<IEventFactory> _eventFactory;

        private Mock<ICommandHandlerAsync<CreateSomething>> _commandHandlerAsync;
        private Mock<ICommandHandlerWithEventsAsync<CreateSomething>> _commandHandlerWithEventsAsync;
        private Mock<IDomainCommandHandlerAsync<CreateAggregate>> _domainCommandHandlerAsync;

        private CreateSomething _createSomething;
        private SomethingCreated _somethingCreated;
        private SomethingCreated _somethingCreatedConcrete;
        private IEnumerable<IEvent> _events;

        private CreateAggregate _createAggregate;
        private AggregateCreated _aggregateCreated;
        private AggregateCreated _aggregateCreatedConcrete;
        private IEnumerable<IDomainEvent> _domainEvents;

        [SetUp]
        public void SetUp()
        {
            _createSomething = new CreateSomething();
            _somethingCreated = new SomethingCreated();
            _somethingCreatedConcrete = new SomethingCreated();
            _events = new List<IEvent> { _somethingCreated };

            _createAggregate = new CreateAggregate();
            _aggregateCreated = new AggregateCreated();
            _aggregateCreatedConcrete = new AggregateCreated();
            _domainEvents = new List<IDomainEvent> { _aggregateCreated };

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

            _domainCommandHandlerAsync = new Mock<IDomainCommandHandlerAsync<CreateAggregate>>();
            _domainCommandHandlerAsync
                .Setup(x => x.HandleAsync(_createAggregate))
                .ReturnsAsync(_domainEvents);

            _resolver = new Mock<IResolver>();
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerAsync<CreateSomething>>())
                .Returns(_commandHandlerAsync.Object);
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithEventsAsync<CreateSomething>>())
                .Returns(_commandHandlerWithEventsAsync.Object);
            _resolver
                .Setup(x => x.Resolve<IDomainCommandHandlerAsync<CreateAggregate>>())
                .Returns(_domainCommandHandlerAsync.Object);

            _sut = new CommandSenderAsync(_resolver.Object, _eventPublisher.Object, _eventStore.Object, _eventFactory.Object);
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
        public async Task SendAndPublishAsyncPublishEvents()
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
        public void SendAndPublishhWithAggregateAsyncThrowsExceptionWhenCommandHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<IDomainCommandHandlerAsync<CreateAggregate>>())
                .Returns((IDomainCommandHandlerAsync<CreateAggregate>)null);
            Assert.ThrowsAsync<ApplicationException>(async () => await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate));
        }

        [Test]
        public async Task SendAndPublishhWithAggregateAsyncSendsCommand()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _domainCommandHandlerAsync.Verify(x => x.HandleAsync(_createAggregate), Times.Once);
        }

        [Test]
        public async Task SendAndPublishhWithAggregateAsyncSaveEvents()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventStore.Verify(x => x.SaveEventAsync<Aggregate>(_aggregateCreatedConcrete), Times.Once);
        }

        [Test]
        public async Task SendAndPublishhWithAggregateAsyncPublishEvents()
        {
            await _sut.SendAndPublishAsync<CreateAggregate, Aggregate>(_createAggregate);
            _eventPublisher.Verify(x => x.PublishAsync(_aggregateCreatedConcrete), Times.Once);
        }
    }
}

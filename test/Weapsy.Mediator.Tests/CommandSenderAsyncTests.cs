using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Weapsy.Mediator.Commands;
using Weapsy.Mediator.Dependencies;
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

        private Mock<ICommandHandlerAsync<CreateSomething>> _commandHandlerAsync;
        private Mock<ICommandHandlerWithEventsAsync<CreateSomething>> _commandHandlerWithEventsAsync;

        private CreateSomething _createSomething;
        private SomethingCreated _somethingCreated;
        private IEnumerable<IEvent> _events;

        [SetUp]
        public void SetUp()
        {
            _createSomething = new CreateSomething();
            _somethingCreated = new SomethingCreated();
            _events = new List<IEvent> { new SomethingCreated() };

            _eventPublisher = new Mock<IEventPublisherAsync>();
            _eventPublisher
                .Setup(x => x.PublishAsync(_somethingCreated))
                .Returns(Task.CompletedTask);

            _commandHandlerAsync = new Mock<ICommandHandlerAsync<CreateSomething>>();
            _commandHandlerAsync
                .Setup(x => x.HandleAsync(_createSomething))
                .Returns(Task.CompletedTask);

            _commandHandlerWithEventsAsync = new Mock<ICommandHandlerWithEventsAsync<CreateSomething>>();
            _commandHandlerWithEventsAsync
                .Setup(x => x.HandleAsync(_createSomething))
                .ReturnsAsync(_events);

            _resolver = new Mock<IResolver>();
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerAsync<CreateSomething>>())
                .Returns(_commandHandlerAsync.Object);
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithEventsAsync<CreateSomething>>())
                .Returns(_commandHandlerWithEventsAsync.Object);

            _sut = new CommandSenderAsync(_resolver.Object, _eventPublisher.Object);
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
            _eventPublisher.Verify(x => x.PublishAsync(It.IsAny<IEvent>()), Times.Once);
        }
    }
}

using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Mediator.Commands;
using Weapsy.Mediator.Dependencies;
using Weapsy.Mediator.Events;
using Weapsy.Mediator.Tests.Fakes;

namespace Weapsy.Mediator.Tests
{
    [TestFixture]
    public class CommandSenderTests
    {
        private ICommandSender _sut;

        private Mock<IResolver> _resolver;
        private Mock<IEventPublisher> _eventPublisher;

        private Mock<ICommandHandler<CreateSomething>> _commandHandler;
        private Mock<ICommandHandlerWithEvents<CreateSomething>> _commandHandlerWithEvents;

        private CreateSomething _createSomething;
        private SomethingCreated _somethingCreated;
        private IEnumerable<IEvent> _events;

        [SetUp]
        public void SetUp()
        {
            _createSomething = new CreateSomething();
            _somethingCreated = new SomethingCreated();
            _events = new List<IEvent> { new SomethingCreated() };

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher
                .Setup(x => x.Publish(_somethingCreated));
            _eventPublisher
                .Setup(x => x.Publish(_somethingCreated));

            _commandHandler = new Mock<ICommandHandler<CreateSomething>>();
            _commandHandler
                .Setup(x => x.Handle(_createSomething));

            _commandHandlerWithEvents = new Mock<ICommandHandlerWithEvents<CreateSomething>>();
            _commandHandlerWithEvents
                .Setup(x => x.Handle(_createSomething))
                .Returns(_events);

            _resolver = new Mock<IResolver>();
            _resolver
                .Setup(x => x.Resolve<ICommandHandler<CreateSomething>>())
                .Returns(_commandHandler.Object);
            _resolver
                .Setup(x => x.Resolve<ICommandHandlerWithEvents<CreateSomething>>())
                .Returns(_commandHandlerWithEvents.Object);

            _sut = new CommandSender(_resolver.Object, _eventPublisher.Object);
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
            _eventPublisher.Verify(x => x.Publish(It.IsAny<IEvent>()), Times.Once);
        }
    }
}

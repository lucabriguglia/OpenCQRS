using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Mediator.Dependencies;
using Weapsy.Mediator.Events;
using Weapsy.Mediator.Tests.Fakes;

namespace Weapsy.Mediator.Tests
{
    [TestFixture]
    public class EventPublisherTests
    {
        private IEventPublisher _sut;

        private Mock<IResolver> _resolver;

        private Mock<IEventHandler<SomethingCreated>> _eventHandler1;
        private Mock<IEventHandler<SomethingCreated>> _eventHandler2;

        private SomethingCreated _somethingCreated;

        [SetUp]
        public void SetUp()
        {
            _somethingCreated = new SomethingCreated();

            _eventHandler1 = new Mock<IEventHandler<SomethingCreated>>();
            _eventHandler1
                .Setup(x => x.Handle(_somethingCreated));

            _eventHandler2 = new Mock<IEventHandler<SomethingCreated>>();
            _eventHandler2
                .Setup(x => x.Handle(_somethingCreated));

            _resolver = new Mock<IResolver>();
            _resolver
                .Setup(x => x.ResolveAll<IEventHandler<SomethingCreated>>())
                .Returns(new List<IEventHandler<SomethingCreated>>{_eventHandler1.Object, _eventHandler2.Object});

            _sut = new EventPublisher(_resolver.Object);
        }
    
        [Test]
        public void PublishThrowsExceptionWhenEventIsNull()
        {
            _somethingCreated = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Publish(_somethingCreated));
        }

        [Test]
        public void PublishFirstEvent()
        {
            _sut.Publish(_somethingCreated);
            _eventHandler1.Verify(x => x.Handle(_somethingCreated), Times.Once);
        }

        [Test]
        public void PublishSecondEvent()
        {
            _sut.Publish(_somethingCreated);
            _eventHandler2.Verify(x => x.Handle(_somethingCreated), Times.Once);
        }       
    }
}

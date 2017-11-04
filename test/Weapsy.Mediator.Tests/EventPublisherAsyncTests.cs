using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Weapsy.Mediator.Dependencies;
using Weapsy.Mediator.Events;
using Weapsy.Mediator.Tests.Fakes;

namespace Weapsy.Mediator.Tests
{
    [TestFixture]
    public class EventPublisherAsyncTests
    {
        private IEventPublisherAsync _sut;

        private Mock<IResolver> _resolver;

        private Mock<IEventHandlerAsync<SomethingCreated>> _eventHandler1;
        private Mock<IEventHandlerAsync<SomethingCreated>> _eventHandler2;

        private SomethingCreated _somethingCreated;

        [SetUp]
        public void SetUp()
        {
            _somethingCreated = new SomethingCreated();

            _eventHandler1 = new Mock<IEventHandlerAsync<SomethingCreated>>();
            _eventHandler1
                .Setup(x => x.HandleAsync(_somethingCreated))
                .Returns(Task.CompletedTask);

            _eventHandler2 = new Mock<IEventHandlerAsync<SomethingCreated>>();
            _eventHandler2
                .Setup(x => x.HandleAsync(_somethingCreated))
                .Returns(Task.CompletedTask);

            _resolver = new Mock<IResolver>();
            _resolver
                .Setup(x => x.ResolveAll<IEventHandlerAsync<SomethingCreated>>())
                .Returns(new List<IEventHandlerAsync<SomethingCreated>>{_eventHandler1.Object, _eventHandler2.Object});

            _sut = new EventPublisherAsync(_resolver.Object);
        }
    
        [Test]
        public void PublishThrowsExceptionWhenEventIsNull()
        {
            _somethingCreated = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.PublishAsync(_somethingCreated));
        }

        [Test]
        public async Task PublishFirstEvent()
        {
            await _sut.PublishAsync(_somethingCreated);
            _eventHandler1.Verify(x => x.HandleAsync(_somethingCreated), Times.Once);
        }

        [Test]
        public async Task PublishSecondEvent()
        {
            await _sut.PublishAsync(_somethingCreated);
            _eventHandler2.Verify(x => x.HandleAsync(_somethingCreated), Times.Once);
        }       
    }
}

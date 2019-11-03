using System.Threading.Tasks;
using Kledex.Bus;
using Kledex.Domain;
using Kledex.Events;
using Kledex.Queries;
using Kledex.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace Kledex.Tests
{
    [TestFixture]
    public class DispatcherTests
    {
        private IDispatcher _sut;

        private Mock<ICommandSender> _commandSender;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IQueryProcessor> _queryDispatcher;
        private Mock<IBusMessageDispatcher> _busMessageDispatcher;

        private CreateSomething _createSomething;
        private SomethingCreated _somethingCreated;
        private GetSomething _getSomething;
        private Something _something;
        private CreateAggregate _createAggregate;
        private CreateAggregateBusMessage _createAggregateBusMessage;

        [SetUp]
        public void SetUp()
        {
            _createSomething = new CreateSomething();
            _somethingCreated = new SomethingCreated();
            _getSomething = new GetSomething();
            _something = new Something();
            _createAggregate = new CreateAggregate();
            _createAggregateBusMessage = new CreateAggregateBusMessage();

            _commandSender = new Mock<ICommandSender>();
            _commandSender
                .Setup(x => x.SendAsync((IDomainCommand<IAggregateRoot>)_createAggregate))
                .Returns(Task.CompletedTask);
            _commandSender
                .Setup(x => x.Send((IDomainCommand<IAggregateRoot>)_createAggregate));

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher
                .Setup(x => x.PublishAsync(_somethingCreated))
                .Returns(Task.CompletedTask);
            _eventPublisher
                .Setup(x => x.Publish(_somethingCreated));

            _queryDispatcher = new Mock<IQueryProcessor>();
            _queryDispatcher
                .Setup(x => x.ProcessAsync(_getSomething))
                .ReturnsAsync(_something);
            _queryDispatcher
                .Setup(x => x.Process(_getSomething))
                .Returns(_something);

            _busMessageDispatcher = new Mock<IBusMessageDispatcher>();
            _busMessageDispatcher
                .Setup(x => x.DispatchAsync(_createAggregateBusMessage))
                .Returns(Task.CompletedTask);

            _sut = new Dispatcher(_commandSender.Object,
                _eventPublisher.Object,
                _queryDispatcher.Object,
                _busMessageDispatcher.Object);
        }

        [Test]
        public async Task SendsCommandWithAggregateAsync()
        {
            await _sut.SendAsync(_createAggregate);
            _commandSender.Verify(x => x.SendAsync(_createAggregate), Times.Once);
        }

        [Test]
        public void SendsCommandWithAggregate()
        {
            _sut.Send(_createAggregate);
            _commandSender.Verify(x => x.Send(_createAggregate), Times.Once);
        }

        [Test]
        public async Task PublishesEventAsync()
        {
            await _sut.PublishAsync(_somethingCreated);
            _eventPublisher.Verify(x => x.PublishAsync(_somethingCreated), Times.Once);
        }

        [Test]
        public void PublishesEvent()
        {
            _sut.Publish(_somethingCreated);
            _eventPublisher.Verify(x => x.Publish(_somethingCreated), Times.Once);
        }

        [Test]
        public async Task GetsResultAsync()
        {
            await _sut.GetResultAsync(_getSomething);
            _queryDispatcher.Verify(x => x.ProcessAsync(_getSomething), Times.Once);
        }

        [Test]
        public void GetsResult()
        {
            _sut.GetResult(_getSomething);
            _queryDispatcher.Verify(x => x.Process(_getSomething), Times.Once);
        }

        [Test]
        public async Task DispatchesBusMessageAsync()
        {
            await _sut.DispatchBusMessageAsync(_createAggregateBusMessage);
            _busMessageDispatcher.Verify(x => x.DispatchAsync(_createAggregateBusMessage), Times.Once);
        }
    }
}

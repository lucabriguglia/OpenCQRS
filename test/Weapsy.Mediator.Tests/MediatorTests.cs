using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Weapsy.Mediator.Commands;
using Weapsy.Mediator.Events;
using Weapsy.Mediator.Queries;
using Weapsy.Mediator.Tests.Fakes;

namespace Weapsy.Mediator.Tests
{
    [TestFixture]
    public class MediatorTests
    {
        private IMediator _sut;

        private Mock<ICommandSenderAsync> _commandSenderAsync;
        private Mock<ICommandSender> _commandSender;

        private Mock<IEventPublisherAsync> _eventPublisherAsync;
        private Mock<IEventPublisher> _eventPublisher;

        private Mock<IQueryDispatcherAsync> _queryDispatcherAsync;
        private Mock<IQueryDispatcher> _queryDispatcher;

        private CreateSomething _createSomething;
        private SomethingCreated _somethingCreated;
        private GetSomething _getSomething;
        private Something _something;

        [SetUp]
        public void SetUp()
        {
            _createSomething = new CreateSomething();
            _somethingCreated = new SomethingCreated();
            _getSomething = new GetSomething();
            _something = new Something();

            _commandSenderAsync = new Mock<ICommandSenderAsync>();
            _commandSenderAsync
                .Setup(x => x.SendAsync(_createSomething))
                .Returns(Task.CompletedTask);
            _commandSenderAsync
                .Setup(x => x.SendAndPublishAsync(_createSomething))
                .Returns(Task.CompletedTask);

            _commandSender = new Mock<ICommandSender>();
            _commandSender
                .Setup(x => x.Send(_createSomething));
            _commandSender
                .Setup(x => x.SendAndPublish(_createSomething));

            _eventPublisherAsync = new Mock<IEventPublisherAsync>();
            _eventPublisherAsync
                .Setup(x => x.PublishAsync(_somethingCreated))
                .Returns(Task.CompletedTask);

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher
                .Setup(x => x.Publish(_somethingCreated));

            _queryDispatcherAsync = new Mock<IQueryDispatcherAsync>();
            _queryDispatcherAsync
                .Setup(x => x.DispatchAsync<IQuery, Something>(_getSomething))
                .ReturnsAsync(_something);

            _queryDispatcher = new Mock<IQueryDispatcher>();
            _queryDispatcher
                .Setup(x => x.Dispatch<IQuery, Something>(_getSomething))
                .Returns(_something);

            _sut = new Mediator(_commandSenderAsync.Object, 
                _commandSender.Object, 
                _eventPublisherAsync.Object,
                _eventPublisher.Object,
                _queryDispatcherAsync.Object,
                _queryDispatcher.Object);
        }

        [Test]
        public async Task SendsCommandAsync()
        {
            await _sut.SendAsync(_createSomething);
            _commandSenderAsync.Verify(x => x.SendAsync(_createSomething), Times.Once);
        }

        [Test]
        public async Task SendsCommandAndPublishAsync()
        {
            await _sut.SendAndPublishAsync(_createSomething);
            _commandSenderAsync.Verify(x => x.SendAndPublishAsync(_createSomething), Times.Once);
        }

        [Test]
        public void SendsCommand()
        {
            _sut.Send(_createSomething);
            _commandSender.Verify(x => x.Send(_createSomething), Times.Once);
        }

        [Test]
        public void SendsCommandAndPublish()
        {
            _sut.SendAndPublish(_createSomething);
            _commandSender.Verify(x => x.SendAndPublish(_createSomething), Times.Once);
        }

        [Test]
        public async Task PublishEventAsync()
        {
            await _sut.PublishAsync(_somethingCreated);
            _eventPublisherAsync.Verify(x => x.PublishAsync(_somethingCreated), Times.Once);
        }

        [Test]
        public void PublishEvent()
        {
            _sut.Publish(_somethingCreated);
            _eventPublisher.Verify(x => x.Publish(_somethingCreated), Times.Once);
        }

        [Test]
        public async Task GetsResultAsync()
        {
            await _sut.GetResultAsync<GetSomething, Something>(_getSomething);
            _queryDispatcherAsync.Verify(x => x.DispatchAsync<GetSomething, Something>(_getSomething), Times.Once);
        }

        [Test]
        public void GetsResult()
        {
            _sut.GetResult<GetSomething, Something>(_getSomething);
            _queryDispatcher.Verify(x => x.Dispatch<GetSomething, Something>(_getSomething), Times.Once);
        }
    }
}

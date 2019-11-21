using System;
using System.Collections.Generic;
using Kledex.Commands;
using Kledex.Dependencies;
using Kledex.Domain;
using Kledex.Events;
using Kledex.Tests.Fakes;
using Kledex.Validation;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Options = Kledex.Configuration.Options;

namespace Kledex.Tests.Commands
{
    [TestFixture]
    public class CommandSenderTests
    {
        private ICommandSender _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IStoreProvider> _storeProvider;
        private Mock<IEventFactory> _eventFactory;
        private Mock<IValidationService> _validationService;

        private Mock<ICommandHandler<CreateSomething>> _commandHandler;
        private Mock<ICommandHandler<CreateAggregate>> _domainCommandHandler;
        private Mock<ISequenceCommandHandler<ICommand>> _sequenceCommandHandler;
        private Mock<IOptions<Options>> _optionsMock;

        private CreateSomething _createSomething;
        private SomethingCreated _somethingCreated;
        private SomethingCreated _somethingCreatedConcrete;
        private IEnumerable<IEvent> _events;

        private CreateAggregate _createAggregate;
        private AggregateCreated _aggregateCreated;
        private AggregateCreated _aggregateCreatedConcrete;
        private Aggregate _aggregate;

        private SampleSequenceCommand _sampleSequenceCommand;

        private CommandResponse _commandResponse;
        private CommandResponse _domainCommandResponse;

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

            _sampleSequenceCommand = new SampleSequenceCommand();

            _commandResponse = new CommandResponse { Events = _events, Result = "Result" };
            _domainCommandResponse = new CommandResponse { Events = _aggregate.Events, Result = "Result" };

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher
                .Setup(x => x.Publish(_aggregateCreatedConcrete));

            _storeProvider = new Mock<IStoreProvider>();
            _storeProvider
                .Setup(x => x.Save(_aggregate.GetType(), _createAggregate.AggregateRootId, _createAggregate, new List<IDomainEvent>() { _aggregateCreated }));

            _eventFactory = new Mock<IEventFactory>();
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_somethingCreated))
                .Returns(_somethingCreatedConcrete);
            _eventFactory
                .Setup(x => x.CreateConcreteEvent(_aggregateCreated))
                .Returns(_aggregateCreatedConcrete);

            _validationService = new Mock<IValidationService>();
            _validationService
                .Setup(x => x.Validate(_createAggregate));

            _commandHandler = new Mock<ICommandHandler<CreateSomething>>();
            _commandHandler
                .Setup(x => x.Handle(_createSomething))
                .Returns(_commandResponse);

            _domainCommandHandler = new Mock<ICommandHandler<CreateAggregate>>();
            _domainCommandHandler
                .Setup(x => x.Handle(_createAggregate))
                .Returns(_domainCommandResponse);

            _sequenceCommandHandler = new Mock<ISequenceCommandHandler<ICommand>>();
            _sequenceCommandHandler
                .Setup(x => x.Handle(It.IsAny<ICommand>(), It.IsAny<CommandResponse>()))
                .Returns(It.IsAny<CommandResponse>());

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveHandler(_createSomething, typeof(ICommandHandler<>)))
                .Returns(_commandHandler.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler(_createAggregate, typeof(ICommandHandler<>)))
                .Returns(_domainCommandHandler.Object);
            _handlerResolver
                .Setup(x => x.ResolveHandler(It.IsAny<ICommand>(), typeof(ISequenceCommandHandler<>)))
                .Returns(_sequenceCommandHandler.Object);

            _optionsMock = new Mock<IOptions<Options>>();
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options());

            _sut = new CommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _storeProvider.Object,
                _validationService.Object,
                _optionsMock.Object);
        }

        [Test]
        public void Send_ThrowsException_WhenCommandIsNull()
        {
            _createAggregate = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Send(_createAggregate));
        }

        [Test]
        public void Send_ValidatesCommand()
        {
            _createSomething.Validate = true;
            _sut.Send(_createSomething);
            _validationService.Verify(x => x.Validate(_createSomething), Times.Once);
        }

        [Test]
        public void Send_HandlesCommand()
        {
            _sut.Send(_createSomething);
            _commandHandler.Verify(x => x.Handle(_createSomething), Times.Once);
        }

        [Test]
        public void Send_HandlesDomainCommand()
        {
            _sut.Send(_createAggregate);
            _domainCommandHandler.Verify(x => x.Handle(_createAggregate), Times.Once);
        }

        [Test]
        public void Send_HandlesCommand_InSequenceCommand()
        {
            _sut.Send(_sampleSequenceCommand);
            _sequenceCommandHandler.Verify(x => x.Handle(It.IsAny<ICommand>(), It.IsAny<CommandResponse>()), Times.Once);
        }

        [Test]
        public void Send_SavesEvents()
        {
            _sut.Send(_createAggregate);
            _storeProvider.Verify(x => x.Save(_aggregate.GetType(), _createAggregate.AggregateRootId, _createAggregate, new List<IDomainEvent>() { _aggregateCreated }), Times.Once);
        }

        [Test]
        public void Send_PublishesEvents()
        {
            _sut.Send(_createAggregate);
            _eventPublisher.Verify(x => x.Publish(_aggregateCreatedConcrete ), Times.Once);
        }

        [Test]
        public void Send_NotPublishesEvents_WhenSetInOptions()
        {
            _optionsMock
                .Setup(x => x.Value)
                .Returns(new Options { PublishEvents = false });

            _sut = new CommandSender(_handlerResolver.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _storeProvider.Object,
                new Mock<IValidationService>().Object,
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

        [Test]
        public void SendWithResult_ReturnsResult()
        {
            var actual = _sut.Send<string>(_createSomething);
            Assert.AreEqual("Result", actual);
        }
    }
}

//using System;
//using System.Collections.Generic;
//using Kledex.Commands;
//using Kledex.Dependencies;
//using Kledex.Events;
//using Kledex.Tests.Fakes;
//using Microsoft.Extensions.Options;
//using Moq;
//using NUnit.Framework;
//using Options = Kledex.Configuration.Options;

//namespace Kledex.Tests.Commands
//{
//    [TestFixture]
//    public class CommandSenderTests
//    {
//        private ICommandSender _sut;

//        private Mock<IHandlerResolver> _handlerResolver;
//        private Mock<IEventPublisher> _eventPublisher;
//        private Mock<IEventFactory> _eventFactory;

//        private Mock<ICommandHandler<CreateSomething>> _commandHandler;
//        private Mock<ICommandHandler<CreateAggregate>> _domainCommandHandler;
//        private Mock<IOptions<Options>> _optionsMock;

//        private CreateSomething _createSomething;
//        private SomethingCreated _somethingCreated;
//        private SomethingCreated _somethingCreatedConcrete;
//        private IEnumerable<IEvent> _events;

//        private CreateAggregate _createAggregate;
//        private AggregateCreated _aggregateCreated;
//        private AggregateCreated _aggregateCreatedConcrete;
//        private Aggregate _aggregate;

//        [SetUp]
//        public void SetUp()
//        {
//            _createSomething = new CreateSomething();
//            _somethingCreated = new SomethingCreated();
//            _somethingCreatedConcrete = new SomethingCreated();
//            _events = new List<IEvent> { _somethingCreated };

//            _createAggregate = new CreateAggregate();
//            _aggregateCreatedConcrete = new AggregateCreated();
//            _aggregate = new Aggregate();
//            _aggregateCreated = (AggregateCreated)_aggregate.Events[0];

//            _eventPublisher = new Mock<IEventPublisher>();
//            _eventPublisher
//                .Setup(x => x.Publish(_somethingCreatedConcrete));
//            _eventPublisher
//                .Setup(x => x.Publish(_aggregateCreatedConcrete));

//            _eventFactory = new Mock<IEventFactory>();
//            _eventFactory
//                .Setup(x => x.CreateConcreteEvent(_somethingCreated))
//                .Returns(_somethingCreatedConcrete);
//            _eventFactory
//                .Setup(x => x.CreateConcreteEvent(_aggregateCreated))
//                .Returns(_aggregateCreatedConcrete);

//            _commandHandler = new Mock<ICommandHandler<CreateSomething>>();
//            _commandHandler
//                .Setup(x => x.Handle(_createSomething))
//                .Returns(_events);

//            _domainCommandHandler = new Mock<ICommandHandler<CreateAggregate>>();
//            _domainCommandHandler
//                .Setup(x => x.Handle(_createAggregate))
//                .Returns(_aggregate.Events);

//            _handlerResolver = new Mock<IHandlerResolver>();
//            _handlerResolver
//                .Setup(x => x.ResolveHandler<ICommandHandler<CreateSomething>>())
//                .Returns(_commandHandler.Object);
//            _handlerResolver
//                .Setup(x => x.ResolveHandler<ICommandHandler<CreateAggregate>>())
//                .Returns(_domainCommandHandler.Object);

//            _optionsMock = new Mock<IOptions<Options>>();
//            _optionsMock
//                .Setup(x => x.Value)
//                .Returns(new Options());

//            _sut = new CommandSender(_handlerResolver.Object,
//                _eventPublisher.Object,
//                _eventFactory.Object,
//                _optionsMock.Object);
//        }

//        [Test]
//        public void Send_ThrowsException_WhenCommandIsNull()
//        {
//            _createSomething = null;
//            Assert.Throws<ArgumentNullException>(() => _sut.Send(_createSomething));
//        }

//        [Test]
//        public void Send_SendsCommand()
//        {
//            _sut.Send(_createSomething);
//            _commandHandler.Verify(x => x.Handle(_createSomething), Times.Once);
//        }

//        [Test]
//        public void Send_PublishesEvents()
//        {
//            _sut.Send(_createSomething);
//            _eventPublisher.Verify(x => x.Publish(_somethingCreatedConcrete), Times.Once);
//        }
//    }
//}

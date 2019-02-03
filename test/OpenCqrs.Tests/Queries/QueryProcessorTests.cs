using System;
using Moq;
using NUnit.Framework;
using OpenCqrs.Abstractions.Queries;
using OpenCqrs.Dependencies;
using OpenCqrs.Queries;
using OpenCqrs.Tests.Fakes;

namespace OpenCqrs.Tests.Queries
{
    [TestFixture]
    public class QueryProcessorTests
    {
        private IQueryProcessor _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IQueryHandler<GetSomething, Something>> _queryHendler;

        private GetSomething _getSomething;
        private Something _something;

        [SetUp]
        public void SetUp()
        {
            _getSomething = new GetSomething();
            _something = new Something();

            _queryHendler = new Mock<IQueryHandler<GetSomething, Something>>();
            _queryHendler
                .Setup(x => x.Retrieve(_getSomething))
                .Returns(_something);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveHandler<IQueryHandler<GetSomething, Something>>())
                .Returns(_queryHendler.Object);

            _sut = new QueryProcessor(_handlerResolver.Object);
        }
    
        [Test]
        public void Process_ThrowsException_WhenQueryIsNull()
        {
            _getSomething = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Process<GetSomething, Something>(_getSomething));
        }

        [Test]
        public void Process_ReturnsResult()
        {
            var result = _sut.Process<GetSomething, Something>(_getSomething);
            Assert.AreEqual(_something, result);
        }      
    }
}

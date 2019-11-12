using System;
using Kledex.Caching;
using Kledex.Dependencies;
using Kledex.Queries;
using Kledex.Tests.Fakes;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Options = Kledex.Configuration.Options;

namespace Kledex.Tests.Queries
{
    [TestFixture]
    public class QueryProcessorTests
    {
        private IQueryProcessor _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IQueryHandler<GetSomething, Something>> _queryHandler;

        private GetSomething _getSomething;
        private Something _something;

        [SetUp]
        public void SetUp()
        {
            _getSomething = new GetSomething();
            _something = new Something();

            _queryHandler = new Mock<IQueryHandler<GetSomething, Something>>();
            _queryHandler
                .Setup(x => x.Handle(_getSomething))
                .Returns(_something);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveQueryHandler(_getSomething, typeof(IQueryHandler<,>)))
                .Returns(_queryHandler.Object);

            _sut = new QueryProcessor(_handlerResolver.Object, new Mock<ICacheManager>().Object, new Mock<IOptions<Options>>().Object);
        }
    
        [Test]
        public void Process_ThrowsException_WhenQueryIsNull()
        {
            _getSomething = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Process(_getSomething));
        }

        [Test]
        public void Process_ReturnsResult()
        {
            var result = _sut.Process(_getSomething);
            Assert.AreEqual(_something, result);
        }      
    }
}

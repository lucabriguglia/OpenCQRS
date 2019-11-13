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
        private Mock<ICacheManager> _cacheManager;
        private Mock<IOptions<Options>> _options;
        private Mock<IQueryHandler<GetSomething, Something>> _queryHandler;

        private GetSomething _getSomething;
        private GetSomethingCacheable _getSomethingCacheable;
        private Something _something;

        [SetUp]
        public void SetUp()
        {
            _getSomething = new GetSomething();
            _getSomethingCacheable = new GetSomethingCacheable();
            _something = new Something();

            _queryHandler = new Mock<IQueryHandler<GetSomething, Something>>();
            _queryHandler
                .Setup(x => x.Handle(_getSomething))
                .Returns(_something);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveQueryHandler(_getSomething, typeof(IQueryHandler<,>)))
                .Returns(_queryHandler.Object);

            _cacheManager = new Mock<ICacheManager>();
            _cacheManager
                .Setup(x => x.GetOrCreate(_getSomethingCacheable.CacheKey, It.IsAny<int>(), It.IsAny<Func<Something>>()))
                .Returns(_something);

            _options = new Mock<IOptions<Options>>();
            _options
                .Setup(x => x.Value)
                .Returns(new Options());

            _sut = new QueryProcessor(_handlerResolver.Object, _cacheManager.Object, _options.Object);
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

        [Test]
        public void Process_ReturnsResultFromCache()
        {
            var result = _sut.Process(_getSomethingCacheable);
            _cacheManager.Verify(x => x.GetOrCreate(_getSomethingCacheable.CacheKey, It.IsAny<int>(), It.IsAny<Func<Something>>()), Times.Once);
        }
    }
}

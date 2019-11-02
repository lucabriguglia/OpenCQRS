using System;
using System.Threading.Tasks;
using Kledex.Dependencies;
using Kledex.Queries;
using Kledex.Tests.Fakes;
using Moq;
using NUnit.Framework;

namespace Kledex.Tests.Queries
{
    [TestFixture]
    public class QueryProcessorAsyncTests
    {
        private IQueryProcessor _sut;

        private Mock<IHandlerResolver> _handlerResolver;
        private Mock<IQueryHandlerAsync<GetSomething, Something>> _queryHandler;

        private GetSomething _getSomething;
        private Something _something;

        [SetUp]
        public void SetUp()
        {
            _getSomething = new GetSomething();
            _something = new Something();

            _queryHandler = new Mock<IQueryHandlerAsync<GetSomething, Something>>();
            _queryHandler
                .Setup(x => x.HandleAsync(_getSomething))
                .ReturnsAsync(_something);

            _handlerResolver = new Mock<IHandlerResolver>();
            _handlerResolver
                .Setup(x => x.ResolveQueryHandler(_getSomething, typeof(IQueryHandlerAsync<,>)))
                .Returns(_queryHandler.Object);

            _sut = new QueryProcessor(_handlerResolver.Object);
        }
    
        [Test]
        public void ProcessAsync_ThrowsException_WhenQueryIsNull()
        {
            _getSomething = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.ProcessAsync(_getSomething));
        }

        [Test]
        public async Task ProcessAsync_ReturnResult()
        {
            var result = await _sut.ProcessAsync(_getSomething);
            Assert.AreEqual(_something, result);
        }      
    }
}

using System;
using System.Threading.Tasks;
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

        private Mock<IQueryHandlerResolver> _queryHandlerResolver;
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

            _queryHandlerResolver = new Mock<IQueryHandlerResolver>();
            _queryHandlerResolver
                .Setup(x => x.ResolveHandler(_getSomething, typeof(IQueryHandlerAsync<,>)))
                .Returns(_queryHandler.Object);

            _sut = new QueryProcessor(_queryHandlerResolver.Object);
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

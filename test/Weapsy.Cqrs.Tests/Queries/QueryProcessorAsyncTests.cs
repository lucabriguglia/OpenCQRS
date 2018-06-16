using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Weapsy.Cqrs.Dependencies;
using Weapsy.Cqrs.Queries;
using Weapsy.Cqrs.Tests.Fakes;

namespace Weapsy.Cqrs.Tests.Queries
{
    [TestFixture]
    public class QueryProcessorAsyncTests
    {
        private IQueryProcessorAsync _sut;

        private Mock<IResolver> _resolver;

        private Mock<IQueryHandlerAsync<GetSomething, Something>> _queryHendler;

        private GetSomething _getSomething;
        private Something _something;

        [SetUp]
        public void SetUp()
        {
            _getSomething = new GetSomething();
            _something = new Something();

            _queryHendler = new Mock<IQueryHandlerAsync<GetSomething, Something>>();
            _queryHendler
                .Setup(x => x.RetrieveAsync(_getSomething))
                .ReturnsAsync(_something);

            _resolver = new Mock<IResolver>();
            _resolver
                .Setup(x => x.Resolve<IQueryHandlerAsync<GetSomething, Something>>())
                .Returns(_queryHendler.Object);

            _sut = new QueryProcessorAsync(_resolver.Object);
        }
    
        [Test]
        public void DispatchThrowsExceptionWhenQueryIsNull()
        {
            _getSomething = null;
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _sut.ProcessAsync<GetSomething, Something>(_getSomething));
        }

        [Test]
        public void DispatchThrowsExceptionWhenQueryHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<IQueryHandlerAsync<GetSomething, Something>>())
                .Returns((IQueryHandlerAsync<GetSomething, Something>)null);
            Assert.ThrowsAsync<ApplicationException>(async () => await _sut.ProcessAsync<GetSomething, Something>(_getSomething));
        }

        [Test]
        public async Task DispatchReturnResult()
        {
            var result = await _sut.ProcessAsync<GetSomething, Something>(_getSomething);
            Assert.AreEqual(_something, result);
        }      
    }
}

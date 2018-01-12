using System;
using Moq;
using NUnit.Framework;
using Weapsy.Mediator.Dependencies;
using Weapsy.Mediator.Queries;
using Weapsy.Mediator.Tests.Fakes;

namespace Weapsy.Mediator.Tests
{
    [TestFixture]
    public class QueryDistacherTests
    {
        private IQueryDispatcher _sut;

        private Mock<IResolver> _resolver;

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

            _resolver = new Mock<IResolver>();
            _resolver
                .Setup(x => x.Resolve<IQueryHandler<GetSomething, Something>>())
                .Returns(_queryHendler.Object);

            _sut = new QueryDispatcher(_resolver.Object);
        }
    
        [Test]
        public void DispatchThrowsExceptionWhenQueryIsNull()
        {
            _getSomething = null;
            Assert.Throws<ArgumentNullException>(() => _sut.Dispatch<GetSomething, Something>(_getSomething));
        }

        [Test]
        public void DispatchThrowsExceptionWhenQueryHandlerIsNotFound()
        {
            _resolver
                .Setup(x => x.Resolve<IQueryHandler<GetSomething, Something>>())
                .Returns((IQueryHandler<GetSomething, Something>)null);
            Assert.Throws<ApplicationException>(() => _sut.Dispatch<GetSomething, Something>(_getSomething));
        }

        [Test]
        public void DispatchReturnResult()
        {
            var result = _sut.Dispatch<GetSomething, Something>(_getSomething);
            Assert.AreEqual(_something, result);
        }      
    }
}

using System.Threading.Tasks;
using Castle.DynamicProxy;
using NUnit.Framework;

namespace RetriableExample.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        private IRepository _proxy;

        [SetUp]
        public void SetUp()
        {
            var repository = new Repository();
            var generator = new ProxyGenerator();
            var interceptor = new RetriableReadAsyncInterceptor();
            _proxy = generator.CreateInterfaceProxyWithTargetInterface<IRepository>(repository, interceptor);
        }

        [Test]
        public void Should_retry_when_db_exception_is_thrown_sync()
        {
            //Act
            var entity = _proxy.GetById(1);

            //Assert
            Assert.IsNotNull(entity);
        }

        [Test]
        public async Task Should_retry_when_db_exception_is_thrown_async()
        {
            //Act
            var entity = await _proxy.GetByIdAsync(1);

            //Assert
            Assert.IsNotNull(entity);
        }
    }
}

using NUnit.Framework;
using Remake_CB_4_Project_1.Persistance;

namespace Remake_CB_4_Project_1.Core
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [Test]
        public void IsSingletonTest()
        {
            var unitOfWork = UnitOfWork.Instance;
            var unitOfWork1 = UnitOfWork.Instance;
            Assert.That(unitOfWork, Is.SameAs(unitOfWork1));
            Assert.That(UnitOfWork.Count, Is.EqualTo(1));
        }
    }
}

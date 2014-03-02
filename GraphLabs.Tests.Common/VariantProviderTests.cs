using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Common;
using Moq;
using NUnit.Framework;

namespace GraphLabs.Tests.Common
{
    [TestFixture]
    public class VariantProviderTests
    {
        private const long TaskId = 974;
        private readonly Guid _sessionGuid = new Guid("C0FAF8AB-8EF9-4B8D-AAA0-15DCDB45B037");

        [Test]
        public void TestGetVariant()
        {
            const string variantData = "Пусть это будут данные варианта. То, что это на самом деле строка, не принципиально.";

            var dataServiceMock = new Mock<ITasksDataServiceClient>(MockBehavior.Strict);
            dataServiceMock
                .Setup(srv => srv.GetVariantAsync(It.Is<long>(l => l == TaskId), It.Is<Guid>(g => g == _sessionGuid)))
                .Callback(() => dataServiceMock.Raise(mock => mock.GetVariantCompleted += null,
                    new GetVariantCompletedEventArgs(new object[] { variantData }, null, false, null)))
                .Verifiable();
            SetupCloseAsync(dataServiceMock);

            using (var wrapper = new DisposableWcfClientWrapper<ITasksDataServiceClient>(dataServiceMock.Object))
            {
                var variantProvider = new VariantProvider(wrapper);
                
            }

        }


        private static void SetupCloseAsync(Mock<ITasksDataServiceClient> registratorMock)
        {
            registratorMock
                .Setup(reg => reg.CloseAsync())
                .Callback(() => registratorMock.Raise(mock => mock.CloseCompleted += null, new AsyncCompletedEventArgs(null, false, null)))
                .Verifiable();
        }
    }
}

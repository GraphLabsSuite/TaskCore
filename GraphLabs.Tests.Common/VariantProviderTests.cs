using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
            var allowedVersion = new Version(1, 0, 234, 999);

            var dataServiceMock = new Mock<ITasksDataServiceClient>(MockBehavior.Strict);
            var taskVariantInfo = new TaskVariantInfo
            {
                Data = Encoding.UTF8.GetBytes(variantData),
                GeneratorVersion = allowedVersion.ToString(),
                Id = 7,
                Number = "test",
                Version = 1
            };

            dataServiceMock
                .Setup(srv => srv.GetVariantAsync(It.Is<long>(l => l == TaskId), It.Is<Guid>(g => g == _sessionGuid)))
                .Callback(() => dataServiceMock.Raise(mock => mock.GetVariantCompleted += null,
                    new GetVariantCompletedEventArgs(new object[] { taskVariantInfo }, null, false, null)))
                .Verifiable();
            SetupCloseAsync(dataServiceMock);

            using (var wrapper = DisposableWcfClientWrapper.Create(dataServiceMock.Object))
            {
                var variantProvider = new VariantProvider(
                    TaskId,
                    _sessionGuid,
                    new[] {new Version(allowedVersion.Major, allowedVersion.Minor + 1)},
                    wrapper);

                using (var flag = new AutoResetEvent(false))
                {
                    var handled = false;
                    EventHandler<VariantDownloadedEventArgs> handler = (sender, args) =>
                    {
                        Assert.AreSame(sender, variantProvider);
                        Assert.NotNull(args);
                        Assert.That(args.Data.SequenceEqual(taskVariantInfo.Data));
                        Assert.AreEqual(taskVariantInfo.Number, args.Number);
                        Assert.AreEqual(taskVariantInfo.Number, args.Number);
                        handled = true;
                        flag.Set();
                    };

                    variantProvider.VariantDownloaded += handler;
                    variantProvider.DownloadVariantAsync();
                    flag.WaitOne(1000);
                    variantProvider.VariantDownloaded -= handler;

                    Assert.IsTrue(handled);
                }
            }

            dataServiceMock.Verify();
        }

        [Test]
        public void TestRetryDownload()
        {
            const string variantData = "Пусть это будут данные варианта. То, что это на самом деле строка, не принципиально.";
            var allowedVersion = new Version(1, 0);

            var dataServiceMock = new Mock<ITasksDataServiceClient>(MockBehavior.Strict);
            var taskVariantInfo = new TaskVariantInfo
            {
                Data = Encoding.UTF8.GetBytes(variantData),
                GeneratorVersion = allowedVersion.ToString(),
                Id = 7,
                Number = "test",
                Version = 1
            };

            dataServiceMock
                .Setup(srv => srv.GetVariantAsync(It.Is<long>(l => l == TaskId), It.Is<Guid>(g => g == _sessionGuid)))
                .Callback(() => dataServiceMock.Raise(mock => mock.GetVariantCompleted += null,
                    new GetVariantCompletedEventArgs(new object[] { taskVariantInfo }, null, false, null)))
                .Verifiable();
            SetupCloseAsync(dataServiceMock);

            using (var wrapper = DisposableWcfClientWrapper.Create(dataServiceMock.Object))
            {
                var variantProvider = new VariantProvider(TaskId, _sessionGuid, new[] { allowedVersion }, wrapper);
                using (var flag = new AutoResetEvent(false))
                {
                    var handled = false;
                    EventHandler<VariantDownloadedEventArgs> handler = (sender, args) =>
                    {
                        Assert.AreSame(sender, variantProvider);
                        Assert.NotNull(args);
                        Assert.That(args.Data.SequenceEqual(taskVariantInfo.Data));
                        Assert.AreEqual(taskVariantInfo.Number, args.Number);
                        Assert.AreEqual(taskVariantInfo.Number, args.Number);
                        handled = true;
                        flag.Set();
                    };

                    variantProvider.VariantDownloaded += handler;
                    variantProvider.DownloadVariantAsync();
                    Assert.Throws<InvalidOperationException>(variantProvider.DownloadVariantAsync);
                    flag.WaitOne(1000);
                    variantProvider.VariantDownloaded -= handler;

                    Assert.IsTrue(handled);
                }
            }

            dataServiceMock.Verify();
        }

        [Test]
        public void TestInvalidVersion()
        {
            const string variantData = "Пусть это будут данные варианта. То, что это на самом деле строка, не принципиально.";
            var allowedVersion = new Version(1, 0);

            var dataServiceMock = new Mock<ITasksDataServiceClient>(MockBehavior.Strict);
            var taskVariantInfo = new TaskVariantInfo
            {
                Data = Encoding.UTF8.GetBytes(variantData),
                GeneratorVersion = allowedVersion.ToString(),
                Id = 7,
                Number = "test",
                Version = 1
            };

            Assert.Throws<Exception>(() =>
            {
                dataServiceMock
                    .Setup(
                        srv => srv.GetVariantAsync(It.Is<long>(l => l == TaskId), It.Is<Guid>(g => g == _sessionGuid)))
                    .Callback(() => dataServiceMock.Raise(mock => mock.GetVariantCompleted += null,
                        new GetVariantCompletedEventArgs(new object[] {taskVariantInfo}, null, false, null)))
                    .Verifiable();
                SetupCloseAsync(dataServiceMock);

                using (var wrapper = DisposableWcfClientWrapper.Create(dataServiceMock.Object))
                {
                    var variantProvider = new VariantProvider(
                        TaskId,
                        _sessionGuid,
                        new[] {new Version(allowedVersion.Major + 1, allowedVersion.Minor)},
                        wrapper);
                    variantProvider.DownloadVariantAsync();
                }
            });
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

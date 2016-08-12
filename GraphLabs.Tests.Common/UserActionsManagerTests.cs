using System;
using System.ComponentModel;
using System.Linq;
using Autofac;
using GraphLabs.Common;
using GraphLabs.Common.UserActionsRegistrator;
using GraphLabs.Common.Utils;
using GraphLabs.Utils.Services;
using Moq;
using NUnit.Framework;
using IContainer = Autofac.IContainer;

namespace GraphLabs.Tests.Common
{
    [TestFixture]
    public class UserActionsManagerTests
    {
        private const long TaskId = 974;
        private readonly Guid _sessionGuid = new Guid("C0FAF8AB-8EF9-4B8D-AAA0-15DCDB45B037");
        private readonly DateTime _someMoment = new DateTime(2014, 01, 01, 00, 00, 00);
        private IContainer _container;

        [TestFixtureSetUp]
        public void BeforeAnyTestsRun()
        {
            var dateServiceMock = new Mock<IDateTimeService>();
            dateServiceMock.Setup(s => s.Now()).Returns(_someMoment);

            var builder = new ContainerBuilder();
            builder.RegisterInstance(dateServiceMock.Object).As<IDateTimeService>();

            _container = builder.Build();
        }

        private UserActionsManager CreateUAManager(DisposableWcfClientWrapper<IUserActionsRegistratorClient> registratorMock, IDateTimeService dateService)
        {
            return new UserActionsManager(TaskId, _sessionGuid, registratorMock, dateService);
        }

        [Test]
        // Тут ещё проверяется синхронизация "очков" при отправке сообщений
        public void TestInfoRegistrationWithSending()
        {
            const string action1Descr = "action1Descr";
            const int newScore = 77;

            var dateService = _container.Resolve<IDateTimeService>();

            // Arrange
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Strict);
            registratorMock
                .Setup(reg => reg.RegisterUserActionsAsync(
                    It.Is<long>(i => i == TaskId),
                    It.Is<Guid>(i => i == _sessionGuid),
                    It.Is<ActionDescription[]>(d =>
                        d.Single().Description == action1Descr &&
                        d.Single().Penalty == 0 &&
                        d.Single().TimeStamp == dateService.Now()),
                    It.Is<bool>(i => i == false)))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] {newScore}, null, false, null)))
                .Verifiable();

            SetupCloseAsync(registratorMock);

            using (var wrapper = DisposableWcfClientWrapper.Create(registratorMock.Object))
            {
                var manager = CreateUAManager(wrapper, dateService);
                manager.SendReportOnEveryAction = true;

                // Action
                manager.RegisterInfo(action1Descr);

                // Assert
                Assert.AreEqual(newScore, manager.Score);
            }

            registratorMock.Verify();
        }


        [Test]
        public void TestInfoRegistrationWithoutSending()
        {
            const string action1Descr = "action1Descr";

            var dateService = _container.Resolve<IDateTimeService>();

            // Arrange
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Strict);
            SetupCloseAsync(registratorMock);

            using (var wrapper = DisposableWcfClientWrapper.Create(registratorMock.Object))
            {
                var manager = CreateUAManager(wrapper, dateService);
                manager.SendReportOnEveryAction = false;

                // Action
                manager.RegisterInfo(action1Descr);

                // Assert
                Assert.AreEqual(UserActionsManager.StartingScore, manager.Score);
            }

            registratorMock.Verify();
        }

        [Test]
        public void TestSingleMistakeRegistration()
        {
            const string action1Descr = "action1Descr";
            const int penalty = 10;
            const int newScore = UserActionsManager.StartingScore - penalty;

            var dateService = _container.Resolve<IDateTimeService>();

            // Arrange
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Strict);
            registratorMock
                .Setup(reg => reg.RegisterUserActionsAsync(
                    It.Is<long>(i => i == TaskId),
                    It.Is<Guid>(i => i == _sessionGuid),
                    It.Is<ActionDescription[]>(d =>
                        d.Single().Description == action1Descr &&
                        d.Single().Penalty == penalty &&
                        d.Single().TimeStamp == dateService.Now()),
                    It.Is<bool>(i => i == false)))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] { newScore }, null, false, null)))
                .Verifiable();

            SetupCloseAsync(registratorMock);

            using (var wrapper = DisposableWcfClientWrapper.Create(registratorMock.Object))
            {
                var manager = CreateUAManager(wrapper, dateService);
                manager.SendReportOnEveryAction = false;

                // Action
                manager.RegisterMistake(action1Descr, penalty);

                // Assert
                Assert.AreEqual(newScore, manager.Score);
            }

            registratorMock.Verify();
        }

        [Test]
        public void TestMistakeAfterInfoWithSending()
        {
            const string action1Descr = "action1Descr";
            const string action2Descr = "action2Descr";
            const int penalty = 10;
            const int newScore = UserActionsManager.StartingScore - penalty;

            var dateService = _container.Resolve<IDateTimeService>();

            // Arrange
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Strict);

            registratorMock
                .Setup(reg => reg.RegisterUserActionsAsync(
                    It.Is<long>(i => i == TaskId),
                    It.Is<Guid>(i => i == _sessionGuid),
                    It.Is<ActionDescription[]>(d =>
                        d.Single().Description == action1Descr &&
                        d.Single().Penalty == 0 &&
                        d.Single().TimeStamp == dateService.Now()),
                    It.Is<bool>(i => i == false)))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] { UserActionsManager.StartingScore }, null, false, null)))
                .Verifiable();

            registratorMock
                .Setup(reg => reg.RegisterUserActionsAsync(
                    It.Is<long>(i => i == TaskId),
                    It.Is<Guid>(i => i == _sessionGuid),
                    It.Is<ActionDescription[]>(d =>
                        d.Single().Description == action2Descr &&
                        d.Single().Penalty == penalty &&
                        d.Single().TimeStamp == dateService.Now()),
                    It.Is<bool>(i => i == false)))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] {newScore}, null, false, null)))
                .Verifiable();

            SetupCloseAsync(registratorMock);

            using (var wrapper = DisposableWcfClientWrapper.Create(registratorMock.Object))
            {
                var manager = CreateUAManager(wrapper, dateService);
                manager.SendReportOnEveryAction = true;

                // Action
                manager.RegisterInfo(action1Descr);
                manager.RegisterMistake(action2Descr, penalty);

                // Assert
                Assert.AreEqual(newScore, manager.Score);
            }

            registratorMock.Verify();
        }

        [Test]
        public void TestMistakeAfterInfoWithoutSending()
        {
            const string action1Descr = "action1Descr";
            const string action2Descr = "action2Descr";
            const int penalty = 10;
            const int newScore = UserActionsManager.StartingScore - penalty;

            var dateService = _container.Resolve<IDateTimeService>();

            // Arrange
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Strict);

            registratorMock
                .Setup(reg => reg.RegisterUserActionsAsync(
                    It.Is<long>(i => i == TaskId),
                    It.Is<Guid>(i => i == _sessionGuid),
                    It.Is<ActionDescription[]>(d => d.Count() == 2 &&
                        d[0].Description == action1Descr &&
                        d[0].Penalty == 0 &&
                        d[0].TimeStamp == dateService.Now() &&
                        d[1].Description == action2Descr &&
                        d[1].Penalty == penalty &&
                        d[1].TimeStamp == dateService.Now()),
                    It.Is<bool>(i => i == false)))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] { newScore }, null, false, null)))
                .Verifiable();

            SetupCloseAsync(registratorMock);

            using (var wrapper = DisposableWcfClientWrapper.Create(registratorMock.Object))
            {
                var manager = CreateUAManager(wrapper, dateService);
                manager.SendReportOnEveryAction = false;

                // Action
                manager.RegisterInfo(action1Descr);
                manager.RegisterMistake(action2Descr, penalty);

                // Assert
                Assert.AreEqual(newScore, manager.Score);
            }

            registratorMock.Verify();
        }

        [Test]
        public void TestFinishTaskAfterInfoWithoutSending()
        {
            const string action1Descr = "action1Descr";

            var dateService = _container.Resolve<IDateTimeService>();

            // Arrange
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Strict);

            registratorMock
                .Setup(reg => reg.RegisterUserActionsAsync(
                    It.Is<long>(i => i == TaskId),
                    It.Is<Guid>(i => i == _sessionGuid),
                    It.Is<ActionDescription[]>(d =>
                        d.Single().Description == action1Descr &&
                        d.Single().Penalty == 0 &&
                        d.Single().TimeStamp == dateService.Now()),
                    It.Is<bool>(i => i == true)))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] { UserActionsManager.StartingScore }, null, false, null)))
                .Verifiable();

            SetupCloseAsync(registratorMock);

            using (var wrapper = DisposableWcfClientWrapper.Create(registratorMock.Object))
            {
                var manager = CreateUAManager(wrapper, dateService);
                manager.SendReportOnEveryAction = false;

                // Action
                manager.RegisterInfo(action1Descr);
                manager.ReportThatTaskFinishedAsync();

                // Assert
                Assert.AreEqual(UserActionsManager.StartingScore, manager.Score);
            }

            registratorMock.Verify();
        }

        [Test]
        public void TestFinishTaskAfterMistake()
        {
            const string action1Descr = "action1Descr";
            const int penalty = 10;
            const int newScore = UserActionsManager.StartingScore - penalty;

            var dateService = _container.Resolve<IDateTimeService>();

            // Arrange
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Strict);

            registratorMock
                .Setup(reg => reg.RegisterUserActionsAsync(
                    It.Is<long>(i => i == TaskId),
                    It.Is<Guid>(i => i == _sessionGuid),
                    It.Is<ActionDescription[]>(d => d.Count() == 1 &&
                        d.Single().Description == action1Descr &&
                        d.Single().Penalty == penalty &&
                        d.Single().TimeStamp == dateService.Now()),
                    It.Is<bool>(i => i == false)))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] { newScore }, null, false, null)))
                .Verifiable();

            registratorMock.Setup(reg => reg.RegisterUserActionsAsync(
                It.Is<long>(i => i == TaskId),
                It.Is<Guid>(i => i == _sessionGuid),
                It.Is<ActionDescription[]>(d => !d.Any()),
                It.Is<bool>(i => i == true)))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] { newScore }, null, false, false)));

            SetupCloseAsync(registratorMock);

            using (var wrapper = DisposableWcfClientWrapper.Create(registratorMock.Object))
            {
                var manager = CreateUAManager(wrapper, dateService);
                manager.SendReportOnEveryAction = false;

                // Action
                manager.RegisterMistake(action1Descr, penalty);
                manager.ReportThatTaskFinishedAsync();

                // Assert
                Assert.AreEqual(newScore, manager.Score);
            }

            registratorMock.Verify();
        }

        [Test]
        public void TestScoreChanged()
        {
            const string descr = "TestScoreChanged";
            const int penalty = 10;
            const int newScore = UserActionsManager.StartingScore - penalty;

            var dateService = _container.Resolve<IDateTimeService>();
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Loose);
            registratorMock.Setup(reg => reg.RegisterUserActionsAsync(
                It.IsAny<long>(),
                It.IsAny<Guid>(),
                It.IsAny<ActionDescription[]>(),
                It.IsAny<bool>()))
                .Callback(() => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                    new RegisterUserActionsCompletedEventArgs(new object[] {newScore}, null, false, false)));
                
            var flag = false;
            using (var wrapper = DisposableWcfClientWrapper.Create(registratorMock.Object))
            {
                var manager = CreateUAManager(wrapper, dateService);
                manager.PropertyChanged += (sender, args) => 
                    flag = args.PropertyName == ExpressionUtility.NameForMember((UserActionsManager m) => m.Score);
                manager.RegisterMistake(descr, penalty);

                Assert.That(manager.Score == newScore);
            }
        }

        private static void SetupCloseAsync(Mock<IUserActionsRegistratorClient> registratorMock)
        {
            registratorMock
                .Setup(reg => reg.CloseAsync())
                .Callback(() => registratorMock.Raise(mock => mock.CloseCompleted += null, new AsyncCompletedEventArgs(null, false, null)))
                .Verifiable();
        }
    }
}

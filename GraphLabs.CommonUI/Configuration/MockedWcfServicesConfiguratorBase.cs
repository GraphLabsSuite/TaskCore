using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphLabs.Common;
using GraphLabs.Common.TasksDataService;
using GraphLabs.Common.UserActionsRegistrator;
using Moq;

namespace GraphLabs.CommonUI.Configuration
{
    /// <summary> Конфигуратор заглушек WCF-сервисов </summary>
    public abstract class MockedWcfServicesConfiguratorBase : WcfServicesConfiguratorBase
    {
        private int _currentScore = UserActionsManager.StartingScore;
        private int _gettingVariantDelay = 1000;

        /// <summary> Задержка по-умолчанию для получения варианта </summary>
        public int GettingVariantDelay
        {
            get { return _gettingVariantDelay; }
            set { _gettingVariantDelay = value; }
        }

        /// <summary> Получить клиент регистратора действий </summary>
        protected override IUserActionsRegistratorClient GetActionRegistratorClient()
        {
            var registratorMock = new Mock<IUserActionsRegistratorClient>(MockBehavior.Loose);

            registratorMock.Setup(reg => reg.RegisterUserActionsAsync(
                It.IsAny<long>(),
                It.IsAny<Guid>(),
                It.Is<ActionDescription[]>(d => d.Count() == 1 && d[0].Penalty == 0),
                It.IsAny<bool>()))
                .Callback(() =>
                    registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                        new RegisterUserActionsCompletedEventArgs(new object[] { _currentScore }, null, false, null)));

            registratorMock.Setup(reg => reg.RegisterUserActionsAsync(
                It.IsAny<long>(),
                It.IsAny<Guid>(),
                It.Is<ActionDescription[]>(d => d.Count() == 1 && d[0].Penalty != 0),
                It.IsAny<bool>()))
                .Callback<long, Guid, ActionDescription[], bool>(
                    (l, g, d, b) => registratorMock.Raise(mock => mock.RegisterUserActionsCompleted += null,
                        new RegisterUserActionsCompletedEventArgs(new object[] { _currentScore = _currentScore - d[0].Penalty },
                            null, false, null)));

            return registratorMock.Object;
        }

        /// <summary> Получить клиент поставщика вариантов </summary>
        protected override ITasksDataServiceClient GetDataServiceClient()
        {
            var debugVariant = GetDebugVariant();

            var dataServiceMock = new Mock<ITasksDataServiceClient>(MockBehavior.Loose);
            dataServiceMock.Setup(srv => srv.GetVariantAsync(It.IsAny<long>(), It.IsAny<Guid>()))
                .Callback(() =>
                          {
                              var delay = GettingVariantDelay;
                              Task.Factory.StartNew(() =>
                              {
                                  Thread.Sleep(delay);
                                  dataServiceMock.Raise(mock => mock.GetVariantCompleted += null,
                                      new GetVariantCompletedEventArgs(new object[] {debugVariant}, null, false, null));
                              });
                          });

            return dataServiceMock.Object;
        }

        /// <summary> Получить отладочный вариант </summary>
        protected abstract TaskVariantDto GetDebugVariant();
    }
}
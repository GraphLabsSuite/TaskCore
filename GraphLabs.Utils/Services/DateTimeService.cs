using System;

namespace GraphLabs.Utils.Services
{
    /// <summary> Сервис даты-времени </summary>
    public class DateTimeService : IDateTimeService
    {
        /// <summary> Сейчас </summary>
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}

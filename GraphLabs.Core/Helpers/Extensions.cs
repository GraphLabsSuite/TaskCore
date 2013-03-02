using System;
using System.Collections.Generic;

namespace GraphLabs.Tasks.Core.Helpers
{
    /// <summary> Вспомогательный класс с расширениями для коллекций </summary>
    public static class Extensions
    {
        /// <summary> Выполняет действие action для каждого объекта коллекции </summary>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var el in collection)
            {
                action(el);
            }
        }

        /// <summary> Выполняет действие action для каждого объекта коллекции </summary>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> range)
        {
            foreach (var el in range)
            {
                collection.Add(el);
            }
        }

    }
}

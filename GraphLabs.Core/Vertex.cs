using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Core
{
    /// <summary> Вершина графа. </summary>
    public class Vertex : IVertex
    {
        /// <summary> Возвращает имя вершины. </summary>
        public string Name { get; set; }

        /// <summary> Создаёт новую вершину с именем Name. </summary>
        public Vertex(string name)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));

            Name = name;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}

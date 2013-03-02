
using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Tasks.Core
{
    /// <summary> Ребро / дуга графа </summary>
    public abstract class Edge : IEdge
    {
        #region Реализация IEdge

        /// <summary> Индекс вершины 1 (вершины-истока) </summary>
        public IVertex Vertex1 { get; private set; }

        /// <summary> Индекс вершины 2 (вершины-стока) </summary>
        public IVertex Vertex2 { get; private set; }

        /// <summary> Ребро ориентированное? (является дугой?) </summary>
        public abstract bool Directed { get; }

        #endregion // Реализация IEdge

        /// <summary> Создаёт новое ребро по указанным параметрам </summary>
        /// <param name="vertex1">Вершина 1</param>
        /// <param name="vertex2">Вершина 2</param>
        protected Edge(IVertex vertex1, IVertex vertex2)
        {
            Contract.Requires<NullReferenceException>(vertex1 != null && vertex2 != null);

            Vertex1 = vertex1;
            Vertex2 = vertex2;
        }

        /// <summary> Returns a string that represents the current object. </summary>
        /// <returns> A string that represents the current object. </returns>
        public abstract override string ToString();
    }
}

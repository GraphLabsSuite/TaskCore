using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Core
{
    /// <summary> Ребро / дуга графа </summary>
    public abstract class Edge : IEdge<Vertex>
    {
        #region Реализация IEdge

        /// <summary> Индекс вершины 1 (вершины-истока) </summary>
        public Vertex Vertex1 { get; private set; }

        /// <summary> Индекс вершины 2 (вершины-стока) </summary>
        public Vertex Vertex2 { get; private set; }

        /// <summary> Индекс вершины 1 (вершины-истока) </summary>
        IVertex IEdge.Vertex1
        {
            get { return Vertex1; }
        }

        /// <summary> Индекс вершины 2 (вершины-стока) </summary>
        IVertex IEdge.Vertex2
        {
            get { return Vertex2; }
        }

        /// <summary> Ребро ориентированное? (является дугой?) </summary>
        public abstract bool Directed { get; }

        #endregion // Реализация IEdge

        /// <summary> Создаёт новое ребро по указанным параметрам </summary>
        /// <param name="vertex1">Вершина 1</param>
        /// <param name="vertex2">Вершина 2</param>
        protected Edge(Vertex vertex1, Vertex vertex2)
        {
            Contract.Requires<ArgumentNullException>(vertex1 != null && vertex2 != null);

            Vertex1 = vertex1;
            Vertex2 = vertex2;
        }

        /// <summary> Returns a string that represents the current object. </summary>
        /// <returns> A string that represents the current object. </returns>
        public abstract override string ToString();

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        public abstract object Clone();

        /// <summary> GetHashCode </summary>
        public override int GetHashCode()
        {
            return Vertex1.GetHashCode() ^ Vertex2.GetHashCode() ^ Directed.GetHashCode();
        }

        #region Сравнение
        
        /// <summary> Сравнение рёбер </summary>
        public bool Equals(IEdge other)
        {
            return this == other;
        }

        /// <summary> Сравниваем </summary>
        public override bool Equals(object obj)
        {
            var e = obj as IEdge;
            return this == e;
        }

        /// <summary> Сравниваем </summary>
        public static bool operator ==(Edge e1, IEdge e2)
        {
            return EqualityComparer.EdgesEquals(e1, e2);
        }

        /// <summary> Сравниваем </summary>
        public static bool operator !=(Edge e1, IEdge e2)
        {
            return !(e1 == e2);
        }

        /// <summary> Сравниваем </summary>
        public static bool operator ==(IEdge e1, Edge e2)
        {
            return e2 == e1;
        }

        /// <summary> Сравниваем </summary>
        public static bool operator !=(IEdge e1, Edge e2)
        {
            return !(e2 == e1);
        }

        /// <summary> Сравниваем </summary>
        public static bool operator ==(Edge e1, Edge e2)
        {
            return e2 == (IEdge)e1;
        }

        /// <summary> Сравниваем </summary>
        public static bool operator !=(Edge e1, Edge e2)
        {
            return !(e2 == e1);
        }

        #endregion
    }
}

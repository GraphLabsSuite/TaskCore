

using System;

namespace GraphLabs.Graphs
{
    /// <summary> Ориентированное ребро </summary>
    public class DirectedEdge : Edge, IEquatable<DirectedEdge>
    {
        /// <summary> Сравнение рёбер </summary>
        public bool Equals(DirectedEdge other)
        {
            return Equals((IEdge)other);
        }

        /// <summary> Ребро ориентированное? (является дугой?) </summary>
        public override bool Directed
        {
            get { return true; }
        }

        /// <summary> Returns a string that represents the current object. </summary>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            return string.Format("{0}->{1}", Vertex1, Vertex2);
        }

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        public override object Clone()
        {
            return new DirectedEdge((Vertex)Vertex1.Clone(), (Vertex)Vertex2.Clone());
        }

        /// <summary> Создаёт новое ребро по указанным параметрам </summary>
        /// <param name="vertex1">Вершина 1</param>
        /// <param name="vertex2">Вершина 2</param>
        public DirectedEdge(Vertex vertex1, Vertex vertex2) :
            base(vertex1, vertex2)
        {
        }
    }
}

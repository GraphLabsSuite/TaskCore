
using System;

namespace GraphLabs.Graphs
{
    /// <summary> Ребро графа </summary>
    public class UndirectedEdge : Edge, IEquatable<UndirectedEdge>
    {
        /// <summary> Ребро ориентированное? (является дугой?) </summary>
        public override bool Directed
        {
            get { return false; }
        }

        /// <summary> Сравниваем </summary>
        public bool Equals(UndirectedEdge other)
        {
            return Equals((IEdge)other);
        }

        /// <summary> Returns a string that represents the current object. </summary>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            return string.Format("{0}--{1}", Vertex1, Vertex2);
        }

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        public override object Clone()
        {
            return new UndirectedEdge((Vertex)Vertex1.Clone(), (Vertex)Vertex2.Clone());
        }

        /// <summary> Создаёт новое ребро по указанным параметрам </summary>
        /// <param name="vertex1">Вершина 1</param>
        /// <param name="vertex2">Вершина 2</param>
        public UndirectedEdge(Vertex vertex1, Vertex vertex2) :
            base(vertex1, vertex2)
        {
        }
    }
}

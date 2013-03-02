
namespace GraphLabs.Core
{
    /// <summary> Ребро графа </summary>
    public class UndirectedEdge : Edge
    {
        /// <summary> Ребро ориентированное? (является дугой?) </summary>
        public override bool Directed
        {
            get { return false; }
        }

        /// <summary> Returns a string that represents the current object. </summary>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            return string.Format("{0}--{1}", Vertex1, Vertex2);
        }

        /// <summary> Создаёт новое ребро по указанным параметрам </summary>
        /// <param name="vertex1">Вершина 1</param>
        /// <param name="vertex2">Вершина 2</param>
        public UndirectedEdge(IVertex vertex1, IVertex vertex2) :
            base(vertex1, vertex2)
        {
        }
    }
}

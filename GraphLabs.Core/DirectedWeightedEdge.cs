
namespace GraphLabs.Core
{
    /// <summary> Взвешенная дуга </summary>
    public class DirectedWeightedEdge : DirectedEdge, IWeightedEdge
    {
        /// <summary> Returns a string that represents the current object. </summary>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            return string.Format("{0}--({2})-->{1}", Vertex1, Vertex2, Weight);
        }

        /// <summary> Вес </summary>
        public int Weight { get; private set; }

        /// <summary> Создаёт новое ребро по указанным параметрам</summary>
        /// <param name="vertex1">Вершина 1</param>
        /// <param name="vertex2">Вершина 2</param>
        /// <param name="weight">Вес</param>
        public DirectedWeightedEdge(IVertex vertex1, IVertex vertex2, int weight) :
            base(vertex1, vertex2)
        {
            Weight = weight;
        }
    }
}

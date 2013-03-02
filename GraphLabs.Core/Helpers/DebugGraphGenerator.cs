#if DEBUG
using System.IO;
using System.Runtime.Serialization;
using GraphLabs.Tasks.Core.DataTransferObjects;
using GraphLabs.Tasks.Core.DataTransferObjects.Converters;

namespace GraphLabs.Tasks.Core.Helpers
{
    /// <summary> Для использования в дебаге из веб-сервисов </summary>
    public static class DebugGraphGenerator
    {
        /// <summary>asdsf </summary>
        public static byte[] GetSerializedGraph()
        {
            var graph = DirectedGraph.CreateEmpty(10);
            graph.AddEdge(new DirectedEdge(graph.Vertices[0], graph.Vertices[1]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[1], graph.Vertices[2]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[2], graph.Vertices[3]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[3], graph.Vertices[4]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[4], graph.Vertices[0]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[5], graph.Vertices[6]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[6], graph.Vertices[7]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[7], graph.Vertices[8]));

            return GraphSerializer.Serialize(graph);
        }
    }
}
#endif
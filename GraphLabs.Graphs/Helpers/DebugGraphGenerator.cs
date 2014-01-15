using GraphLabs.Graphs.DataTransferObjects.Converters;
#if DEBUG


namespace GraphLabs.Graphs.Helpers
{
    /// <summary> Для использования в дебаге из веб-сервисов </summary>
    public static class DebugGraphGenerator
    {
        /// <summary>asdsf </summary>
        public static byte[] GetSerializedGraph()
        {
            var graph = DirectedGraph.CreateEmpty(7);
            graph.AddEdge(new DirectedEdge(graph.Vertices[0], graph.Vertices[5]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[1], graph.Vertices[0]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[1], graph.Vertices[5]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[2], graph.Vertices[1]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[2], graph.Vertices[5]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[3], graph.Vertices[4]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[4], graph.Vertices[2]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[4], graph.Vertices[3]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[5], graph.Vertices[6]));
            graph.AddEdge(new DirectedEdge(graph.Vertices[6], graph.Vertices[4]));

            return GraphSerializer.Serialize(graph);
        }

        /// <summary>asdsf </summary>
        public static byte[] GetSerializedWeightedGraph()
        {
            var graph = DirectedWeightedGraph.CreateEmpty(8);
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[0], graph.Vertices[1], 2));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[0], graph.Vertices[6], 6));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[1], graph.Vertices[2], 7));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[1], graph.Vertices[4], 2));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[2], graph.Vertices[3], 3));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[4], graph.Vertices[5], 2));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[4], graph.Vertices[6], 1));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[5], graph.Vertices[2], 1));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[5], graph.Vertices[7], 2));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[6], graph.Vertices[7], 4));
            graph.AddEdge(new DirectedWeightedEdge(graph.Vertices[7], graph.Vertices[3], 1));

            return GraphSerializer.Serialize(graph);
        }
    }
}
#endif
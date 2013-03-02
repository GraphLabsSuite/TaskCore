using System;

namespace GraphLabs.Tasks.Components.Visualization
{
    /// <summary> EventArgs для события клика по вершине </summary>
    public class VertexClickEventArgs : EventArgs
    {
        /// <summary> Вершина </summary>
        public Vertex Vertex { get; private set; }

        /// <summary> Ctor. </summary>
        public VertexClickEventArgs(Vertex vertex)
        {
            Vertex = vertex;
        }
    }
}

using System;
using System.Windows.Controls;

namespace GraphLabs.Graphs.UIComponents.Visualization
{
    public class VisualizerClickEventArgs<T> : EventArgs
        where T: Control
    {
        /// <summary> Ctor. </summary>
        public VisualizerClickEventArgs(T control)
        {
            Control = control;
        }

        /// <summary> Вершина </summary>
        public T Control { get; private set; }
    }

    /// <summary> EventArgs для события клика по вершине </summary>
    public class VertexClickEventArgs : VisualizerClickEventArgs<Vertex>
    {
        public VertexClickEventArgs(Vertex control) : base(control)
        {
        }
    }
    public class EdgeClickEventArgs : VisualizerClickEventArgs<Edge>
    {
        public EdgeClickEventArgs(Edge control) : base(control)
        {
        }
    }
}

using System;
using System.Windows.Controls;

namespace GraphLabs.Graphs.UIComponents.Visualization
{
    /// <summary> EventArgs для события клика по визуализатору </summary>
    public class VisualizerClickEventArgs<T> : EventArgs
        where T: Control
    {
        /// <summary> EventArgs для события клика по визуализатору </summary>
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
        /// <summary> EventArgs для события клика по вершине </summary>
        public VertexClickEventArgs(Vertex control) : base(control)
        {
        }
    }

    /// <summary> EventArgs для события клика по ребру </summary>
    public class EdgeClickEventArgs : VisualizerClickEventArgs<Edge>
    {
        /// <summary> EventArgs для события клика по ребру </summary>
        public EdgeClickEventArgs(Edge control) : base(control)
        {
        }
    }
}

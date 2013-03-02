using System;
using System.Collections.Generic;

namespace GraphLabs.Tasks.Core
{
    /// <summary> EventArgs для события изменения графа </summary>
    public sealed class GraphChangedEventArgs : EventArgs
    {
        /// <summary> Новые рёбра </summary>
        public IEnumerable<IEdge> NewEdges { get; private set; }

        /// <summary> Удалённые или замещённые рёбра </summary>
        public IEnumerable<IEdge> OldEdges { get; private set; }

        /// <summary> Новые вершины </summary>
        public IEnumerable<IVertex> NewVertices { get; private set; }

        /// <summary> Удалённые или замещённые вершины </summary>
        public IEnumerable<IVertex> OldVertices { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="newVertices"> Добавленные вершины </param>
        /// <param name="oldVertices"> Удалённые или замещённые вершины </param>
        /// <param name="newEdges"> Добавленные рёбра </param>
        /// <param name="oldEdges"> Удалённые или замещённые рёбра </param>
        public GraphChangedEventArgs(IEnumerable<IVertex> newVertices, IEnumerable<IVertex> oldVertices,
            IEnumerable<IEdge> newEdges, IEnumerable<IEdge> oldEdges)
        {
            NewEdges = newEdges;
            OldEdges = oldEdges;
            NewVertices = newVertices;
            OldVertices = oldVertices;
        }
    }
}

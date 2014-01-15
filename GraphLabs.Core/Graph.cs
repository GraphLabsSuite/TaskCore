using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GraphLabs.Graphs.Helpers;
using GraphLabs.Utils;

namespace GraphLabs.Graphs
{
    /// <summary> Абстрактный граф </summary>
    public abstract class Graph<TVertex, TEdge> : IGraph<TVertex, TEdge>, IObservableGraph, ICloneable
        where TVertex : IVertex
        where TEdge : IEdge<TVertex>
    {
        /// <summary> Коллекция рёбрышек </summary>
        protected readonly IList<TEdge> EdgesList;

        /// <summary> Коллекция вершинок </summary>
        protected readonly IList<TVertex> VerticesList;


        #region Implementation of IGraph

        /// <summary> Граф ориентированный? </summary>
        public abstract bool Directed { get; }

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        public abstract bool AllowMultipleEdges { get; }

        /// <summary> Числов рёбер </summary>
        public int EdgesCount
        {
            get { return EdgesList.Count; }
        }

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        ReadOnlyCollection<IEdge> IGraph.Edges
        {
            get { return _readOnlyIEdges ?? (_readOnlyIEdges = new ReadOnlyCollection<IEdge>(new ListAdapter<TEdge, IEdge>(EdgesList))); }
        }
        private ReadOnlyCollection<IEdge> _readOnlyIEdges;

        /// <summary> Добавляет ребро newEdge к графу </summary>
        void IGraph.AddEdge(IEdge edge)
        {
            AddEdge((TEdge)edge);
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        void IGraph.RemoveEdge(IEdge edge)
        {
            RemoveEdge((TEdge)edge);
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        IEdge IGraph.this[IVertex v1, IVertex v2]
        {
            get { return this[(TVertex)v1, (TVertex)v2]; }
        }

        /// <summary> Числов вершин </summary>
        public int VerticesCount
        {
            get { return VerticesList.Count; }
        }

        ReadOnlyCollection<IVertex> IGraph.Vertices
        {
            get { return _readOnlyIVertices ?? (_readOnlyIVertices = new ReadOnlyCollection<IVertex>(new ListAdapter<TVertex, IVertex>(Vertices))); }
        }
        private ReadOnlyCollection<IVertex> _readOnlyIVertices;

        /// <summary> Добавляет вершину vertex в граф </summary>
        void IGraph.AddVertex(IVertex vertex)
        {
            AddVertex((TVertex)vertex);
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        void IGraph.RemoveVertex(IVertex vertex)
        {
            RemoveVertex((TVertex)vertex);
        }

        #endregion


        #region Implementation of IGraph

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        public ReadOnlyCollection<TEdge> Edges
        {
            get { return _readOnlyTEdges ?? (_readOnlyTEdges = new ReadOnlyCollection<TEdge>(EdgesList)); } 
        }
        private ReadOnlyCollection<TEdge> _readOnlyTEdges;

        /// <summary> Добавляет ребро newEdge к графу </summary>
        public void AddEdge(TEdge edge)
        {
            EdgesList.Add(edge);
            OnGraphChanged(this,
                new GraphChangedEventArgs(
                    null,
                    null,
                    new[] { (IEdge)edge },
                    null)
                    );
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        public void RemoveEdge(TEdge edge)
        {
            EdgesList.Remove(edge);
            OnGraphChanged(this,
                new GraphChangedEventArgs(
                    null,
                    null,
                    null,
                    new[] { (IEdge)edge })
                    );
        }

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        public ReadOnlyCollection<TVertex> Vertices
        {
            get { return _readOnlyTVertices ?? (_readOnlyTVertices = new ReadOnlyCollection<TVertex>(VerticesList)); }
        }
        private ReadOnlyCollection<TVertex> _readOnlyTVertices;

        /// <summary> Добавляет вершину vertex в граф </summary>
        public void AddVertex(TVertex vertex)
        {
            VerticesList.Add(vertex);
            OnGraphChanged(this,
                new GraphChangedEventArgs(
                    new[] { (IVertex)vertex },
                    null,
                    null,
                    null)
                    );
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        public void RemoveVertex(TVertex vertex)
        {
            var edgesToRemove = EdgesList.Where(e => e.IsIncidentTo(vertex)).ToArray();
            edgesToRemove.ForEach(e => EdgesList.Remove(e));
            VerticesList.Remove(vertex);
            OnGraphChanged(this, 
                new GraphChangedEventArgs(
                    null,
                    new[] { (IVertex)vertex }, 
                    null,
                    edgesToRemove.Cast<IEdge>())
                    );
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        public abstract TEdge this[TVertex v1, TVertex v2] { get; }

        #endregion // Implementation of IGraph

        
        #region Constructors
        
        /// <summary> Создаёт новый совсем пустой граф. </summary>
        protected Graph()
        {
            VerticesList = new List<TVertex>();
            EdgesList = new List<TEdge>();
        }

        #endregion // Constructors


        #region Implementation of IObservableGraph

        /// <summary> Происходит при добавлении/удалении рёбер или вершин </summary>
        public event EventHandler<GraphChangedEventArgs> GraphChanged;

        /// <summary> Callback на изменение вершины </summary>
        public virtual void OnGraphChanged(object sender, GraphChangedEventArgs e)
        {
            if (GraphChanged != null)
            {
                GraphChanged(sender, e);
            }
        }

        #endregion


        #region Implementation of ICloneable

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        public abstract object Clone();

        #endregion
    }
}

using System;
using GraphLabs.Tasks.Core.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GraphLabs.Tasks.Core
{
    /// <summary> Абстрактный граф </summary>
    public abstract class Graph : IObservableGraph, ICloneable
    {
        /// <summary> Коллекция рёбрышек </summary>
        protected IList<IEdge> EdgesList { get; set; }

        /// <summary> Коллекция вершинок </summary>
        protected IList<IVertex> VerticesList { get; set; }


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
        public ReadOnlyCollection<IEdge> Edges
        {
            get { return new ReadOnlyCollection<IEdge>(EdgesList); } 
        }

        /// <summary> Добавляет ребро newEdge к графу </summary>
        public virtual void AddEdge(IEdge edge)
        {
            EdgesList.Add(edge);
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        public virtual void RemoveEdge(IEdge edge)
        {
            EdgesList.Remove(edge);
            OnGraphChanged(this,
                new GraphChangedEventArgs(
                    null,
                    null,
                    null,
                    new[] { edge })
                    );
        }

        /// <summary> Числов вершин </summary>
        public int VerticesCount
        {
            get { return VerticesList.Count; }
        }

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        public ReadOnlyCollection<IVertex> Vertices
        {
            get { return new ReadOnlyCollection<IVertex>(VerticesList); }
        }

        /// <summary> Добавляет вершину vertex в граф </summary>
        public virtual void AddVertex(IVertex vertex)
        {
            VerticesList.Add(vertex);
            OnGraphChanged(this,
                new GraphChangedEventArgs(
                    new[] { vertex },
                    null,
                    null,
                    null)
                    );
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        public virtual void RemoveVertex(IVertex vertex)
        {
            var edgesToRemove = EdgesList.Where(e => e.IsIncidentTo(vertex)).ToArray();
            edgesToRemove.ForEach(e => EdgesList.Remove(e));
            VerticesList.Remove(vertex);
            OnGraphChanged(this, 
                new GraphChangedEventArgs(
                    null, 
                    new [] { vertex }, 
                    null, 
                    edgesToRemove.AsEnumerable())
                    );
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        public abstract IEdge this[IVertex v1, IVertex v2] { get; }

        #endregion // Implementation of IGraph

        
        #region Constructors
        
        /// <summary> Создаёт новый совсем пустой граф. </summary>
        protected Graph()
        {
            VerticesList = new List<IVertex>();
            EdgesList = new List<IEdge>();
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

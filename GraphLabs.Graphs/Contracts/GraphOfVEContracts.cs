using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GraphLabs.Graphs.Contracts
{
    /// <summary> Класс контрактов для интерфейса IGaph </summary>
    [ContractClassFor(typeof(IGraph<,>))]
    public abstract class GraphOfVEContracts<TVertex, TEdge> : IGraph<TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex>
    {
        #region IGraph

        /// <summary> Граф ориентированный? </summary>
        public abstract bool Directed { get; }

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        public abstract bool AllowMultipleEdges { get; }

        /// <summary> Числов рёбер </summary>
        public abstract int EdgesCount { get; }

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        ReadOnlyCollection<IEdge> IGraph.Edges
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary> Добавляет ребро newEdge к графу </summary>
        public abstract void AddEdge(IEdge edge);

        /// <summary> Удаляет ребро edge из графа </summary>
        public abstract void RemoveEdge(IEdge edge);

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        IEdge IGraph.this[IVertex v1, IVertex v2]
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary> Числов вершин </summary>
        public abstract int VerticesCount { get; }

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        ReadOnlyCollection<IVertex> IGraph.Vertices
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary> Добавляет вершину vertex в граф </summary>
        public abstract void AddVertex(IVertex vertex);

        /// <summary> Удалёет вершину vertex из графа </summary>
        public abstract void RemoveVertex(IVertex vertex);
        
        #endregion


        #region Implementation of IGraph<,>

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        public ReadOnlyCollection<TEdge> Edges
        {
            get
            {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<TEdge>>() != null);
                return default(ReadOnlyCollection<TEdge>);
            }
        }

        /// <summary> Добавляет ребро newEdge к графу </summary>
        public void AddEdge(TEdge newEdge)
        {
            Contract.Requires<ArgumentNullException>(newEdge != null);
            Contract.Requires<ArgumentException>(newEdge.Directed == Directed);
            Contract.Requires<InvalidOperationException>(AllowMultipleEdges || !Edges.Contains(newEdge));
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        public void RemoveEdge(TEdge edge)
        {
            Contract.Requires<ArgumentNullException>(edge != null);
            Contract.Requires<InvalidOperationException>(Edges.Contains(edge));
        }

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        public ReadOnlyCollection<TVertex> Vertices
        {
            get
            {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<TVertex>>() != null);

                return default(ReadOnlyCollection<TVertex>);
            }
        }

        /// <summary> Добавляет вершину vertex в граф </summary>
        public void AddVertex(TVertex vertex)
        {
            Contract.Requires<ArgumentNullException>(vertex != null);
            Contract.Requires<InvalidOperationException>(!Vertices.Contains(vertex));
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        public void RemoveVertex(TVertex vertex)
        {
            Contract.Requires<ArgumentNullException>(vertex != null);
            Contract.Requires<InvalidOperationException>(Vertices.Any(v => ReferenceEquals(v, vertex)));
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        public TEdge this[TVertex v1, TVertex v2]
        {
            get
            {
                Contract.Requires(v1 != null);
                Contract.Requires(v2 != null);

                return default(TEdge);
            }
        }

        #endregion

    }
}
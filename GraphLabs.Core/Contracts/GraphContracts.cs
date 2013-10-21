using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace GraphLabs.Core.Contracts
{
    /// <summary> Класс контрактов для интерфейса IGaph </summary>
    [ContractClassFor(typeof(IGraph<,>))]
    public abstract class GraphContracts<TVertex, TEdge> : IGraph<TVertex, TEdge>
        where TVertex : IVertex
        where TEdge : IEdge<TVertex>
    {

        #region Implementation of IGraph

        /// <summary> Граф ориентированный? </summary>
        public bool Directed { get; private set; }

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        public bool AllowMultipleEdges { get; private set; }

        /// <summary> Числов рёбер </summary>
        public int EdgesCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return default(int);
            }
        }

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        ReadOnlyCollection<IEdgeBase> IGraphBase.Edges
        {
            get
            {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<IEdgeBase>>() != null);
                Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IEdgeBase>>(), e => e is TEdge));

                return default(ReadOnlyCollection<IEdgeBase>);
            }
        }

        /// <summary> Добавляет ребро newEdge к графу </summary>
        void IGraphBase.AddEdge(IEdgeBase edge)
        {
            Contract.Requires<ArgumentException>(edge is TEdge);
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        void IGraphBase.RemoveEdge(IEdgeBase edge)
        {
            Contract.Requires<ArgumentException>(edge is TEdge);
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        IEdgeBase IGraphBase.this[IVertex v1, IVertex v2]
        {
            get
            {
                Contract.Requires<ArgumentException>(v1 is TVertex);
                Contract.Requires<ArgumentException>(v2 is TVertex);

                Contract.Ensures(Contract.Result<IEdgeBase>() is TEdge);

                return default(IEdgeBase);
            }
        }

        /// <summary> Числов вершин </summary>
        public int VerticesCount
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return default(int);
            }
        }

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        ReadOnlyCollection<IVertex> IGraphBase.Vertices
        {
            get
            {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<IVertex>>() != null);
                Contract.Ensures(Contract.ForAll(Contract.Result<ReadOnlyCollection<IVertex>>(), e => e is TVertex));

                return default(ReadOnlyCollection<IVertex>);
            }
        }

        /// <summary> Добавляет вершину vertex в граф </summary>
        void IGraphBase.AddVertex(IVertex vertex)
        {
            Contract.Requires<ArgumentException>(vertex is TVertex);
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        void IGraphBase.RemoveVertex(IVertex vertex)
        {
            Contract.Requires<ArgumentException>(vertex is TVertex);
        }
        
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

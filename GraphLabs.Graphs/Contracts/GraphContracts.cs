using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GraphLabs.Graphs.Contracts
{
    /// <summary> Класс контрактов для интерфейса IGaph </summary>
    [ContractClassFor(typeof(IGraph))]
    public abstract class GraphContracts : IGraph

    {

        #region Implementation of IGraph

        /// <summary> Граф ориентированный? </summary>
        public bool Directed { get; private set; }

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        public bool AllowMultipleEdges { get; private set; }

        /// <summary> Граф взвешенный? </summary>
        public bool Weighted { get; private set; }

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
        public ReadOnlyCollection<IEdge> Edges
        {
            get
            {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<IEdge>>() != null);
                return default(ReadOnlyCollection<IEdge>);
            }
        }

        /// <summary> Добавляет ребро newEdge к графу </summary>
        public void AddEdge(IEdge edge)
        {
            Contract.Requires<ArgumentNullException>(edge != null);
            Contract.Requires<InvalidOperationException>(AllowMultipleEdges || !Edges.Contains(edge));
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        public void RemoveEdge(IEdge edge)
        {
            Contract.Requires<ArgumentNullException>(edge != null);
            Contract.Requires<InvalidOperationException>(Edges.Any(e => ReferenceEquals(e, edge)));
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        public IEdge this[IVertex v1, IVertex v2]
        {
            get
            {
                Contract.Requires<ArgumentNullException>(v1 != null);
                Contract.Requires<ArgumentNullException>(v2 != null);
                return default(IEdge);
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
        public ReadOnlyCollection<IVertex> Vertices
        {
            get
            {
                Contract.Ensures(Contract.Result<ReadOnlyCollection<IVertex>>() != null);
                return default(ReadOnlyCollection<IVertex>);
            }
        }

        /// <summary> Добавляет вершину vertex в граф </summary>
        public void AddVertex(IVertex vertex)
        {
            Contract.Requires<ArgumentNullException>(vertex != null);
            Contract.Requires<InvalidOperationException>(!Vertices.Contains(vertex));
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        public void RemoveVertex(IVertex vertex)
        {
            Contract.Requires<ArgumentNullException>(vertex != null);
            Contract.Requires<InvalidOperationException>(Vertices.Any(v => ReferenceEquals(v, vertex)));
        }

        #endregion

    }
}

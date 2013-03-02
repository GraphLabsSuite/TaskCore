using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using GraphLabs.Core.Helpers;

namespace GraphLabs.Core
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
        public ReadOnlyCollection<IEdge> Edges { get; private set; }

        /// <summary> Добавляет ребро newEdge к графу </summary>
        public void AddEdge(IEdge newEdge)
        {
            Contract.Requires<ArgumentNullException>(newEdge != null);
            Contract.Requires<ArgumentException>(newEdge.Directed == Directed);
            // При добавлении нужно убедиться, что нет _какого-либо_ ребра между вершинами - поэтому компаратор (см bug82)
            Contract.Requires<InvalidOperationException>(AllowMultipleEdges || !Edges.Contains(newEdge, EdgesComparer.Comparer));
        }

        /// <summary> Удаляет ребро edge из графа </summary>
        public void RemoveEdge(IEdge edge)
        {
            Contract.Requires<ArgumentNullException>(edge != null);
            // Удаляем конкретное ребро (см bug82)
            Contract.Requires<ArgumentException>(Edges.Contains(edge));            
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
        public ReadOnlyCollection<IVertex> Vertices { get; private set; }

        /// <summary> Добавляет вершину vertex в граф </summary>
        public void AddVertex(IVertex vertex)
        {
            Contract.Requires<ArgumentNullException>(vertex != null);
            // Используем компаратор, т.к. требуется, чтобы не было вершин с одинаковыми именами (см bug82)
            Contract.Requires<InvalidOperationException>(!Vertices.Contains(vertex, VerticesComparer.Comparer));
        }

        /// <summary> Удалёет вершину vertex из графа </summary>
        public void RemoveVertex(IVertex vertex)
        {
            Contract.Requires<ArgumentNullException>(vertex != null);
            // Удалить можно только конкретную вершину - поэтому ищем только её (см bug82)
            Contract.Requires<ArgumentException>(Vertices.Contains(vertex));
        }

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        public IEdge this[IVertex v1, IVertex v2]
        {
            get
            {
                Contract.Requires(v1 != null);
                Contract.Requires(v2 != null);

                return default(IEdge);
            }
        }

        #endregion

    }
}

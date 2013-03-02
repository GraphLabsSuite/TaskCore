using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace GraphLabs.Core
{
    /// <summary> Граф </summary>
    [ContractClass(typeof(GraphContracts))]
    public interface IGraph
    {
        /// <summary> Граф ориентированный? </summary>
        bool Directed { get; }

        /// <summary> Допускать два и более ребра между двумя вершинами? </summary>
        bool AllowMultipleEdges { get; }
        
        
        #region Edges

        /// <summary> Числов рёбер </summary>
        int EdgesCount { get; }

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        ReadOnlyCollection<IEdge> Edges { get; }

        /// <summary> Добавляет ребро newEdge к графу </summary>
        void AddEdge(IEdge edge);

        /// <summary> Удаляет ребро edge из графа </summary>
        void RemoveEdge(IEdge edge);

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        IEdge this[IVertex v1, IVertex v2] { get; }

        #endregion // Edges


        #region Vertices

        /// <summary> Числов вершин </summary>
        int VerticesCount { get; }

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        ReadOnlyCollection<IVertex> Vertices { get; }

        /// <summary> Добавляет вершину vertex в граф </summary>
        void AddVertex(IVertex vertex);

        /// <summary> Удалёет вершину vertex из графа </summary>
        void RemoveVertex(IVertex vertex);

        #endregion // Vertices
    }
}

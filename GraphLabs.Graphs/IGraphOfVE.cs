using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using GraphLabs.Graphs.Contracts;

namespace GraphLabs.Graphs
{
    /// <summary> Граф </summary>
    [ContractClass(typeof(GraphOfVEContracts<,>))]
    public interface IGraph<TVertex, TEdge> : IGraph
        where TVertex : IVertex
        where TEdge : IEdge<TVertex>
    {
        #region Edges

        /// <summary> Доступная только для чтения коллекция рёбер </summary>
        new ReadOnlyCollection<TEdge> Edges { get; }

        /// <summary> Добавляет ребро newEdge к графу </summary>
        void AddEdge(TEdge edge);

        /// <summary> Удаляет ребро edge из графа </summary>
        void RemoveEdge(TEdge edge);

        /// <summary> Возвращает ребро между вершинами v1 и v2 (если есть) или null (если ребра нет) </summary>
        TEdge this[TVertex v1, TVertex v2] { get; }

        #endregion // Edges


        #region Vertices

        /// <summary> Доступная только для чтения коллекция вершин </summary>
        new ReadOnlyCollection<TVertex> Vertices { get; }

        /// <summary> Добавляет вершину vertex в граф </summary>
        void AddVertex(TVertex vertex);

        /// <summary> Удалёет вершину vertex из графа </summary>
        void RemoveVertex(TVertex vertex);

        #endregion // Vertices
    }
}

using System;

namespace GraphLabs.Graphs
{
    /// <summary> Вершина графа </summary>
    public interface IVertex : ICloneable, IEquatable<IVertex>
    {
        /// <summary> Название вершины </summary>
        string Name { get; }
        
        /// <summary> Текст вершины </summary>
        string DispName { get; }
    }

    /// <summary> Вершина с меткой </summary>
    public interface ILabeledVertex : IVertex, IEquatable<ILabeledVertex>
    {
        /// <summary> Метка </summary>
        string Label { get; }
    }
}

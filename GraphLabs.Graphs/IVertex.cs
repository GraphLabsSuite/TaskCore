using System;

namespace GraphLabs.Graphs
{
    /// <summary> Вершина графа </summary>
    public interface IVertex : ICloneable, IEquatable<IVertex>
    {
        /// <summary> Название вершины </summary>
        string Name { get; }

        /// <summary> Подпись над вершиной </summary>
        string Text { get; }

        /// <summary> Переименовать вершину </summary>
        IVertex Rename(string newName);
    }
}

using System;

namespace GraphLabs.Core
{
    /// <summary> Вершина графа </summary>
    public interface IVertex : ICloneable, IEquatable<IVertex>
    {
        /// <summary> Название вершины </summary>
        string Name { get; }

        /// <summary> Переименовать вершину </summary>
        IVertex Rename(string newName);
    }
}

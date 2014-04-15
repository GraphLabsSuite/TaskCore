using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Graphs
{
    /// <summary> Вершина графа. </summary>
    public class Vertex : IVertex, IEquatable<Vertex>
    {
        /// <summary> Цвет </summary>
        public int Color{ get; set; }
        /// <summary> Возвращает имя вершины. </summary>
        public string Name { get; private set; }

        /// <summary> Создаёт новую вершину с именем Name. </summary>
        public Vertex(string name)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));

            Name = name;
        }

        /// <summary> Переименовать </summary>
        public Vertex Rename(string newName)
        {
            return (Vertex)((IVertex)this).Rename(newName);
        }

        /// <summary> Переименовать вершину </summary>
        IVertex IVertex.Rename(string newName)
        {
            Name = newName;
            return this;
        }

        /// <summary> Returns a string that represents the current object. </summary>
        /// <returns> A string that represents the current object. </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary> Создаёт глубокую копию данного объекта </summary>
        public object Clone()
        {
            return new Vertex(Name);
        }

        /// <summary> GetHashCode </summary>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #region Сравнение

        /// <summary> Сравнение вершин </summary>
        public virtual bool Equals(IVertex other)
        {
            return ValueEqualityComparer.VerticesEquals(this, other);
        }

        /// <summary> Сравнение вершин </summary>
        public bool Equals(Vertex other)
        {
            return Equals((IVertex)other);
        }

        /// <summary> Сравниваем </summary>
        public sealed override bool Equals(object obj)
        {
            var v = obj as IVertex;
            return Equals(v);
        }

        #endregion
    }
}

using System;
using System.Diagnostics.Contracts;

namespace GraphLabs.Core
{
    /// <summary> Вершина графа. </summary>
    public class Vertex : IVertex
    {
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

        /// <summary> Сравнение вершин </summary>
        public bool Equals(IVertex other)
        {
            return Name == other.Name;
        }

        /// <summary> Сравниваем </summary>
        public override bool Equals(object obj)
        {
            var v = obj as Vertex;
            if (v != null)
            {
                return Equals(v);
            }

            return false;
        }

        /// <summary> GetHashCode </summary>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary> Оператор сравнения </summary>
        public static bool operator ==(Vertex v1, Vertex v2)
        {
            if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null))
            {
                return ReferenceEquals(v1, null) && ReferenceEquals(v2, null);
            }

            return v1.Equals(v2);
        }

        /// <summary> Оператор сравнения </summary>
        public static bool operator !=(Vertex v1, Vertex v2)
        {
            return !(v1 == v2);
        }
    }
}

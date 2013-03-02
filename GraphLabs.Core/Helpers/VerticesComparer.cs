using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GraphLabs.Tasks.Core.Helpers
{

    /// <summary> Компаратор для вершин </summary>
    public sealed class VerticesComparer : IEqualityComparer<IVertex>
    {
        /// <summary> Ленивый экземпляр компаратора </summary>
        public static VerticesComparer Comparer
        {
            get { return _verticesComparer ?? (_verticesComparer = new VerticesComparer()); }
        }

        private static VerticesComparer _verticesComparer;

        /// <summary> Приватный конструктор </summary>
        /// <remarks> Нельзя просто так взять и создать VerticesComparer </remarks>
        private VerticesComparer() { }

        #region Implementation of IEqualityComparer<in IVertex>

        /// <summary> Determines whether the specified objects are equal. </summary>
        /// <returns> true if the specified objects are equal; otherwise, false. </returns>
        /// <param name="x">The first object of type <cref name="IEdge"/> to compare.</param>
        /// <param name="y">The second object of type <cref name="IEdge"/> to compare.</param>
        public bool Equals(IVertex x, IVertex y)
        {
            Contract.Assume(x != null && y != null);

            return x.Name == y.Name;
        }


        /// <summary> Returns a hash code for the specified object. </summary>
        /// <returns> A hash code for the specified object. </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/> 
        /// is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(IVertex obj)
        {
            return obj.Name.GetHashCode();
        }

        #endregion
    }
}


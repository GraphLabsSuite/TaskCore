using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace GraphLabs.Tasks.Core.Helpers
{
    /// <summary> Компаратор для рёбер </summary>
    public class EdgesComparer : IEqualityComparer<IEdge>
    {
        /// <summary> Ленивый экземпляр компаратора </summary>
        public static EdgesComparer Comparer
        {
             get { return _edgesComparer ?? (_edgesComparer = new EdgesComparer()); }
        }
        private static EdgesComparer _edgesComparer;

        /// <summary> Приватный конструктор </summary>
        /// <remarks> Нельзя просто так взять и создать EdgesComparer </remarks>
        private EdgesComparer() { }

        #region Implementation of IEqualityComparer<in IEdge>

        /// <summary> Determines whether the specified objects are equal. </summary>
        /// <returns> true if the specified objects are equal; otherwise, false. </returns>
        /// <param name="x">The first object of type <cref name="IEdge"/> to compare.</param>
        /// <param name="y">The second object of type <cref name="IEdge"/> to compare.</param>
        public bool Equals(IEdge x, IEdge y)
        {
            Contract.Assume(x != null && y != null);
            return x.Directed == y.Directed &&
                   (
                       VerticesComparer.Comparer.Equals(x.Vertex1, y.Vertex1) && 
                       VerticesComparer.Comparer.Equals(x.Vertex2, y.Vertex2) ||

                       !x.Directed && VerticesComparer.Comparer.Equals(x.Vertex2, y.Vertex1) &&
                       VerticesComparer.Comparer.Equals(x.Vertex1, y.Vertex2)
                   );
        }


        /// <summary> Returns a hash code for the specified object. </summary>
        /// <returns> A hash code for the specified object. </returns>
        /// <param name="obj">The <see cref="T:System.Object"/> for which a hash code is to be returned.</param>
        /// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj"/>
        /// is a reference type and <paramref name="obj"/> is null.</exception>
        public int GetHashCode(IEdge obj)
        {
            // понадобится - напишем, лениво
            throw new NotImplementedException();
        }

        #endregion
    }
}

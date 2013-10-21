using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace GraphLabs.Core.Helpers
{
    /// <summary> TTarget-обёртка коллекции элементов TSource  </summary>
    public sealed class ListAdapter<TSource, TTarget> : IList<TTarget>
        where TSource : TTarget
    {
        private readonly IList<TSource> _sourceList;

        /// <summary> Обёртка коллекции элементов TSource  </summary>
        public ListAdapter(IList<TSource> sourceList)
        {
            _sourceList = sourceList;
        }

        /// <summary> </summary>
        public IEnumerator<TTarget> GetEnumerator()
        {
            return _sourceList
                .Cast<TTarget>()
                .GetEnumerator();
        }

        /// <summary> </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary> </summary>
        public void Add(TTarget item)
        {
            Contract.Requires<ArgumentException>(item is TSource);
            _sourceList.Add((TSource)item);
        }

        /// <summary> </summary>
        public void Clear()
        {
            _sourceList.Clear();
        }

        /// <summary> </summary>
        public bool Contains(TTarget item)
        {
            Contract.Requires<ArgumentException>(item is TSource);
            return _sourceList
                .Cast<TTarget>()
                .Contains(item);
        }

        /// <summary> </summary>
        public void CopyTo(TTarget[] array, int arrayIndex)
        {
            _sourceList
                .Cast<TTarget>()
                .ToArray()
                .CopyTo(array, arrayIndex);
        }

        /// <summary> </summary>
        public bool Remove(TTarget item)
        {
            Contract.Requires<ArgumentException>(item is TSource);
            return _sourceList.Remove((TSource)item);
        }

        /// <summary> </summary>
        public int Count
        {
            get { return _sourceList.Count; }
        }

        /// <summary> </summary>
        public bool IsReadOnly
        {
            get { return _sourceList.IsReadOnly; }
        }

        /// <summary> </summary>
        public int IndexOf(TTarget item)
        {
            Contract.Requires<ArgumentException>(item is TSource);
            return _sourceList.IndexOf((TSource)item);
        }

        /// <summary> </summary>
        public void Insert(int index, TTarget item)
        {
            Contract.Requires<ArgumentException>(item is TSource);
            _sourceList.Insert(index, (TSource)item);
        }

        /// <summary> </summary>
        public void RemoveAt(int index)
        {
            _sourceList.RemoveAt(index);
        }

        /// <summary> </summary>
        public TTarget this[int index]
        {
            get { return _sourceList[index]; }
            set
            {
                Contract.Requires<ArgumentException>(value is TSource);
                _sourceList[index] = (TSource)value;
            }
        }
    }
}

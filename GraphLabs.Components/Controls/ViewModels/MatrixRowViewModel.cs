using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Media;

namespace GraphLabs.Components.Controls.ViewModels
{
    /// <summary> Вью-модель строки матрицы </summary>
    public class MatrixRowViewModel<T> : DependencyObject, IList<T>, IList, INotifyCollectionChanged
    {
        private readonly ObservableCollection<T> _collection;

        /// <summary> Ctor. </summary>
        public MatrixRowViewModel(ObservableCollection<T> collection)
        {
            Contract.Requires(collection != null);
            _collection = collection;
        }

        /// <summary> Фон </summary>
        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(
                "Background",
                typeof(Brush),
                typeof(MatrixRowViewModel<T>),
                new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));

        /// <summary> Фон </summary>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary> Строка включена? </summary>
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(MatrixRowViewModel<T>), new PropertyMetadata(true));

        /// <summary> Строка включена? </summary>
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }


        #region Implementation of IList<T>, etc

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public void Add(T item)
        {
            _collection.Add(item);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <returns>
        /// The position into which the new element was inserted.
        /// </returns>
        /// <param name="value">The <see cref="T:System.Object"/> to add to the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public int Add(object value)
        {
            return ((IList)_collection).Add(value);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Object"/> is found in the <see cref="T:System.Collections.IList"/>; otherwise, false.
        /// </returns>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>. </param>
        public bool Contains(object value)
        {
            return ((IList)_collection).Contains(value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only. </exception>
        void IList.Clear()
        {
            ((IList)_collection).Clear();
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="value"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="value">The <see cref="T:System.Object"/> to locate in the <see cref="T:System.Collections.IList"/>. </param>
        public int IndexOf(object value)
        {
            return ((IList)_collection).IndexOf(value);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value"/> should be inserted. </param><param name="value">The <see cref="T:System.Object"/> to insert into the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception><exception cref="T:System.NullReferenceException"><paramref name="value"/> is null reference in the <see cref="T:System.Collections.IList"/>.</exception>
        public void Insert(int index, object value)
        {
            ((IList)_collection).Insert(index, value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"/>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"/> to remove from the <see cref="T:System.Collections.IList"/>. </param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        public void Remove(object value)
        {
            ((IList)_collection).Remove(value);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.IList"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"/> is read-only.-or- The <see cref="T:System.Collections.IList"/> has a fixed size. </exception>
        void IList.RemoveAt(int index)
        {
            ((IList)_collection).RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.IList"/>. </exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.IList"/> is read-only. </exception>
        object IList.this[int index]
        {
            get { return ((IList)_collection)[index]; }
            set { ((IList)_collection)[index] = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.IList"/> is read-only; otherwise, false.
        /// </returns>
        bool IList.IsReadOnly
        {
            get { return ((IList)_collection).IsReadOnly; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"/> has a fixed size.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.IList"/> has a fixed size; otherwise, false.
        /// </returns>
        public bool IsFixedSize
        {
            get { return ((IList)_collection).IsFixedSize; }
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        void ICollection<T>.Clear()
        {
            _collection.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type T cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(T item)
        {
            return _collection.Remove(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing. </param><param name="index">The zero-based index in <paramref name="array"/> at which copying begins. </param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null. </exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero. </exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>.-or- The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(Array array, int index)
        {
            ((ICollection)_collection).CopyTo(array, index);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        int ICollection.Count
        {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        public object SyncRoot
        {
            get { return ((ICollection)_collection).SyncRoot; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <returns>
        /// true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.
        /// </returns>
        public bool IsSynchronized
        {
            get { return ((ICollection)_collection).IsSynchronized; }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        int ICollection<T>.Count
        {
            get { return _collection.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>)_collection).IsReadOnly; }
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
        /// </summary>
        /// <returns>
        /// The index of <paramref name="item"/> if found in the list; otherwise, -1.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
        public int IndexOf(T item)
        {
            return _collection.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param><param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public void Insert(int index, T item)
        {
            _collection.Insert(index, item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        void IList<T>.RemoveAt(int index)
        {
            _collection.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <returns>
        /// The element at the specified index.
        /// </returns>
        /// <param name="index">The zero-based index of the element to get or set.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception><exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
        public T this[int index]
        {
            get { return _collection[index]; }
            set { _collection[index] = value; }
        }

        /// <summary>
        /// Occurs when the items list of the collection has changed, or the collection is reset.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _collection.CollectionChanged += value; }
            remove { _collection.CollectionChanged -= value; }
        }
        
        #endregion
    }
}

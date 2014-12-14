using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace GraphLabs.Graphs
{
    /// <summary> Вершина графа. </summary>
    public class Vertex : IVertex, ILabeledVertex, IEquatable<Vertex>, INotifyPropertyChanged
    {
        private int _color;

        /// <summary> Цвет </summary>
        public int Color 
        {
            get { return _color; } 
            set 
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        private string _label;

        /// <summary> Метка </summary>
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged("Label");
            }
        }
        
        /// <summary> Возвращает имя вершины. </summary>
        public string Name { get; private set; }

        /// <summary> Создаёт новую вершину с именем Name. </summary>
        public Vertex(string name)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(name));
            Name = name;
            Label = "";
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
            OnPropertyChanged("Name");
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
        public virtual bool Equals(ILabeledVertex other)
        {
            return Equals((IVertex)other);
        }

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


        #region

        /// <summary> Уведомляет об изменении свойства </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary> Уведомляет об изменении свойства </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var h = PropertyChanged;
            if (h != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}

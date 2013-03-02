using System.Runtime.Serialization;

namespace GraphLabs.Core.DataTransferObjects
{
    /// <summary> Класс для пересылки вершин </summary>
    [DataContract]
    public class VertexDto
    {
        /// <summary> Имя вершины </summary>
        [DataMember]
        public string Name { get; set; }
    }
}

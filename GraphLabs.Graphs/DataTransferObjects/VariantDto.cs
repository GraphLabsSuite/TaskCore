using System.Runtime.Serialization;

namespace GraphLabs.Graphs.DataTransferObjects
{
    /// <summary> Класс для пересылки варианта </summary>
    [DataContract]
    public class VariantDto
    {
        /// <summary> Набор графов </summary>
        [DataMember]
        public GraphDto[] Graphs { get; set; }
    }
}

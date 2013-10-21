using System.IO;
using System.Runtime.Serialization;

namespace GraphLabs.Core.DataTransferObjects.Converters
{
    /// <summary> Вспомагательный класс для сериализации/десериализации графов </summary>
    public static class GraphSerializer
    {
        /// <summary> Сериализует граф </summary>
        /// <param name="graph"> Граф </param>
        /// <returns>Массив байтов</returns>
        public static byte[] Serialize(IGraph graph)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(GraphDto));
                serializer.WriteObject(stream, GraphToDtoConverter.Convert(graph));

                return stream.ToArray();
            }
        }

        /// <summary> Десериализует граф </summary>
        /// <param name="graph">Массив байтов</param>
        /// <returns>
        /// <see cref="DirectedGraph"/> или <see cref="UndirectedGraph"/>
        /// </returns>
        public static IGraph Deserialize(byte[] graph)
        {
            using (var stream = new MemoryStream(graph))
            {
                var deSerializer = new DataContractSerializer(typeof(GraphDto));
                return GraphToDtoConverter.ConvertBack((GraphDto)deSerializer.ReadObject(stream));
            }
        }
    }
}

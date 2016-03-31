using System.IO;
using System.Runtime.Serialization;

namespace GraphLabs.Graphs.DataTransferObjects.Converters
{
    /// <summary> Вспомагательный класс для сериализации/десериализации варианта </summary>
    public static class VariantSerializer
    {
        /// <summary> Сериализует вариант </summary>
        public static byte[] Serialize(IGraph[] graphs)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(VariantDto));
                serializer.WriteObject(stream, VariantToDtoConverter.Convert(graphs));

                return stream.ToArray();
            }
        }

        /// <summary> Десериализует вариант </summary>
        public static IGraph[] Deserialize(byte[] serializedVariant)
        {
            using (var stream = new MemoryStream(serializedVariant))
            {
                var deSerializer = new DataContractSerializer(typeof(VariantDto));
                return VariantToDtoConverter.ConvertBack((VariantDto)deSerializer.ReadObject(stream));
            }
        }
    }
}

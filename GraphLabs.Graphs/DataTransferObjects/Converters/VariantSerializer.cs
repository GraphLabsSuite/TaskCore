using System.IO;
using System.Runtime.Serialization;

namespace GraphLabs.Graphs.DataTransferObjects.Converters
{
    /// <summary> Вспомагательный класс для сериализации/десериализации варианта </summary>
    public static class VariantSerializer
    {
        /// <summary> Сериализует вариант </summary>
        public static byte[] Serialize(object[] data)
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(VariantDto));
                serializer.WriteObject(stream, VariantToDtoConverter.Convert(data));

                return stream.ToArray();
            }
        }

        /// <summary> Десериализует вариант </summary>
        public static object[] Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var deSerializer = new DataContractSerializer(typeof(VariantDto));
                return VariantToDtoConverter.ConvertBack((VariantDto) deSerializer.ReadObject(stream));
            }
        }
    }
}

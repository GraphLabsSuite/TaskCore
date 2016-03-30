using System.Collections.Generic;
using System.Linq;

namespace GraphLabs.Graphs.DataTransferObjects.Converters
{
    internal static class VariantToDtoConverter
    {
        public static VariantDto Convert(object[] objects)
        {
            var graphDtos = objects
                .OfType<IGraph>()
                .Select(GraphToDtoConverter.Convert)
                .ToList();
            return new VariantDto
            {
                Graphs = graphDtos.ToArray()
            };
        }

        public static object[] ConvertBack(VariantDto data)
        {
            var objects = new List<object>();
            objects.AddRange(data.Graphs.Select(GraphToDtoConverter.ConvertBack));
            return objects.ToArray();
        }
    }
}

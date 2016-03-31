using System.Collections.Generic;
using System.Linq;

namespace GraphLabs.Graphs.DataTransferObjects.Converters
{
    internal static class VariantToDtoConverter
    {
        public static VariantDto Convert(IGraph[] graphs)
        {
            var graphDtos = graphs
                .Select(GraphToDtoConverter.Convert)
                .ToList();
            return new VariantDto
            {
                Graphs = graphDtos.ToArray()
            };
        }

        public static IGraph[] ConvertBack(VariantDto data)
        {
            return data.Graphs
                .Select(GraphToDtoConverter.ConvertBack)
                .ToArray();
        }
    }
}

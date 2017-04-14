using GraphLabs.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphLabs.Graphs
{
    /// <summary> Строка, представляющая граф </summary>
    public class PrintGraph
    {
        /// <summary> Представляет граф в виде строки </summary>
        public string GraphToString (IGraph graph)
        {
            var stringToReturn = "({";
            graph.Vertices.ForEach(v =>
            {
                stringToReturn += v.ToString() + "; ";
            }
            );
            stringToReturn = stringToReturn.Remove(stringToReturn.Length - 2) + "}, {";
            graph.Edges.ForEach(e =>
            {
                stringToReturn += "(" + e.Vertex1.ToString() + ", " + e.Vertex2.ToString() + "), ";
            }
            );
            stringToReturn = stringToReturn.Remove(stringToReturn.Length - 2) + "})";
            return stringToReturn;
        }
    }
}

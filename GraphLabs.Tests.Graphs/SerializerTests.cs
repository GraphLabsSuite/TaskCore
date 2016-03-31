using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphLabs.Graphs;
using GraphLabs.Graphs.DataTransferObjects.Converters;
using NUnit.Framework;

namespace GraphLabs.Tests.Graphs
{

    /// <summary> Тесты сериализации </summary>
    [TestFixture]
    public class SerializerTests
    {
        private IEnumerable<IGraph> CreateGraphs()
        {
            var graph1 = new UndirectedGraph();
            var vertexA = new Vertex("A");
            var vertexB = new Vertex("B");
            var vertexC = new Vertex("C");
            var vertexD = new Vertex("D");
            var vertexG = new Vertex("G");
            var vertexH = new Vertex("H");
            var vertexI = new Vertex("I");
            var vertexJ = new Vertex("J");

            graph1.AddEdge(new UndirectedEdge(vertexA, vertexG));
            graph1.AddEdge(new UndirectedEdge(vertexA, vertexH));
            graph1.AddEdge(new UndirectedEdge(vertexA, vertexI));
            graph1.AddEdge(new UndirectedEdge(vertexG, vertexB));
            graph1.AddEdge(new UndirectedEdge(vertexG, vertexC));
            graph1.AddEdge(new UndirectedEdge(vertexB, vertexH));
            graph1.AddEdge(new UndirectedEdge(vertexB, vertexJ));
            graph1.AddEdge(new UndirectedEdge(vertexH, vertexD));
            graph1.AddEdge(new UndirectedEdge(vertexC, vertexI));
            graph1.AddEdge(new UndirectedEdge(vertexC, vertexJ));
            graph1.AddEdge(new UndirectedEdge(vertexD, vertexI));
            graph1.AddEdge(new UndirectedEdge(vertexD, vertexJ));

            graph1.AddVertex(vertexA);
            graph1.AddVertex(vertexB);
            graph1.AddVertex(vertexC);
            graph1.AddVertex(vertexD);
            graph1.AddVertex(vertexG);
            graph1.AddVertex(vertexH);
            graph1.AddVertex(vertexI);
            graph1.AddVertex(vertexJ);

            yield return graph1;

            // Graph2
            var graph2 = new UndirectedGraph();

            var vertex1 = new Vertex("1");
            var vertex2 = new Vertex("2");
            var vertex3 = new Vertex("3");
            var vertex4 = new Vertex("4");
            var vertex5 = new Vertex("5");
            var vertex6 = new Vertex("6");
            var vertex7 = new Vertex("7");
            var vertex8 = new Vertex("8");

            graph2.AddEdge(new UndirectedEdge(vertex1, vertex2));
            graph2.AddEdge(new UndirectedEdge(vertex1, vertex5));
            graph2.AddEdge(new UndirectedEdge(vertex1, vertex4));
            graph2.AddEdge(new UndirectedEdge(vertex4, vertex3));
            graph2.AddEdge(new UndirectedEdge(vertex4, vertex8));
            graph2.AddEdge(new UndirectedEdge(vertex3, vertex7));
            graph2.AddEdge(new UndirectedEdge(vertex3, vertex2));
            graph2.AddEdge(new UndirectedEdge(vertex2, vertex6));
            graph2.AddEdge(new UndirectedEdge(vertex5, vertex6));
            graph2.AddEdge(new UndirectedEdge(vertex5, vertex8));
            graph2.AddEdge(new UndirectedEdge(vertex7, vertex6));
            graph2.AddEdge(new UndirectedEdge(vertex7, vertex8));

            graph2.AddVertex(vertex1);
            graph2.AddVertex(vertex2);
            graph2.AddVertex(vertex3);
            graph2.AddVertex(vertex4);
            graph2.AddVertex(vertex5);
            graph2.AddVertex(vertex6);
            graph2.AddVertex(vertex7);
            graph2.AddVertex(vertex8);

            yield return graph2;
        }

        [Test]
        public void TestSerializeVariantDto()
        {
            var graphs = CreateGraphs().ToArray();
            var dto = VariantSerializer.Serialize(graphs);

            CollectionAssert.IsNotEmpty(dto);

            var deserializedGraphs = VariantSerializer.Deserialize(dto);

            Assert.That(deserializedGraphs.Count() == 2);
        }
    }
}

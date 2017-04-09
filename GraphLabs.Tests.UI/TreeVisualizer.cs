using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using GraphLabs.Graphs.UIComponents.Visualization;

namespace GraphLabs.Tests.UI
{
    public class TreeVisualizer : IVisualizationAlgorithm
    {
        private Vertex root;
        public TreeVisualizer(Vertex root)
        {
            this.root = root;
        }
        public string Name()
        {
            throw new System.NotImplementedException();
        }

        public void Visualize()
        {
            var vertices = Visualizer.Vertices;
            if (!vertices.Any())
                return;

            var curH = 100;
            var STEP = 100;
            
            var que = new Queue<Vertex>();
            que.Enqueue(root);

            while (!que.IsNullOrEmpty())
            {
                var StepX = Visualizer.ActualWidth / (que.Count + 1);
                var i = 1;
                foreach (var vert in que)
                {
                    vert.ModelX = StepX * i++;
                    vert.ModelY = curH;
                    vert.ScaleFactor = 1;
                }
                curH += STEP;

                var nextQue = new Queue<Vertex>();
                while (!que.IsNullOrEmpty())
                {
                    var ver = que.Dequeue();
                    var quenextPart = getAdjcentVertexes(ver);
                    while (!quenextPart.IsNullOrEmpty())
                        nextQue.Enqueue(quenextPart.Dequeue());     
                }
                que = nextQue;
            }

           
        }

        private Queue<Vertex> getAdjcentVertexes(Vertex v)
        {
            Queue<Vertex> q = new Queue<Vertex>();
            var edges = Visualizer.Edges;
            foreach (var edge in edges)
            {
                if (edge.Vertex1.Equals(v))
                    q.Enqueue(edge.Vertex2);
            }
            return q;
        }

        public GraphVisualizer Visualizer { get; set; }
    }
}
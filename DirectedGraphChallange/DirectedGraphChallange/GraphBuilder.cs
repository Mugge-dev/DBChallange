using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectedGraphChallange
{
    class GraphBuilder
    {

        /// <summary>
        /// Builds a directed graph from the rules of the assignment and the given input in form of a file.
        /// </summary>
        public static List<List<Node>> BuildGraph(string inputFile)
        {
            var graph = new List<List<Node>>();

            using (var reader = new StreamReader(@"../../Resources/" + inputFile))
            {
                // Read in lines from file and add layers of nodes to our graph.
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var layer = new List<Node>();
                    foreach (var item in values)
                    {
                        layer.Add(new Node(int.Parse(item)));
                    }
                    graph.Add(layer);
                }
            }

            // Appying the links between nodes in our graph according to the rules.
            for (int level = 0; level < graph.Count - 1; level++)
            {
                var layer = graph[level];

                for (int node = 0; node < layer.Count; node++)
                {
                    layer[node].Left = graph[level + 1][node];
                    layer[node].Right = graph[level + 1][node + 1];
                }
            }

            return graph;
        }
    }
}

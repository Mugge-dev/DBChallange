using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectedGraphChallange
{
    class Program
    {
        public static List<List<Node>> Graph = new List<List<Node>>();
        public static List<int[]> ValidPaths = new List<int[]>();
        public static Stack<int> TempPath = new Stack<int>();
        public static int CurrentMax;
        public static Stack<int[]> CurrentMaxPath = new Stack<int[]>();


        static void Main(string[] args)
        {
            try
            {
                BuildGraph();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Could not find the directory for the file.{Environment.NewLine}Press enter to continue...");
                Console.ReadLine();
                return;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Could not find file in specified location.{Environment.NewLine}Press enter to continue...");
                Console.ReadLine();
                return;
            }
            CalcPath(Graph[0][0]);
            PrintResult();
        }

        static void BuildGraph()
        {
            using (var reader = new StreamReader(@"../../Resources/DirectedGraphInput.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var layer = new List<Node>();
                    foreach (var item in values)
                    {
                        layer.Add(new Node(int.Parse(item)));
                    }
                    Graph.Add(layer);
                }
            }

            for (int i = 0; i < Graph.Count - 1; i++)
            {
                var layer = Graph[i];

                for (int j = 0; j < layer.Count; j++)
                {
                    layer[j].Left = Graph[i + 1][j];
                    layer[j].Right = Graph[i + 1][j + 1];
                }
            }
        }

        static void CalcPath(Node nextNode)
        {
            TempPath.Push(nextNode.Value);

            if (nextNode.Left != null && !(nextNode.Value % 2 == nextNode.Left.Value % 2))
            {
                CalcPath(nextNode.Left);
            }
            if (nextNode.Right != null && !(nextNode.Value % 2 == nextNode.Right.Value % 2))
            {
                CalcPath(nextNode.Right);
            }
            if (nextNode.Right == null && nextNode.Left == null)
            {
                var TempPathSum = TempPath.Sum();
                if (TempPathSum > CurrentMax)
                {
                    CurrentMax = TempPathSum;
                    if (CurrentMaxPath.Count != 0)
                    {
                        CurrentMaxPath.Pop();
                    }
                    CurrentMaxPath.Push(TempPath.ToArray());
                }

                ValidPaths.Add(TempPath.ToArray());
            }

            TempPath.Pop();
        }

        static void PrintResult()
        {
            Console.WriteLine($"Max sum: {CurrentMax}");
            var path = CurrentMaxPath.Peek().Reverse();
            Console.WriteLine($"Path: { String.Join(", ", path) }");
            Console.ReadLine();
        }
    }
}

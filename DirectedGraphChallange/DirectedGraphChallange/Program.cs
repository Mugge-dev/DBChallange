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
        private static string SampleInput { get; } = "SampleInput.csv";
        private static string DirectedGraphInput { get; } = "DirectedGraphInput.csv";

        private static List<List<Node>> Graph = new List<List<Node>>();
        private static readonly Stack<int> TempPath = new Stack<int>();
        private static int CurrentMax;
        private static readonly Stack<int[]> CurrentMaxPath = new Stack<int[]>();


        static void Main(string[] args)
        {
            try
            {
                Graph = GraphBuilder.BuildGraph(DirectedGraphInput);
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

            CalcPath(Graph[0][0]); // [0][0] is the first node of the graph
            PrintResult();
        }

        /// <summary>
        /// Goes through the graph according to the rules. Saves the path which gives the highest total and the total value, for printing.
        /// </summary>
        /// <param name="nextNode">The node to operate from</param>
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
            }
            TempPath.Pop();
        }


        /// <summary>
        /// Prints maximum total value found along with the path where this result was found.
        /// </summary>
        static void PrintResult()
        {
            Console.WriteLine($"Max sum: {CurrentMax}");
            var path = CurrentMaxPath.Peek().Reverse();
            Console.WriteLine($"Path: { String.Join(", ", path) }");
            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }
}

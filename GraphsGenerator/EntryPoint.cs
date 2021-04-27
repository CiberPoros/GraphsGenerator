using System.IO;
using System.Linq;
using CommandLine;

namespace GraphsGenerator
{
    class EntryPoint
    {
        static void Main(string[] args)
        {
            var consoleArguments = new Options();
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(o => 
                {
                    var generator = new BruteForceGenerator();
                    var result = generator.Generate(o.VertexCount).ToList();
                    System.Console.WriteLine($"Total graphs count: {result.Count}.");

                    if (o.WriteGraphsToFile)
                    {
                        File.WriteAllLines(o.FileName, result);
                        System.Console.WriteLine($"Graphs was written to \"{o.FileName}\" file.");
                        return;
                    }

                    System.Console.WriteLine("Graphs in g6 format: ");
                    foreach (var g6 in result)
                    {
                        System.Console.WriteLine(g6);
                    };
                });
        }
    }
}

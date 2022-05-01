using System;
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
                    IGenerator generator = IGenerator.CreateNew(o.GeneratorType);
                    var startTime = DateTime.Now;

                    var result = generator.Generate(o.VertexCount, o.EdgesCount, o.ConnectedOnly);
                    
                    if (o.WriteGraphsToFile)
                    {
                        File.WriteAllLines(o.FileName, result);
                        Console.WriteLine($"Graphs was written to \"{o.FileName}\" file.");
                        return;
                    }

                    Console.WriteLine("Graphs in g6 format: ");
                    foreach (var g6 in result)
                    {
                        Console.WriteLine(g6);
                    };

                    Console.WriteLine($"Total graphs count: {result.Count()}.");
                    Console.WriteLine($"Total calculating time in seconds: {(DateTime.Now - startTime).TotalSeconds}.");
                });
        }
    }
}

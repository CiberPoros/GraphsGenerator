using CommandLine;

namespace GraphsGenerator
{
    public class Options
    {
        [Option('v', "VertexCount", Required = true, HelpText = "Vertex count.")]
        public int VertexCount { get; set; }

        [Option('w', "WriteToFile", Default = false, HelpText = "Print all graphs to file.")]
        public bool WriteGraphsToFile { get; set; }

        [Option('f', "FileName", Default = "Graphs.txt", HelpText = "File name for writing graphs. Will be user if parameter -w is true.")]
        public string FileName { get; set; }
    }
}

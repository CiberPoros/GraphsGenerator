using CommandLine;

namespace GraphsGenerator
{
    internal class Options
    {
        [Option('v', "VertexCount", Required = true, HelpText = "Vertex count.")]
        public int VertexCount { get; set; }

        [Option('g', "GeneratorType", Default = GeneratorType.GENERATOR_BY_CANONICAL_CODE, HelpText = "Generator type. 1 - use caconical code (default), 2 - brute force all graphs")]
        public GeneratorType GeneratorType { get; set; }

        [Option('w', "WriteToFile", Default = false, HelpText = "Print all graphs to file.")]
        public bool WriteGraphsToFile { get; set; }

        [Option('f', "FileName", Default = "Graphs.txt", HelpText = "File name for writing graphs. Will be user if parameter -w is true.")]
        public string FileName { get; set; }
    }
}

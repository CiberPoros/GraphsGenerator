using System.Collections.Generic;

namespace GraphsGenerator
{
    internal interface IGenerator
    {
        IEnumerable<string> Generate(int vertexCount);

        public static IGenerator CreateNew(GeneratorType generatorType) => generatorType switch
        {
            GeneratorType.GENERATOR_BY_CANONICAL_CODE => new GeneratorByCanonicalCode(),
            GeneratorType.BRUTE_FORCE_ALL_GRAPHS => new BruteForceAllGraphsGenerator(),
            _ => throw new System.NotImplementedException(),
        };
    }
}
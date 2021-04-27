using System.Collections.Generic;
using System.Numerics;

namespace GraphsGenerator
{
    public class GeneratorByCanonicalCode : IGenerator
    {
        public IEnumerable<string> Generate(int vertexCount)
        {
            Dictionary<BigInteger, string> graphs = new Dictionary<BigInteger, string>();
            var bitsCount = (int)(((vertexCount + .0) / 2) * (vertexCount - 1));

            long limit = 1L << (bitsCount + 1);
            for (var i = 0L; i < limit; i++)
            {
                var vector = GenerationUtils.ToAjentityVector(i, vertexCount);
                var graph = new Graph(vector);

                if (!ConnectivityChecker.IsConnected(graph))
                {
                    continue;
                }

                var code = IsomorphismChecker.GetCon(graph);

                if (!graphs.ContainsKey(code))
                {
                    graphs.Add(code, graph.ToG6());
                    yield return graph.ToG6();
                }
            }
        }
    }
}

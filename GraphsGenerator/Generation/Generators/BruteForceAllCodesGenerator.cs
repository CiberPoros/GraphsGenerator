using System.Collections.Generic;

namespace GraphsGenerator
{
    public class BruteForceAllCodesGenerator : IGenerator
    {
        public IEnumerable<string> Generate(int vertexCount, int? edgesCount = null, bool connectedOnly = true)
        {
            var bitsCount = (int)(((vertexCount + .0) / 2) * (vertexCount - 1));

            HashSet<long> codes = new HashSet<long>();

            long limit = 1L << (bitsCount);
            for (var i = 0L; i < limit; i++)
            {
                if (edgesCount is not null && BitsMagicUtils.GetCountOfBits(i) != edgesCount)
                {
                    continue;
                }

                var vector = GenerationUtils.ToAjentityVector(i, vertexCount);
                var graph = new Graph(vector);

                if (connectedOnly && !ConnectivityChecker.IsConnected(graph))
                {
                    continue;
                }

                var code = GenerationUtils.GetSimpleCode(vector);

                if (codes.Contains(code))
                {
                    continue;
                }

                foreach (var substitution in GenerationUtils.EnumerateAllSubstitutions(graph))
                {
                    var currentVector = GenerationUtils.UseSubstitution(vector, substitution);
                    var currentCode = GenerationUtils.GetSimpleCode(currentVector);
                    codes.Add(currentCode);
                }

                yield return graph.ToG6();
            }
        }
    }
}

using System.Collections.Generic;

namespace GraphsGenerator
{
    public class BruteForceAllCodesGenerator : IGenerator
    {
        public IEnumerable<string> Generate(int vertexCount)
        {
            var bitsCount = (int)(((vertexCount + .0) / 2) * (vertexCount - 1));

            HashSet<long> codes = new HashSet<long>();

            long limit = 1L << (bitsCount + 1);
            for (var i = 0L; i < limit; i++)
            {
                var vector = GenerationUtils.ToAjentityVector(i, vertexCount);
                var graph = new Graph(vector);

                if (!ConnectivityChecker.IsConnected(graph))
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

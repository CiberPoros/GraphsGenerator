using System.Collections.Generic;

namespace GraphsGenerator
{
    internal class BruteForceAllCodesWithFilterGenerator : IGenerator
    {
        readonly HashSet<long> _canonCodes = new();
        readonly HashSet<long> _notCanonCodes = new();

        public IEnumerable<string> Generate(int vertexCount)
        {
            var bitsCount = (int)(((vertexCount + .0) / 2) * (vertexCount - 1));
            return EnumerateCodes(1L << (bitsCount - 1), bitsCount - 2, vertexCount, bitsCount);
        }

        private IEnumerable<string> EnumerateCodes(long code, int position, int vertexCount, int bitsCount)
        {
            var vector = GenerationUtils.ToAjentityVector(code, vertexCount);
            var graph = new Graph(vector);

            if (!CheckIsCanonByBruteForce(graph, vector, bitsCount))
            {
                yield break;
            }

            if (ConnectivityChecker.IsConnected(graph))
            {
                yield return graph.ToG6();
            }

            var mask = 1L << position;
            for (int i = position; i >= 0; i--, mask >>= 1)
            {
                foreach (var item in EnumerateCodes(code | mask, i - 1, vertexCount, bitsCount))
                {
                    yield return item;
                }
            }
        }

        private bool CheckIsCanonByCanonCode(Graph graph, List<int> vector, int bitsCount)
        {
            var canonicalCode = IsomorphismChecker.GetCon(graph);
            var simpleCode = GenerationUtils.GetBigIntegerMaxCode(graph, bitsCount);

            return canonicalCode == simpleCode;
        }

        private bool CheckIsCanonByBruteForce(Graph graph, List<int> vector, int bitsCount)
        {
            var startCode = GenerationUtils.GetMaxCode(vector, bitsCount);

            if (_canonCodes.Contains(startCode))
            {
                return true;
            }

            if (_notCanonCodes.Contains(startCode))
            {
                return false;
            }

            var maxCode = startCode;

            var codes = new HashSet<long>();

            foreach (var substitution in GenerationUtils.EnumerateAllSubstitutions(graph))
            {
                var currentVector = GenerationUtils.UseSubstitution(vector, substitution);
                var currentCode = GenerationUtils.GetMaxCode(currentVector, bitsCount);
                
                if (_canonCodes.Contains(currentCode))
                {
                    return false;
                }

                codes.Add(currentCode);

                if (currentCode > maxCode)
                {
                    maxCode = currentCode;
                }
            }

            codes.Remove(maxCode);
            foreach (var code in codes)
            {
                _notCanonCodes.Add(code);
            }

            _canonCodes.Add(maxCode);

            return startCode == maxCode;
        }
    }
}

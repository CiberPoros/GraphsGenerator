using System.Collections.Generic;
using System.Linq;

namespace GraphsGenerator
{
    internal class BruteForceAllCodesWithFilterGenerator : IGenerator
    {
        readonly HashSet<long> _canonCodes = new();
        readonly HashSet<long> _notCanonCodes = new();

        public IEnumerable<string> Generate(int vertexCount, int? edgesCount = null, bool connectedOnly = true)
        {
            var bitsCount = (int)(((vertexCount + .0) / 2) * (vertexCount - 1));
            return EnumerateCodes(0L, bitsCount - 1, vertexCount, bitsCount, connectedOnly, edgesCount, 1);
        }

        private IEnumerable<string> EnumerateCodes(long code, int position, int vertexCount, int bitsCount, bool connectedOnly, int? edgesCount, int deep = 1)
        {
            var vector = GenerationUtils.ToAjentityVector(code, vertexCount);
            var graph = new Graph(vector);

            if (!CheckIsCanonByBruteForce(graph, vector, bitsCount))
            {
                yield break;
            }

            if ((edgesCount is null || edgesCount == deep) && (!connectedOnly || ConnectivityChecker.IsConnected(graph)))
            {
                yield return graph.ToG6();
            }

            if (edgesCount is not null && deep >= edgesCount)
            {
                yield break;
            }

            var mask = 1L << position;
            for (int i = position; i >= 0; i--, mask >>= 1)
            {
                foreach (var item in EnumerateCodes(code | mask, i - 1, vertexCount, bitsCount, connectedOnly, edgesCount, deep + 1))
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

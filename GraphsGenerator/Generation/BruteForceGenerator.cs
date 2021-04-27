using System.Collections.Generic;
using System.Numerics;

namespace GraphsGenerator
{
    public class BruteForceGenerator
    {
        public IEnumerable<string> Generate(int vertexCount)
        {
            Dictionary<BigInteger, string> graphs = new Dictionary<BigInteger, string>();
            var bitsCount = (int)(((vertexCount + .0) / 2) * (vertexCount - 1));

            long limit = 1L << (bitsCount + 1);
            for (var i = 0L; i < limit; i++)
            {
                var vector = ToAjentityVector(i, vertexCount);
                var graph = new Graph(vector);

                if (!ConnectivityChecker.IsConnected(graph))
                {
                    continue;
                }
                
                var code = IsomorphismChecker.GetCon(graph);

                if (!graphs.ContainsKey(code))
                {
                    graphs.Add(code, graph.ToG6());
                }
            }
          
            return graphs.Values;
        }

        private static List<int> ToAjentityVector(long longView, int vertexCount)
        {
            var result = new List<int>();

            for (int i = 0; i < vertexCount; i++)
            {
                result.Add(0);
            }

            for (int i = vertexCount - 1, iMask = 1 << (vertexCount - 1); i >= 0; i--, iMask >>= 1)
            {
                for (int j = vertexCount - 1, jMask = 1 << (vertexCount - 1); j > i; j--, jMask >>= 1)
                {
                    if (longView % 2 == 1)
                    {
                        result[i] |= jMask;
                        result[j] |= iMask;
                    }

                    longView >>= 1;
                }
            }

            return result;
        }
    }
}

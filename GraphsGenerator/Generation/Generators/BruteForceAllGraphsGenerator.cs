using System.Collections.Generic;
using System.Linq;

namespace GraphsGenerator
{
    class BruteForceAllGraphsGenerator : IGenerator
    {
        public IEnumerable<string> Generate(int vertexCount)
        {
            var bitsCount = (int)(((vertexCount + .0) / 2) * (vertexCount - 1));

            List<Graph> graphs = new List<Graph>();

            long limit = 1L << (bitsCount + 1);
            for (var i = 0L; i < limit; i++)
            {
                var vector = GenerationUtils.ToAjentityVector(i, vertexCount);
                var graph = new Graph(vector);

                if (!ConnectivityChecker.IsConnected(graph))
                {
                    continue;
                }

                bool isUnique = true;
                foreach (var substitution in EnumerateAllSubstitutions(graph))
                {
                    var currentVector = UseSubstitution(vector, substitution);
                    var currentGraph = new Graph(currentVector);

                    if (graphs.Contains(currentGraph))
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique)
                {
                    graphs.Add(graph);
                    yield return graph.ToG6();
                }
            }
        }

        public static List<int> UseSubstitution(List<int> vector, List<int> substitution)
        {
            var n = vector.Count;
            var result = Enumerable.Repeat(0, n).ToList();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0, jMask = 1; j < n; j++, jMask <<= 1)
                {
                    if ((vector[i] & jMask) == 0)
                    {
                        continue;
                    }

                    result[substitution[i]] |= 1 << substitution[j];
                    result[substitution[j]] |= 1 << substitution[i];
                }
            }

            return result;
        }

        public static IEnumerable<List<int>> EnumerateAllSubstitutions(Graph graph)
        {
            var n = graph.VertexCount;

            var vertexNumbers = new int[n];
            bool[] used = new bool[n];

            return Enumerate(used, 0);

            IEnumerable<List<int>> Enumerate(bool[] used, int deep)
            {
                if (deep == n)
                { 
                    yield return vertexNumbers.ToList();
                }

                for (int i = 0; i < n; i++)
                {
                    if (used[i])
                    {
                        continue;
                    }

                    used[i] = true;
                    vertexNumbers[deep] = i;

                    foreach (var val in Enumerate(used, deep + 1))
                        yield return val;

                    used[i] = false;
                }
            }
        }
    }
}

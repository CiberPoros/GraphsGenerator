using System.Collections.Generic;

namespace GraphsGenerator
{
    class BruteForceAllGraphsGenerator : IGenerator
    {
        public IEnumerable<string> Generate(int vertexCount, int? edgesCount = null, bool connectedOnly = true)
        {
            var bitsCount = (int)(((vertexCount + .0) / 2) * (vertexCount - 1));
            List<Graph> graphs = new List<Graph>();

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

                bool isUnique = true;
                foreach (var substitution in GenerationUtils.EnumerateAllSubstitutions(graph))
                {
                    var currentVector = GenerationUtils.UseSubstitution(vector, substitution);
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
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GraphsGenerator
{
    internal static class GenerationUtils
    {
        public static List<int> ToAjentityVector(long longView, int vertexCount)
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

        public static List<int> ToAjentityVectorByMaxCode(long longView, int vertexCount)
        {
            var result = new List<int>();

            for (int i = 0; i < vertexCount; i++)
            {
                result.Add(0);
            }

            for (int i = 0, iMask = 1; i < vertexCount; i++, iMask <<= 1)
            {
                for (int j = i + 1, jMask = (1 << i + 1); j < vertexCount; j++, jMask <<= 1)
                {
                    if ((longView & 1) == 1)
                    {
                        result[i] |= jMask;
                        result[j] |= iMask;
                    }

                    longView >>= 1;
                }

                return result;
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

        public static long GetSimpleCode(List<int> vector)
        {
            var result = 0L;

            var currentMask = 1;
            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = i + 1, jMask = 1 << (i + 1); j < vector.Count; j++, jMask <<= 1)
                {
                    if ((vector[i] & jMask) != 0)
                    {
                        result ^= currentMask;
                    }

                    currentMask <<= 1;
                }
            }

            return result;
        }

        public static long GetMaxCode(List<int> vector, int bitsCount)
        {
            var result = 0L;

            var currentMask = 1L << bitsCount;
            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = i + 1, jMask = 1 << (i + 1); j < vector.Count; j++, jMask <<= 1)
                {
                    if ((vector[i] & jMask) != 0)
                    {
                        result ^= currentMask;
                    }

                    currentMask >>= 1;
                }
            }

            return result;
        }

        public static BigInteger GetBigIntegerSimpleCode(Graph g)
        {
            BigInteger bi = 0;
            for (int i = 0; i < g.VertexCount - 1; i++)
            {
                for (int j = i + 1; j < g.VertexCount; j++)
                {
                    if ((g[i] & (1 << j)) > 0)
                        bi++;

                    bi <<= 1;
                }
            }

            bi >>= 1;

            return bi;
        }

        public static long GetBigIntegerMaxCode(Graph g, int bitsCount)
        {
            long result = 0;
            var mask = 1L << bitsCount;
            for (int i = 0; i < g.VertexCount - 1; i++)
            {
                for (int j = i + 1; j < g.VertexCount; j++)
                {
                    if ((g[i] & (1 << j)) > 0)
                        result++;

                    result ^= mask;
                    mask >>= 1;
                }
            }

            result ^= (1L << bitsCount);

            return result;
        }
    }
}

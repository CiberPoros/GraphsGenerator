using System.Collections.Generic;

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
    }
}

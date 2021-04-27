using System;
using System.Collections.Generic;
using System.Numerics;
using static GraphsGenerator.Graph;

namespace GraphsGenerator
{
    class IsomorphismChecker
    {
        static int[] psiObratn = null;
        static BigInteger psiCanCode = 0;
        static int gcaFirst = 0;
        static int gcaCannon = 0;

        public static BigInteger GetCon(Graph g)
        {
            LinkedList<int> baseList = new LinkedList<int>();
            baseList.AddFirst((1 << g.VertexCount) - 1);
            BigInteger bi = -1;
            Permutation(ref g, baseList, ref bi);

            return bi;
        }

        public static int Permutation(ref Graph g, LinkedList<int> subdivisions, ref BigInteger maxCanCode)
        {
            Refine(ref g, subdivisions);

            if (g.VertexCount == subdivisions.Count)
            {
                int[] func = new int[subdivisions.Count];
                int iter = 0;
                foreach (var val in subdivisions)
                {
                    func[iter] = val;
                    iter++;
                }

                BigInteger bi = 0;
                for (int i = 0; i < g.VertexCount - 1; i++)
                {
                    for (int j = i + 1; j < g.VertexCount; j++)
                    {
                        if ((g[BitToInt[func[i]]] & func[j]) > 0)
                            bi++;

                        bi <<= 1;
                    }
                }

                bi >>= 1;

                if (bi == maxCanCode)
                    return 1;

                if (bi > maxCanCode)
                {
                    maxCanCode = bi;
                    return 2;
                }

                return 0;
            }

            int isCan = 0;
            for (var i = subdivisions.First; null != i; i = i.Next)
            {
                int cntBits = 0;
                int n = i.Value;
                while (n > 0)
                {
                    cntBits += 1 & n;
                    n >>= 1;

                    if (cntBits == 2)
                        break;
                }

                if (cntBits > 1)
                {
                    n = i.Value;

                    while (n > 0)
                    {
                        int m = n & (n - 1);

                        LinkedList<int> l = new LinkedList<int>(subdivisions);

                        for (var it = l.First; null != it; it = it.Next)
                        {
                            if (it.Value == i.Value)
                            {
                                l.AddBefore(it, n - m);
                                it.Value -= (n - m);
                                break;
                            }
                        }

                        n = m;

                        int localIsCan = Permutation(ref g, l, ref maxCanCode);

                        isCan = Math.Max(isCan, localIsCan);

                        if (localIsCan == 1 && isCan < 2)
                            return 1;
                    }

                    break;
                }
            }

            return isCan;
        }

        public static void Refine(ref Graph g, LinkedList<int> subdivisions)
        {
            for (var i = subdivisions.First; null != i;)
            {
                bool f = false;

                LinkedListNode<int> nodeForI = null;

                for (var j = subdivisions.First; null != j;)
                {
                    int n = j.Value;
                    SortedDictionary<int, int> d = new SortedDictionary<int, int>();
                    while (n > 0)
                    {
                        int m = n & (n - 1);
                        int res = g.AdjacencyVector[BitToInt[n - m]] & i.Value;

                        int cntBits = 0;
                        while (res > 0)
                        {
                            cntBits += 1 & res;
                            res >>= 1;
                        }

                        if (d.ContainsKey(cntBits))
                            d[cntBits] |= n - m;
                        else
                            d.Add(cntBits, n - m);

                        n = m;
                    }

                    if (d.Count == 1)
                    {
                        j = j.Next;
                        continue;
                    }

                    var next = j.Next;

                    foreach (var kvp in d)
                    {
                        subdivisions.AddAfter(j, kvp.Value);

                        if (null == nodeForI)
                            nodeForI = j.Previous;
                    }

                    subdivisions.Remove(j);
                    j = next;

                    f = true;
                }

                if (f)
                    i = nodeForI;
                else
                    i = i.Next;
            }
        }

        public static int FirstNode(ref Graph g, LinkedList<int> subdivisions, ref BigInteger maxCanCode, int level)
        {
            Refine(ref g, subdivisions);

            if (g.VertexCount == subdivisions.Count)
            {
                psiObratn = new int[subdivisions.Count];
                int[] прямоеОтображение = new int[subdivisions.Count];
                int iter = 0;
                foreach (var val in subdivisions)
                {
                    psiObratn[BitToInt[val]] = iter;
                    прямоеОтображение[iter] = val;
                    iter++;
                }

                gcaFirst = level;

                BigInteger bi = 0;
                for (int i = 0; i < g.VertexCount - 1; i++)
                {
                    for (int j = i + 1; j < g.VertexCount; j++)
                    {
                        if ((g[BitToInt[прямоеОтображение[i]]] & прямоеОтображение[j]) > 0)
                            bi++;

                        bi <<= 1;
                    }
                }

                bi >>= 1;

                psiCanCode = bi;

                maxCanCode = bi;
                gcaCannon = level;

                return level - 1;
            }

            for (var i = subdivisions.First; null != i; i = i.Next)
            {
                int cntBits = 0;
                int n = i.Value;
                while (n > 0)
                {
                    cntBits += 1 & n;
                    n >>= 1;

                    if (cntBits == 2)
                        break;
                }

                if (cntBits > 1)
                {
                    n = i.Value;

                    LinkedListNode<int> firstNode = null;
                    bool isFirst = true;
                    while (n > 0)
                    {
                        int m = n & (n - 1);

                        LinkedList<int> l = new LinkedList<int>(subdivisions);

                        for (var it = l.First; null != it; it = it.Next)
                        {
                            if (it.Value == i.Value)
                            {
                                l.AddBefore(it, n - m);
                                it.Value -= (n - m);

                                if (null == firstNode)
                                    firstNode = it.Previous;

                                break;
                            }
                        }

                        n = m;

                        int rtnLevel = -1;
                        if (isFirst)
                        {
                            rtnLevel = FirstNode(ref g, l, ref maxCanCode, level + 1);
                            gcaFirst = level;
                            isFirst = false;
                        }
                        else
                        {
                            rtnLevel = OtherNode(ref g, l, ref maxCanCode, level + 1);
                        }

                        if (rtnLevel < level)
                            return rtnLevel;

                        if (level < gcaCannon)
                            gcaCannon = level;
                    }

                    break;
                }
            }

            return level - 1;
        }

        public static int OtherNode(ref Graph g, LinkedList<int> subdivisions, ref BigInteger maxCanCode, int level)
        {
            Refine(ref g, subdivisions);

            if (g.VertexCount == subdivisions.Count)
            {
                int[] прямоеОтображение = new int[subdivisions.Count];
                int iter = 0;
                foreach (var val in subdivisions)
                {
                    прямоеОтображение[iter] = val;
                    iter++;
                }

                BigInteger bi = 0;
                for (int i = 0; i < g.VertexCount - 1; i++)
                {
                    for (int j = i + 1; j < g.VertexCount; j++)
                    {
                        if ((g[BitToInt[прямоеОтображение[i]]] & прямоеОтображение[j]) > 0)
                            bi++;

                        bi <<= 1;
                    }
                }

                bi >>= 1;

                if (bi == psiCanCode)
                {
                    return gcaFirst;
                }

                if (bi == maxCanCode)
                {
                    return gcaCannon;
                }
                else if (bi > maxCanCode)
                {
                    maxCanCode = bi;
                }

                return level - 1;
            }

            for (var i = subdivisions.First; null != i; i = i.Next)
            {
                int cntBits = 0;
                int n = i.Value;
                while (n > 0)
                {
                    cntBits += 1 & n;
                    n >>= 1;

                    if (cntBits == 2)
                        break;
                }

                if (cntBits > 1)
                {
                    n = i.Value;

                    while (n > 0)
                    {
                        int m = n & (n - 1);

                        LinkedList<int> l = new LinkedList<int>(subdivisions);

                        for (var it = l.First; null != it; it = it.Next)
                        {
                            if (it.Value == i.Value)
                            {
                                l.AddBefore(it, n - m);
                                it.Value -= (n - m);

                                break;
                            }
                        }

                        n = m;

                        int rtnLevel = OtherNode(ref g, l, ref maxCanCode, level + 1);

                        if (rtnLevel < level)
                            return rtnLevel;

                        if (level < gcaCannon)
                            gcaCannon = level;
                    }

                    break;
                }
            }

            return level - 1;
        }
    }

    class IntComparer : IComparer<int>
    {
        public int Compare(int a, int b)
        {
            return b.CompareTo(a);
        }
    }
}

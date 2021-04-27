using System;
using System.Collections.Generic;

namespace GraphsGenerator
{
    public class Graph
    {
        public static readonly Dictionary<int, int> BitToInt = new Dictionary<int, int>()
        {
            { 1, 0 },
            { 1 << 1, 1 },
            { 1 << 2, 2 },
            { 1 << 3, 3 },
            { 1 << 4, 4 },
            { 1 << 5, 5 },
            { 1 << 6, 6 },
            { 1 << 7, 7 },
            { 1 << 8, 8 },
            { 1 << 9, 9 },
            { 1 << 10, 10 },
            { 1 << 11, 11 },
            { 1 << 12, 12 },
            { 1 << 13, 13 },
            { 1 << 14, 14 },
            { 1 << 15, 15 },
            { 1 << 16, 16 },
            { 1 << 17, 17 },
            { 1 << 18, 18 },
            { 1 << 19, 19 },
            { 1 << 20, 20 },
            { 1 << 21, 21 },
            { 1 << 22, 22 },
            { 1 << 23, 23 },
            { 1 << 24, 24 },
            { 1 << 25, 25 },
            { 1 << 26, 26 },
            { 1 << 27, 27 },
            { 1 << 28, 28 },
            { 1 << 29, 29 }
        };

        private List<int> _adjacencyVector;

        public int VertexCount { get => _adjacencyVector.Count; }
        public List<int> AdjacencyVector { get => _adjacencyVector; set => _adjacencyVector = value; }

        public int this[int index]
        {
            get
            {
                return _adjacencyVector[index];
            }
            set
            {
                _adjacencyVector[index] = value;
            }
        }

        public int this[int index1, int index2]
        {
            get
            {
                return (_adjacencyVector[index1] & (1 << index2));
            }
            set
            {
                _adjacencyVector[index1] = _adjacencyVector[index1] | (1 << value);
            }
        }

        public Graph() => _adjacencyVector = new List<int>(31);
        public Graph(List<int> adjacencyVector)
        {
            _adjacencyVector = new List<int>(31);
            foreach (var val in adjacencyVector)
                _adjacencyVector.Add(val);
        }

        public Graph(bool[,] adjacencyMatrix)
        {
            _adjacencyVector = new List<int>(31);

            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                int offset = 1;
                int val = 0;
                for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    if (adjacencyMatrix[i, j])
                        val |= offset;

                    offset <<= 1;
                }

                _adjacencyVector.Add(val);
            }
        }

        public void AddVertex(int vertex) => _adjacencyVector.Add(vertex);

        public Graph CopyGr() => new Graph(AdjacencyVector);

        public string[] ToStringAdjacencyMatrix()
        {
            string[] res = new string[VertexCount];

            for (int i = 0; i < VertexCount; i++)
            {
                int k = 1;
                string s = "";
                for (int j = 0; j < VertexCount; j++)
                {
                    s = s + ((AdjacencyVector[i] & k) > 0 ? "1 " : "0 ");
                    k <<= 1;
                }

                s = s.TrimEnd(' ');
                res[i] = s;
            }

            return res;
        }

        public string ToListEdges()
        {
            string res = "";

            for (int i = 0; i < VertexCount; i++)
                for (int j = i + 1, mask = 1 << j; j < VertexCount; j++, mask <<= 1)
                    if ((AdjacencyVector[i] & mask) > 0)
                        res = res + (i + 1).ToString() + " " + (j + 1).ToString() + '\n';

            return res;
        }

        public string ToStringVector()
        {
            string res = "";

            for (int i = 0; i < VertexCount; i++)
            {
                res = res + AdjacencyVector[i].ToString() + ",";
            }

            return res.TrimEnd(',');
        }

        public void OutToConsoleMatrix()
        {
            string[] s = ToStringAdjacencyMatrix();
            foreach (var val in s)
                Console.WriteLine(val);

            Console.WriteLine();
        }

        public static Graph G6ToGraph(string s)
        {
            int n = s[0] - '?';
            List<int> li = new List<int>(n);
            for (int i = 0; i < n; i++)
                li.Add(0);

            int rOffset = 32;
            int k = 1;
            int val = s[k] - '?';

            int it = 1;

            for (int i = 0; i < n; i++)
            {
                int offset = 1;
                for (int j = 0; j < i; j++)
                {
                    if ((val & rOffset) > 0)
                    {
                        li[i] |= offset;
                        li[j] |= it;
                    }

                    rOffset >>= 1;
                    offset <<= 1;

                    if (rOffset == 0)
                    {
                        rOffset = 32;
                        k++;
                        if (k < s.Length)
                            val = s[k] - '?';
                        else if (j != i - 1)
                        {
                            throw new Exception("Ошибка формата g6");
                        }
                    }
                }

                it <<= 1;
            }

            return new Graph(li);
        }

        public string ToG6()
        {
            string res = ((char)('?' + VertexCount)).ToString();

            char c = '?';
            int rOffset = 32;
            for (int i = 1; i < VertexCount; i++)
            {
                int offset = 1;
                for (int j = 0; j < i; j++)
                {
                    if ((_adjacencyVector[i] & offset) > 0)
                        c += (char)rOffset;

                    rOffset >>= 1;
                    offset <<= 1;
                    if (rOffset == 0)
                    {
                        res += c;
                        c = '?';
                        rOffset = 32;
                    }
                }
            }

            if (c != '?')
                res += c;

            return res;
        }
    }
}

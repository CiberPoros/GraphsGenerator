namespace GraphsGenerator
{
    public static class ConnectivityChecker
    {
        public static bool IsConnected(Graph g)
        {
            var used = 0;
            Dfs(g, ref used);
            return used == (1 << (g.VertexCount)) - 1;
        }

        private static void Dfs(Graph g, ref int used, int v = 0)
        {
            used |= 1 << v;

            for (int i = 0, mask = 1; i < g.VertexCount; i++, mask <<= 1)
            {
                if ((g.AdjacencyVector[v] & mask) != 0 && (used & mask) == 0)
                {
                    Dfs(g, ref used, i);
                }
            }
        }
    }
}

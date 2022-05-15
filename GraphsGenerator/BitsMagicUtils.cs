namespace GraphsGenerator
{
    internal class BitsMagicUtils
    {
        const int FullByteMask = 0b11111111;
        private static int[] _bitsCount;

        private static void InitBitsCount()
        {
            _bitsCount = new int[65536];

            for (int i = 0; i < 65536; i++)
            {
                var current = i;
                var res = 0;
                while (current > 0)
                {
                    res++;
                    current &= current - 1;  // Забираем младшую единичку.
                }

                _bitsCount[i] = res;
            }
        }

        public static int GetCountOfBits(long value)
        {
            if (_bitsCount is null)
            {
                InitBitsCount();
            }

            var res = 0;
            while (value > 0)
            {
                res += _bitsCount[value & FullByteMask];
                value >>= 8; // byte size
            }

            return res;
        }

        public static int GetCountOfBitsInt(int value)
        {
            if (_bitsCount is null)
            {
                InitBitsCount();
            }

            var res = 0;
            while (value > 0)
            {
                res += _bitsCount[value & FullByteMask];
                value >>= 8; // byte size
            }

            return res;
        }
    }
}

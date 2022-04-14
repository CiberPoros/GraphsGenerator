namespace GraphsGenerator
{
    internal enum GeneratorType
    {
        /// <summary>
        /// Использует илгоритм получения канонического кода как из nauty
        /// </summary>
        GENERATOR_BY_CANONICAL_CODE = 1,
        /// <summary>
        /// Перебирает все графы для каждого кода
        /// </summary>
        BRUTE_FORCE_ALL_GRAPHS = 2,
        /// <summary>
        /// Перебирает все коды
        /// </summary>
        BRUTE_FORCE_ALL_CODES = 3,
        /// <summary>
        /// Перебирает все коды, останавливая рекурсивный процесс, если текущий код не канонический
        /// </summary>
        BRUTE_FORCE_ALL_CODES_WITH_FILTER = 4
    }
}

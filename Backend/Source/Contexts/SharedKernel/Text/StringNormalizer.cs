namespace SharedKernel.Text
{
    /// <summary>
    /// Provides utility methods for normalizing string values.
    /// This class applies only technical normalization and must not contain business rules.
    /// </summary>
    public static class StringNormalizer
    {
        /// <summary>
        /// Normalizes a string by trimming leading/trailing whitespace
        /// and collapsing multiple internal spaces into a single space.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// - <c>null</c> if the input is null  
        /// - An empty string if the input contains only whitespace  
        /// - A normalized string otherwise
        /// </returns>
        public static string? Normalize(string? input)
        {
            if (input is null)
                return null;

            var trimmed = input.Trim();

            if (string.IsNullOrEmpty(trimmed))
                return string.Empty;

            return CollapseWhitespace(trimmed);
        }

        /// <summary>
        /// Normalizes a string and guarantees a non-null result.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>
        /// A normalized string. Returns empty string if input is null.
        /// </returns>
        public static string NormalizeOrEmpty(string? input)
            => Normalize(input) ?? string.Empty;

        /// <summary>
        /// Collapses multiple whitespace characters into a single space.
        /// </summary>
        private static string CollapseWhitespace(string input)
        {
            var parts = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return string.Join(' ', parts);
        }
    }
}
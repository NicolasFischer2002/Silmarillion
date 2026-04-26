namespace SharedKernel.Validation
{
    /// <summary>
    /// Provides helper methods for validating enum values.
    /// </summary>
    public static class EnumGuard
    {
        /// <summary>
        /// Determines whether the specified enum value is defined in its enumeration type.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="value">The enum value to validate.</param>
        /// <returns>
        /// <c>true</c> if the value is defined in the enum; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// In C#, enum types can hold values that are not explicitly defined,
        /// especially when cast from numeric types or when using the default value (0).
        /// This method ensures the value is a valid member of the enum.
        /// </remarks>
        public static bool IsDefined<TEnum>(TEnum value)
            where TEnum : struct, Enum
        {
            return Enum.IsDefined(value);
        }
    }
}
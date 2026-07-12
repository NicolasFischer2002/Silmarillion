using Domain.Errors;
using SharedKernel.Results;

namespace Domain.Policies
{
    public static class CnpjPolicy
    {
        public const int DigitsLength = 14;

        public static Result<string> Validate(string? value)
        {
            var normalized = Normalize(value);

            if (string.IsNullOrWhiteSpace(normalized))
                return Result<string>.Failure(OrganizationErrors.CnpjRequired());

            if (normalized.Length != DigitsLength)
                return Result<string>.Failure(OrganizationErrors.CnpjInvalid());

            if (AllDigitsAreEqual(normalized))
                return Result<string>.Failure(OrganizationErrors.CnpjInvalid());

            if (!IsValidCheckDigits(normalized))
                return Result<string>.Failure(OrganizationErrors.CnpjInvalid());

            return Result<string>.Success(normalized);
        }

        private static string? Normalize(string? value)
        {
            if (value is null)
                return null;

            var trimmed = value.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
                return string.Empty;

            return new string([.. trimmed.Where(char.IsDigit)]);
        }

        private static bool AllDigitsAreEqual(string value)
            => value.Distinct().Count() == 1;

        private static bool IsValidCheckDigits(string cnpj)
        {
            var numbers = cnpj.Select(c => c - '0').ToArray();

            int[] weights1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] weights2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

            var digit1 = CalculateDigit(numbers[..12], weights1);
            var digit2 = CalculateDigit(numbers[..13], weights2);

            return numbers[12] == digit1 && numbers[13] == digit2;
        }

        private static int CalculateDigit(int[] numbers, int[] weights)
        {
            var sum = numbers.Zip(weights, (n, w) => n * w).Sum();
            var remainder = sum % 11;

            return remainder < 2 ? 0 : 11 - remainder;
        }
    }
}
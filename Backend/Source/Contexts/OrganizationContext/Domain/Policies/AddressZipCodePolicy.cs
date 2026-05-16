using Domain.Errors;
using SharedKernel.Results;

namespace Domain.Policies
{
    public static class AddressZipCodePolicy
    {
        public const int DigitsLength = 8;

        public static Result<string> Validate(string? normalized)
        {
            if (string.IsNullOrWhiteSpace(normalized))
                return Result<string>.Failure(OrganizationErrors.ZipCodeRequired());

            if (normalized.Length != DigitsLength)
                return Result<string>.Failure(OrganizationErrors.ZipCodeInvalid());

            if (normalized.Distinct().Count() == 1)
                return Result<string>.Failure(OrganizationErrors.ZipCodeInvalid());

            return Result<string>.Success(normalized);
        }
    }
}
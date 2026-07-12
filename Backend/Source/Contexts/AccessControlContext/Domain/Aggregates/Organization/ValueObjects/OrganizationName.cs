using Domain.Aggregates.Organization.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Aggregates.Organization.ValueObjects
{
    public sealed record OrganizationName
    {
        public string Value { get; }
        private const int MaxLength = 150;
        private const int MinLength = 1;

        private OrganizationName(string value)
        {
            Value = value;
        }

        public static Result<OrganizationName> Create(string? value)
        {
            var normalizedValue = StringNormalizer.Normalize(value);

            if (string.IsNullOrWhiteSpace(normalizedValue))
                return Result<OrganizationName>.Failure(OrganizationErrors.OrganizationNameRequired());

            if (normalizedValue.Length < MinLength)
                return Result<OrganizationName>.Failure(OrganizationErrors.OrganizationNameTooShort(MinLength));

            if (normalizedValue.Length > MaxLength)
                return Result<OrganizationName>.Failure(OrganizationErrors.OrganizationNameTooLong(MaxLength));

            return Result<OrganizationName>.Success(new OrganizationName(normalizedValue));
        }

        public override string ToString() => Value;
    }
}
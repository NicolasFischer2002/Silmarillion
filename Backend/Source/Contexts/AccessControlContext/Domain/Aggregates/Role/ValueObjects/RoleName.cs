using Domain.Aggregates.Role.Errors;
using SharedKernel.Results;
using SharedKernel.Text;

namespace Domain.Aggregates.Role.ValueObjects
{
    public sealed record RoleName
    {
        public string Value { get; }
        private const int MinLength = 3;
        private const int MaxLength = 100;

        private RoleName(string name)
        {
            Value = name;
        }

        public static Result<RoleName> Create(string? name)
        {
            var normalizedValue = StringNormalizer.NormalizeOrEmpty(name);

            if (string.IsNullOrWhiteSpace(normalizedValue))
                return Result<RoleName>.Failure(RoleNameErrors.NameRequired());

            if (normalizedValue.Length < MinLength)
                return Result<RoleName>.Failure(RoleNameErrors.NameTooShort(MinLength));

            if (normalizedValue.Length > MaxLength)
                return Result<RoleName>.Failure(RoleNameErrors.NameTooLong(MaxLength));

            return Result<RoleName>.Success(new RoleName(normalizedValue));
        }

        public override string ToString() => Value;
    }
}
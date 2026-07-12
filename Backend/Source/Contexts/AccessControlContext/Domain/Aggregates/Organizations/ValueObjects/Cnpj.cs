using Domain.Aggregates.Organizations.Policies;
using SharedKernel.Results;

namespace Domain.Aggregates.Organizations.ValueObjects
{
    public sealed record Cnpj
    {
        public string Value { get; }

        private Cnpj(string value)
        {
            Value = value;
        }

        public static Result<Cnpj> Create(string? value)
        {
            var validation = CnpjPolicy.Validate(value);

            if (validation.IsFailure)
                return Result<Cnpj>.Failure(validation.Errors);

            return Result<Cnpj>.Success(new Cnpj(validation.Value));
        }

        public string GetFormattedValue()
        {
            return $"{Value[..2]}.{Value[2..5]}.{Value[5..8]}/{Value[8..12]}-{Value[12..14]}";
        }

        public override string ToString() => Value;
    }
}
using Domain.Aggregates.Session.Errors;
using SharedKernel.Results;
using SharedKernel.Text;
using System.Net.Sockets;

namespace Domain.Aggregates.Session.ValueObjects
{
    public sealed record IPAddress
    {
        public string Value { get; }

        private const int MaxLength = 45;

        private IPAddress(string value)
        {
            Value = value;
        }

        public static Result<IPAddress> Create(string? value)
        {
            var normalizedValue = StringNormalizer.Normalize(value);

            if (string.IsNullOrWhiteSpace(normalizedValue))
                return Result<IPAddress>.Failure(IPAddressErrors.IpAddressRequired());

            if (normalizedValue.Length > MaxLength)
                return Result<IPAddress>.Failure(IPAddressErrors.IpAddressTooLong(MaxLength));

            if (!System.Net.IPAddress.TryParse(normalizedValue, out var ip))
                return Result<IPAddress>.Failure(IPAddressErrors.IpAddressInvalid());

            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                // IPv4 Must consist of exactly four octets
                if (normalizedValue.Split('.').Length != 4)
                    return Result<IPAddress>.Failure(IPAddressErrors.IpAddressInvalid());
            }

            return Result<IPAddress>.Success(
                new IPAddress(normalizedValue));
        }

        public override string ToString() => Value;
    }
}
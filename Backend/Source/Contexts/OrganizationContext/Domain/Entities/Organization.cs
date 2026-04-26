using Domain.Enums;
using Domain.ValueObjects;
using SharedKernel.Results;

namespace Domain.Entities
{
    public sealed class Organization
    {
        public Guid Id { get; }
        public OrganizationName OrganizationName { get; private set; }
        public Cnpj Cnpj { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
        public Address Address { get; private set; }
        public OrganizationStatus OrganizationStatus { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime LastModifiedAt { get; private set; }

        private Organization(
            Guid id,
            OrganizationName organizationName,
            Cnpj cnpj,
            EmailAddress emailAddress,
            Address address,
            OrganizationStatus organizationStatus,
            DateTime createdAt,
            DateTime lastModifiedAt)
        {
            Id = id;
            OrganizationName = organizationName;
            Cnpj = cnpj;
            EmailAddress = emailAddress;
            Address = address;
            OrganizationStatus = organizationStatus;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public static Result<Organization> Create(
            OrganizationName organizationName,
            Cnpj cnpj,
            EmailAddress emailAddress,
            Address address)
        {
            var now = DateTime.UtcNow;

            return Result<Organization>.Success(new Organization(
                Guid.NewGuid(), organizationName, cnpj, emailAddress, address, OrganizationStatus.Active, now, now
            ));
        }
    }
}
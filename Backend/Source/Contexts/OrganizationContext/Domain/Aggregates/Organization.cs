using Domain.Enums;
using Domain.Errors;
using Domain.ValueObjects;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace Domain.Aggregates
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
            Guid Id,
            OrganizationName organizationName,
            Cnpj cnpj,
            EmailAddress emailAddress,
            Address address)
        {
            if (Id == Guid.Empty)
                return Result<Organization>.Failure(OrganizationErrors.OrganizationIdInvalid());

            if (organizationName is null)
                return Result<Organization>.Failure(OrganizationErrors.OrganizationNameRequired());

            if (cnpj is null)
                return Result<Organization>.Failure(OrganizationErrors.CnpjRequired());

            if (emailAddress is null)
                return Result<Organization>.Failure(EmailAddressPolicyErrors.EmailRequired());

            if (address is null)
                return Result<Organization>.Failure(OrganizationErrors.AddressRequired());

            var now = DateTime.UtcNow;

            return Result<Organization>.Success(new Organization(
                Id, organizationName, cnpj, emailAddress, address, OrganizationStatus.Active, now, now
            ));
        }
    }
}
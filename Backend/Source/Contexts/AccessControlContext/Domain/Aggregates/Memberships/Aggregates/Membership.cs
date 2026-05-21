using Domain.Aggregates.Memberships.Constants;
using Domain.Aggregates.Memberships.Errors;
using Domain.Aggregates.Memberships.Events;
using SharedKernel.DomainEvents;
using SharedKernel.Results;

namespace Domain.Aggregates.Memberships.Aggregates
{
    public sealed class Membership : Entity
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid OrganizationId { get; }
        public MembershipStatus Status { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime LastModifiedAt { get; private set; }

        private Membership(
            Guid id, 
            Guid userId, 
            Guid organizationId, 
            MembershipStatus status, 
            DateTime createdAt, 
            DateTime lastModifiedAt)
        {
            Id = id;
            UserId = userId;
            OrganizationId = organizationId;
            Status = status;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public static Result<Membership> CreatePending(
            Guid id,
            Guid userId,
            Guid organizationId)
        {
            if (id == Guid.Empty)
                return Result<Membership>.Failure(MembershipErrors.MembershipIdRequired());

            if (userId == Guid.Empty)
                return Result<Membership>.Failure(MembershipErrors.UserIdRequired());

            if (organizationId == Guid.Empty)
                return Result<Membership>.Failure(MembershipErrors.OrganizationIdRequired());

            var now = DateTime.UtcNow;

            return Result<Membership>.Success(new Membership(
                id, userId, organizationId, MembershipStatus.Pending, now, now
            ));
        }

        public Result Activate()
        {
            if (Status == MembershipStatus.Active)
                return Result.Failure(MembershipErrors.MembershipAlreadyActive());

            if (Status == MembershipStatus.Revoked)
                return Result.Failure(MembershipErrors.CannotActivateRevokedMembership());

            Status = MembershipStatus.Active;

            var now = DateTime.UtcNow;

            LastModifiedAt = now;

            AddDomainEvent(
                new MembershipActivatedDomainEvent(
                    Id,
                    UserId,
                    OrganizationId,
                    now
                )
            );

            return Result.Success();
        }

        public Result Deactivate()
        {
            if (Status == MembershipStatus.Inactive)
                return Result.Failure(MembershipErrors.MembershipAlreadyInactive());

            if (Status == MembershipStatus.Revoked)
                return Result.Failure(MembershipErrors.CannotDeactivateRevokedMembership());

            Status = MembershipStatus.Inactive;

            var now = DateTime.UtcNow;

            LastModifiedAt = now;

            AddDomainEvent(
                new MembershipDeactivatedDomainEvent(
                    Id,
                    UserId,
                    OrganizationId,
                    now
                )
            );

            return Result.Success();
        }

        public Result Revoke()
        {
            if (Status == MembershipStatus.Revoked)
                return Result.Failure(MembershipErrors.MembershipAlreadyRevoked());

            Status = MembershipStatus.Revoked;

            var now = DateTime.UtcNow;

            LastModifiedAt = now;

            AddDomainEvent(
                new MembershipRevokedDomainEvent(
                    Id,
                    UserId,
                    OrganizationId,
                    now
                )
            );

            return Result.Success();
        }
    }
}
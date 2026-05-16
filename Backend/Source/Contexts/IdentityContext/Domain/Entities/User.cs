using Domain.Enums;
using Domain.Errors;
using Domain.ValueObjects;
using SharedKernel.Errors;
using SharedKernel.Results;

namespace Domain.Entities
{
    public sealed class User
    {
        public Guid Id { get; }
        public FullName FullName { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
        public PasswordHash PasswordHash { get; private set; }
        public UserStatus Status { get; private set; }
        public bool MustChangePassword { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime LastModifiedAt { get; private set; }

        private User(
            Guid id,
            FullName fullName,
            EmailAddress emailAddress,
            PasswordHash passwordHash,
            UserStatus status,
            bool mustChangePassword,
            DateTime createdAt,
            DateTime lastModifiedAt)
        {
            Id = id;
            FullName = fullName;
            EmailAddress = emailAddress;
            PasswordHash = passwordHash;
            Status = status;
            MustChangePassword = mustChangePassword;
            CreatedAt = createdAt;
            LastModifiedAt = lastModifiedAt;
        }

        public static Result<User> CreatePending(
            Guid id,
            FullName fullName,
            EmailAddress emailAddress,
            PasswordHash passwordHash)
        {
            if (id == Guid.Empty)
                return Result<User>.Failure(IdentityErrors.UserIdInvalid());

            if (fullName is null)
                return Result<User>.Failure(IdentityErrors.FullNameRequired());

            if (emailAddress is null)
                return Result<User>.Failure(EmailAddressPolicyErrors.EmailRequired());

            if (passwordHash is null)
                return Result<User>.Failure(IdentityErrors.PasswordHashRequired());

            var user = new User(
                id,
                fullName,
                emailAddress,
                passwordHash,
                UserStatus.Pending,
                true,
                DateTime.UtcNow,
                DateTime.UtcNow
            );

            return Result<User>.Success(user);
        }

        public Result ChangeFullName(FullName fullName)
        {            
            if (fullName is null)
                return Result.Failure(IdentityErrors.FullNameRequired());

            FullName = fullName;
            LastModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result ChangeEmailAddress(EmailAddress emailAddress)
        {
            if (emailAddress is null)
                return Result.Failure(EmailAddressPolicyErrors.EmailRequired());
            
            EmailAddress = emailAddress;
            LastModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result ChangePasswordHash(PasswordHash passwordHash)
        {
            if (passwordHash is null)
                return Result.Failure(IdentityErrors.PasswordHashRequired());

            PasswordHash = passwordHash;
            MustChangePassword = false;
            LastModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Activate()
        {
            if (Status == UserStatus.Active)
                return Result.Failure(IdentityErrors.UserAlreadyActive());

            if (MustChangePassword)
                return Result.Failure(IdentityErrors.PasswordChangeRequired());

            Status = UserStatus.Active;
            LastModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Deactivate()
        {
            if (Status == UserStatus.Inactive)
                return Result.Failure(IdentityErrors.UserAlreadyInactive());

            Status = UserStatus.Inactive;
            LastModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result RequirePasswordChange()
        {
            if (MustChangePassword)
                return Result.Success();

            MustChangePassword = true;
            LastModifiedAt = DateTime.UtcNow;

            return Result.Success();
        }
    }
}
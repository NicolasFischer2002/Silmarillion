using Domain.Aggregates;
using Domain.Enums;
using Domain.Errors;
using Domain.ValueObjects;
using SharedKernel.Errors;
using SharedTests.Assertions;

namespace Tests.Domain.Aggregates;

[TestClass]
public class UserTests
{
    private static FullName ValidFullName()
        => FullName.Create("João da Silva").Value;

    private static EmailAddress ValidEmail()
        => EmailAddress.Create("joao.silva@example.com").Value;

    private static PasswordHash ValidPasswordHash()
        => PasswordHash.Create("hashed_password").Value;

    private static User CreateValidUser()
    {
        var result = User.CreatePending(
            Guid.NewGuid(),
            FullName.Create("João da Silva").Value,
            EmailAddress.Create("joao.silva@example.com").Value,
            PasswordHash.Create("hashed_password").Value
        );

        Assert.IsTrue(result.IsSuccess);
        return result.Value;
    }

    [TestMethod]
    public void CreatePending_ShouldReturnFailure_WhenIdIsEmpty()
    {
        var result = User.CreatePending(
            Guid.Empty,
            ValidFullName(),
            ValidEmail(),
            ValidPasswordHash()
        );

        result.ShouldFailWith(IdentityErrors.UserIdInvalid());
    }

    [TestMethod]
    public void CreatePending_ShouldReturnFailure_WhenFullNameIsNull()
    {
        var result = User.CreatePending(
            Guid.NewGuid(),
            null!,
            ValidEmail(),
            ValidPasswordHash()
        );

        result.ShouldFailWith(IdentityErrors.FullNameRequired());
    }

    [TestMethod]
    public void CreatePending_ShouldReturnFailure_WhenEmailIsNull()
    {
        var result = User.CreatePending(
            Guid.NewGuid(),
            ValidFullName(),
            null!,
            ValidPasswordHash()
        );

        result.ShouldFailWith(EmailAddressPolicyErrors.EmailRequired());
    }

    [TestMethod]
    public void CreatePending_ShouldReturnFailure_WhenPasswordHashIsNull()
    {
        var result = User.CreatePending(
            Guid.NewGuid(),
            ValidFullName(),
            ValidEmail(),
            null!
        );

        result.ShouldFailWith(IdentityErrors.PasswordHashRequired());
    }

    [TestMethod]
    public void CreatePending_ShouldReturnSuccess_WhenInputIsValid()
    {
        var id = Guid.NewGuid();
        var fullName = ValidFullName();
        var email = ValidEmail();
        var passwordHash = ValidPasswordHash();

        var result = User.CreatePending(
            id,
            fullName,
            email,
            passwordHash
        );

        result.ShouldSucceed();

        var user = result.Value;

        Assert.AreEqual(id, user.Id);
        Assert.AreEqual(fullName, user.FullName);
        Assert.AreEqual(email, user.EmailAddress);
        Assert.AreEqual(passwordHash, user.PasswordHash);

        Assert.AreEqual(UserStatus.Pending, user.Status);
        Assert.IsTrue(user.MustChangePassword);

        Assert.IsTrue(user.CreatedAt <= DateTime.UtcNow);
        Assert.IsTrue(user.LastModifiedAt <= DateTime.UtcNow);
    }

    [TestMethod]
    public void ChangeFullName_ShouldReturnFailure_WhenFullNameIsNull()
    {
        var user = CreateValidUser();

        var result = user.ChangeFullName(null!);

        result.ShouldFailWith(IdentityErrors.FullNameRequired());
    }

    [TestMethod]
    public void ChangeFullName_ShouldUpdateFullName_WhenValid()
    {
        var user = CreateValidUser();

        var newFullName = FullName.Create("Maria Souza Silva").Value;

        var result = user.ChangeFullName(newFullName);

        result.ShouldSucceed();
        Assert.AreEqual(newFullName, user.FullName);
    }

    [TestMethod]
    public void ChangeFullName_ShouldUpdateLastModifiedAt_WhenSuccessful()
    {
        var user = CreateValidUser();

        var originalLastModifiedAt = user.LastModifiedAt;

        var newFullName = FullName.Create("Carlos Oliveira Santos").Value;

        var result = user.ChangeFullName(newFullName);

        result.ShouldSucceed();
        Assert.IsTrue(user.LastModifiedAt > originalLastModifiedAt);
    }

    [TestMethod]
    public void ChangeEmailAddress_ShouldReturnFailure_WhenEmailIsNull()
    {
        var user = CreateValidUser();

        var result = user.ChangeEmailAddress(null!);

        result.ShouldFailWith(EmailAddressPolicyErrors.EmailRequired());
    }

    [TestMethod]
    public void ChangeEmailAddress_ShouldUpdateEmail_WhenValid()
    {
        var user = CreateValidUser();

        var newEmail = EmailAddress.Create("novo.email@example.com").Value;

        var result = user.ChangeEmailAddress(newEmail);

        result.ShouldSucceed();
        Assert.AreEqual(newEmail, user.EmailAddress);
    }

    [TestMethod]
    public void ChangeEmailAddress_ShouldUpdateLastModifiedAt_WhenSuccessful()
    {
        var user = CreateValidUser();

        var originalLastModifiedAt = user.LastModifiedAt;

        var newEmail = EmailAddress.Create("outro.email@example.com").Value;

        var result = user.ChangeEmailAddress(newEmail);

        result.ShouldSucceed();
        Assert.IsTrue(user.LastModifiedAt > originalLastModifiedAt);
    }

    [TestMethod]
    public void ChangePasswordHash_ShouldReturnFailure_WhenPasswordHashIsNull()
    {
        var user = CreateValidUser();

        var result = user.ChangePasswordHash(null!);

        result.ShouldFailWith(IdentityErrors.PasswordHashRequired());
    }

    [TestMethod]
    public void ChangePasswordHash_ShouldUpdatePasswordHash_WhenValid()
    {
        var user = CreateValidUser();

        var newPasswordHash = PasswordHash.Create("new_hashed_password").Value;

        var result = user.ChangePasswordHash(newPasswordHash);

        result.ShouldSucceed();
        Assert.AreEqual(newPasswordHash, user.PasswordHash);
    }

    [TestMethod]
    public void ChangePasswordHash_ShouldSetMustChangePasswordToFalse_WhenSuccessful()
    {
        var user = CreateValidUser();

        Assert.IsTrue(user.MustChangePassword);

        var newPasswordHash = PasswordHash.Create("new_hashed_password").Value;

        var result = user.ChangePasswordHash(newPasswordHash);

        result.ShouldSucceed();
        Assert.IsFalse(user.MustChangePassword);
    }

    [TestMethod]
    public void ChangePasswordHash_ShouldUpdateLastModifiedAt_WhenSuccessful()
    {
        var user = CreateValidUser();

        var originalLastModifiedAt = user.LastModifiedAt;

        var newPasswordHash = PasswordHash.Create("another_hashed_password").Value;

        var result = user.ChangePasswordHash(newPasswordHash);

        result.ShouldSucceed();
        Assert.IsTrue(user.LastModifiedAt > originalLastModifiedAt);
    }

    [TestMethod]
    public void Activate_ShouldReturnFailure_WhenUserIsAlreadyActive()
    {
        var user = CreateValidUser();

        // First, resolve the password precondition
        var password = PasswordHash.Create("new_password_hash").Value;
        user.ChangePasswordHash(password);

        // Successfully activated
        user.Activate();

        // Try activating it again
        var result = user.Activate();

        result.ShouldFailWith(IdentityErrors.UserAlreadyActive());
    }

    [TestMethod]
    public void Activate_ShouldReturnFailure_WhenPasswordChangeIsRequired()
    {
        var user = CreateValidUser();

        var result = user.Activate();

        result.ShouldFailWith(IdentityErrors.PasswordChangeRequired());
    }

    [TestMethod]
    public void Activate_ShouldUpdateStatusToActive_WhenValid()
    {
        var user = CreateValidUser();

        var password = PasswordHash.Create("new_password_hash").Value;
        user.ChangePasswordHash(password);

        var result = user.Activate();

        result.ShouldSucceed();
        Assert.AreEqual(UserStatus.Active, user.Status);
    }

    [TestMethod]
    public void Activate_ShouldUpdateLastModifiedAt_WhenSuccessful()
    {
        var user = CreateValidUser();

        var password = PasswordHash.Create("new_password_hash").Value;
        user.ChangePasswordHash(password);

        var originalLastModifiedAt = user.LastModifiedAt;

        var result = user.Activate();

        result.ShouldSucceed();
        Assert.IsTrue(user.LastModifiedAt > originalLastModifiedAt);
    }

    [TestMethod]
    public void Deactivate_ShouldReturnFailure_WhenUserIsAlreadyInactive()
    {
        var user = CreateValidUser();

        // First deactivation should succeed
        user.Deactivate();

        // Second attempt should fail
        var result = user.Deactivate();

        result.ShouldFailWith(IdentityErrors.UserAlreadyInactive());
    }

    [TestMethod]
    public void Deactivate_ShouldUpdateStatusToInactive_WhenValid()
    {
        var user = CreateValidUser();

        var result = user.Deactivate();

        result.ShouldSucceed();
        Assert.AreEqual(UserStatus.Inactive, user.Status);
    }

    [TestMethod]
    public void Deactivate_ShouldUpdateLastModifiedAt_WhenSuccessful()
    {
        var user = CreateValidUser();

        var originalLastModifiedAt = user.LastModifiedAt;

        var result = user.Deactivate();

        result.ShouldSucceed();
        Assert.IsTrue(user.LastModifiedAt > originalLastModifiedAt);
    }

    [TestMethod]
    public void RequirePasswordChange_ShouldNotChangeState_WhenAlreadyRequired()
    {
        var user = CreateValidUser();

        // Precondition: user is created with MustChangePassword = true
        var originalLastModifiedAt = user.LastModifiedAt;

        var result = user.RequirePasswordChange();

        result.ShouldSucceed();
        Assert.IsTrue(user.MustChangePassword);
        Assert.AreEqual(originalLastModifiedAt, user.LastModifiedAt);
    }

    [TestMethod]
    public void RequirePasswordChange_ShouldSetMustChangePasswordToTrue_WhenNotRequired()
    {
        var user = CreateValidUser();

        // First, change password to disable requirement
        var password = PasswordHash.Create("new_password_hash").Value;
        user.ChangePasswordHash(password);

        Assert.IsFalse(user.MustChangePassword);

        var result = user.RequirePasswordChange();

        result.ShouldSucceed();
        Assert.IsTrue(user.MustChangePassword);
    }

    [TestMethod]
    public void RequirePasswordChange_ShouldUpdateLastModifiedAt_WhenStateChanges()
    {
        var user = CreateValidUser();

        // First, change password to disable requirement
        var password = PasswordHash.Create("new_password_hash").Value;
        user.ChangePasswordHash(password);

        var originalLastModifiedAt = user.LastModifiedAt;

        var result = user.RequirePasswordChange();

        result.ShouldSucceed();
        Assert.IsTrue(user.LastModifiedAt > originalLastModifiedAt);
    }
}
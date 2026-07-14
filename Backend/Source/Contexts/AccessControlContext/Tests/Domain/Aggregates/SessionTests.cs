using Domain.Aggregates.Session.Aggregate;
using Domain.Aggregates.Session.Errors;
using Domain.Aggregates.Session.Events;
using Domain.Aggregates.Session.ValueObjects;
using SharedTests.Assertions;

namespace Tests.Domain.Aggregates
{
    [TestClass]
    public class SessionTests
    {
        [TestMethod]
        public void Create_ShouldReturnFailure_WhenSessionIdIsEmpty()
        {
            var result = Session.Create(
                Guid.Empty,
                Guid.NewGuid(),
                ValidRefreshTokenHash(),
                DateTime.UtcNow.AddDays(30),
                ValidIPAddress(),
                ValidUserAgent());

            result.ShouldFailWith(SessionErrors.SessionIdInvalid());
        }

        [TestMethod]
        public void Create_ShouldReturnFailure_WhenUserIdIsEmpty()
        {
            var result = Session.Create(
                Guid.NewGuid(),
                Guid.Empty,
                ValidRefreshTokenHash(),
                DateTime.UtcNow.AddDays(30),
                ValidIPAddress(),
                ValidUserAgent());

            result.ShouldFailWith(SessionErrors.UserIdInvalid());
        }

        [TestMethod]
        public void Create_ShouldReturnFailure_WhenRefreshTokenHashIsNull()
        {
            var result = Session.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                null!,
                DateTime.UtcNow.AddDays(30),
                ValidIPAddress(),
                ValidUserAgent());

            result.ShouldFailWith(SessionErrors.RefreshTokenHashRequired());
        }

        [TestMethod]
        public void Create_ShouldReturnFailure_WhenIPAddressIsNull()
        {
            var result = Session.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ValidRefreshTokenHash(),
                DateTime.UtcNow.AddDays(30),
                null!,
                ValidUserAgent());

            result.ShouldFailWith(SessionErrors.IpAddressRequired());
        }

        [TestMethod]
        public void Create_ShouldReturnFailure_WhenUserAgentIsNull()
        {
            var result = Session.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ValidRefreshTokenHash(),
                DateTime.UtcNow.AddDays(30),
                ValidIPAddress(),
                null!);

            result.ShouldFailWith(SessionErrors.UserAgentRequired());
        }

        [TestMethod]
        public void Create_ShouldReturnFailure_WhenExpirationDateIsInvalid()
        {
            var result = Session.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ValidRefreshTokenHash(),
                DateTime.UtcNow.AddMinutes(-1),
                ValidIPAddress(),
                ValidUserAgent());

            result.ShouldFailWith(SessionErrors.ExpirationDateInvalid());
        }

        [TestMethod]
        public void Create_ShouldReturnSuccess_WhenValuesAreValid()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var expiresAt = DateTime.UtcNow.AddDays(30);

            var refreshToken = ValidRefreshTokenHash();
            var ipAddress = ValidIPAddress();
            var userAgent = ValidUserAgent();

            var result = Session.Create(
                id,
                userId,
                refreshToken,
                expiresAt,
                ipAddress,
                userAgent);

            result.ShouldSucceed();

            var session = result.Value;

            Assert.AreEqual(id, session.Id);
            Assert.AreEqual(userId, session.UserId);
            Assert.AreEqual(refreshToken, session.RefreshTokenHash);
            Assert.AreEqual(ipAddress, session.IPAddress);
            Assert.AreEqual(userAgent, session.UserAgent);
            Assert.AreEqual(expiresAt, session.ExpiresAt);

            Assert.IsNull(session.RevokedAt);
            Assert.IsNull(session.LastActivityAt);

            Assert.HasCount(1, session.DomainEvents);
            Assert.IsInstanceOfType(
                session.DomainEvents.Single(),
                typeof(SessionCreatedDomainEvent));
        }

        [TestMethod]
        public void Touch_ShouldUpdateLastActivityAt_WhenSessionIsActive()
        {
            var session = CreateValidSession();

            var result = session.Touch();

            result.ShouldSucceed();

            Assert.IsNotNull(session.LastActivityAt);
        }

        [TestMethod]
        public void Touch_ShouldReturnFailure_WhenSessionIsRevoked()
        {
            var session = CreateValidSession();

            session.Revoke();

            var result = session.Touch();

            result.ShouldFailWith(SessionErrors.SessionNotActive());
        }

        [TestMethod]
        public void RotateRefreshToken_ShouldReturnFailure_WhenRefreshTokenHashIsNull()
        {
            var session = CreateValidSession();

            var result = session.RotateRefreshToken(null!);

            result.ShouldFailWith(SessionErrors.RefreshTokenHashRequired());
        }

        [TestMethod]
        public void RotateRefreshToken_ShouldReturnFailure_WhenRefreshTokenIsAlreadyInUse()
        {
            var session = CreateValidSession();

            var result = session.RotateRefreshToken(session.RefreshTokenHash);

            result.ShouldFailWith(SessionErrors.RefreshTokenAlreadyInUse());
        }

        [TestMethod]
        public void RotateRefreshToken_ShouldUpdateRefreshTokenHash_WhenValid()
        {
            var session = CreateValidSession();

            var newHash = RefreshTokenHash
                .Create("new_refresh_token_hash")
                .Value;

            var result = session.RotateRefreshToken(newHash);

            result.ShouldSucceed();

            Assert.AreEqual(newHash, session.RefreshTokenHash);
            Assert.IsNotNull(session.LastActivityAt);

            Assert.HasCount(2, session.DomainEvents);
            Assert.IsInstanceOfType(
                session.DomainEvents.Last(),
                typeof(RefreshTokenRotatedDomainEvent));
        }

        [TestMethod]
        public void Revoke_ShouldReturnSuccess_WhenSessionIsActive()
        {
            var session = CreateValidSession();

            var result = session.Revoke();

            result.ShouldSucceed();

            Assert.IsNotNull(session.RevokedAt);

            Assert.HasCount(2, session.DomainEvents);
            Assert.IsInstanceOfType(
                session.DomainEvents.Last(),
                typeof(SessionRevokedDomainEvent));
        }

        [TestMethod]
        public void Revoke_ShouldReturnFailure_WhenSessionIsAlreadyRevoked()
        {
            var session = CreateValidSession();

            session.Revoke();

            var result = session.Revoke();

            result.ShouldFailWith(SessionErrors.SessionAlreadyRevoked());
        }

        [TestMethod]
        public void IsActive_ShouldReturnTrue_WhenSessionIsActive()
        {
            var session = CreateValidSession();

            Assert.IsTrue(session.IsActive());
        }

        [TestMethod]
        public void IsActive_ShouldReturnFalse_WhenSessionIsRevoked()
        {
            var session = CreateValidSession();

            session.Revoke();

            Assert.IsFalse(session.IsActive());
        }

        [TestMethod]
        public void IsRevoked_ShouldReturnTrue_WhenSessionIsRevoked()
        {
            var session = CreateValidSession();

            session.Revoke();

            Assert.IsTrue(session.IsRevoked());
        }

        [TestMethod]
        public void IsExpired_ShouldReturnFalse_WhenSessionHasNotExpired()
        {
            var session = CreateValidSession();

            Assert.IsFalse(session.IsExpired());
        }

        private static Session CreateValidSession()
        {
            var result = Session.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                ValidRefreshTokenHash(),
                DateTime.UtcNow.AddDays(30),
                ValidIPAddress(),
                ValidUserAgent());

            result.ShouldSucceed();

            return result.Value;
        }

        private static RefreshTokenHash ValidRefreshTokenHash()
        {
            return RefreshTokenHash
                .Create("refresh_token_hash")
                .Value;
        }

        private static IPAddress ValidIPAddress()
        {
            return IPAddress
                .Create("127.0.0.1")
                .Value;
        }

        private static UserAgent ValidUserAgent()
        {
            return UserAgent
                .Create("Mozilla/5.0")
                .Value;
        }
    }
}
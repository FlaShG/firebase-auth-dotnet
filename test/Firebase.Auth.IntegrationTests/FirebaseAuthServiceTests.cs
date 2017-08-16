using Firebase.Auth.Payloads;
using Firebase.Auth.Payloads.Firebase.Auth.Payloads;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Firebase.Auth.Tests
{
    public class FirebaseAuthServiceTests
    {
        private readonly string webApiKey;
        private readonly string knownValidEmail;
        private readonly string knownValidPassword;
        private readonly string knownDisabledEmail;
        private readonly string knownDisabledPassword;

        public FirebaseAuthServiceTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("secrets.json")
                .Build();

            webApiKey = config["firebaseWebApiKey"];
            knownValidEmail = config["knownValidEmail"];
            knownValidPassword = config["knownVaidPassword"];
            knownDisabledEmail = config["knownDisabledEmail"];
            knownDisabledPassword = config["knownDisabledPassword"];
        }

        #region SignUpNewUser

        private async Task<SignUpNewUserResponse> SignUpNewUser_ValidCredentials()
        {
            using (var service = CreateService())
            {
                var request = new SignUpNewUserRequest()
                {
                    Email = GenerateValidEmail(),
                    Password = "testasdf32t23t23t1234"
                };

                return await service.SignUpNewUser(request);
            }
        }

        [Fact]
        public async Task SignUpNewUser_ValidCredentials_ReturnsIdToken()
        {
            var response = await SignUpNewUser_ValidCredentials();
            Assert.NotNull(response.IdToken);
            Assert.NotEmpty(response.IdToken);
        }
        [Fact]
        public async Task SignUpNewUser_ValidCredentials_ReturnsEmail()
        {
            var response = await SignUpNewUser_ValidCredentials();
            Assert.NotNull(response.Email);
            Assert.NotEmpty(response.Email);
        }

        [Fact]
        public async Task SignUpNewUser_ValidCredentials_ReturnsRefreshToken()
        {
            var response = await SignUpNewUser_ValidCredentials();
            Assert.NotNull(response.RefreshToken);
            Assert.NotEmpty(response.RefreshToken);
        }

        [Fact]
        public async Task SignUpNewUser_ValidCredentials_ReturnsExpiresIn()
        {
            var response = await SignUpNewUser_ValidCredentials();
            Assert.True(response.ExpiresIn > 0);
        }

        [Fact]
        public async Task SignUpNewUser_ValidCredentials_ReturnsLocalId()
        {
            var response = await SignUpNewUser_ValidCredentials();
            Assert.NotNull(response.LocalId);
            Assert.NotEmpty(response.LocalId);
        }

        [Fact]
        public async Task SignUpNewUser_ExistingEmail_ThrowsEmailExists()
        {
            using (var service = CreateService())
            {
                var request = new SignUpNewUserRequest()
                {
                    Email = knownValidEmail,
                    Password = "anyoldpassword"
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.SignUpNewUser(request));
                Assert.Equal(FirebaseAuthMessageType.EmailExists, exception.Error?.MessageType);
            }
        }

        [Fact]
        public async Task SignUpNewUser_WeakPassword_ThrowsWeakPassword()
        {
            using (var service = CreateService())
            {
                var request = new SignUpNewUserRequest()
                {
                    Email = GenerateValidEmail(),
                    Password = "12345"
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.SignUpNewUser(request));
                Assert.Equal(FirebaseAuthMessageType.WeakPassword, exception.Error?.MessageType);
            }
        }

        [Fact]
        public async Task SignUpNewUser_InvalidEmail_ThrowsInvalidEmail()
        {
            using (var service = CreateService())
            {
                var request = new SignUpNewUserRequest()
                {
                    Email = "invalidemail",
                    Password = "validpassword"
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.SignUpNewUser(request));
                Assert.Equal(FirebaseAuthMessageType.InvalidEmail, exception.Error?.MessageType);
            }
        }

        [Fact]
        public async Task SignUpNewUser_NoPassword_ThrowsMissingPassword()
        {
            using (var service = CreateService())
            {
                var request = new SignUpNewUserRequest()
                {
                    Email = "valid@valid.com",
                    Password = ""
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.SignUpNewUser(request));
                Assert.Equal(FirebaseAuthMessageType.MissingPassword, exception.Error?.MessageType);
            }
        }

        [Fact]
        public async Task SignUpNewUser_NoEmail_ThrowsMissingEmail()
        {
            using (var service = CreateService())
            {
                var request = new SignUpNewUserRequest()
                {
                    Email = "",
                    Password = "asdfasdfasdfasdf"
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.SignUpNewUser(request));
                Assert.Equal(FirebaseAuthMessageType.MissingEmail, exception.Error?.MessageType);
            }
        }

        #endregion

        #region VerifyPassword

        private async Task<VerifyPasswordResponse> VerifyPassword_ValidCredentials()
        {
            using (var service = CreateService())
            {
                var request = new VerifyPasswordRequest()
                {
                    Email = knownValidEmail,
                    Password = knownValidPassword
                };

                return await service.VerifyPassword(request);
            }
        }

        [Fact]
        public async Task VerifyPassword_ValidCredentials_ReturnsIdToken()
        {
            var response = await VerifyPassword_ValidCredentials();
            Assert.NotNull(response.IdToken);
            Assert.NotEmpty(response.IdToken);
        }

        [Fact]
        public async Task VerifyPassword_ValidCredentials_ReturnsEmail()
        {
            var response = await VerifyPassword_ValidCredentials();
            Assert.NotNull(response.Email);
            Assert.NotEmpty(response.Email);
        }

        [Fact]
        public async Task VerifyPassword_ValidCredentials_ReturnsRefreshToken()
        {
            var response = await VerifyPassword_ValidCredentials();
            Assert.NotNull(response.RefreshToken);
            Assert.NotEmpty(response.RefreshToken);
        }

        [Fact]
        public async Task VerifyPassword_ValidCredentials_ReturnsExpiresIn()
        {
            var response = await VerifyPassword_ValidCredentials();
            Assert.True(response.ExpiresIn > 0);
        }

        [Fact]
        public async Task VerifyPassword_ValidCredentials_ReturnsLocalId()
        {
            var response = await VerifyPassword_ValidCredentials();
            Assert.NotNull(response.LocalId);
            Assert.NotEmpty(response.LocalId);
        }

        [Fact]
        public async Task VerifyPassword_ValidCredentials_ReturnsRegisteredTrue()
        {
            var response = await VerifyPassword_ValidCredentials();
            Assert.True(response.Registered);
        }

        [Fact]
        public async Task VerifyPassword_RandomEmail_ThrowsEmailNotFound()
        {
            using (var service = CreateService())
            {
                var request = new VerifyPasswordRequest()
                {
                    Email = GenerateValidEmail(),
                    Password = "anyoldpassword"
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.VerifyPassword(request));
                Assert.Equal(FirebaseAuthMessageType.EmailNotFound, exception.Error?.MessageType);
            }
        }

        [Fact]
        public async Task VerifyPassword_WrongPassword_ThrowsInvalidPassword()
        {
            using (var service = CreateService())
            {
                var request = new VerifyPasswordRequest()
                {
                    Email = knownValidEmail,
                    Password = "1234588272727272918*(*D"
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.VerifyPassword(request));
                Assert.Equal(FirebaseAuthMessageType.InvalidPassword, exception.Error?.MessageType);
            }
        }

        [Fact]
        public async Task VerifyPassword_InvalidEmail_ThrowsInvalidEmail()
        {
            using (var service = CreateService())
            {
                var request = new VerifyPasswordRequest()
                {
                    Email = "invalidemail",
                    Password = "validpassword"
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.VerifyPassword(request));
                Assert.Equal(FirebaseAuthMessageType.InvalidEmail, exception.Error?.MessageType);
            }
        }

        [Fact]
        public async Task VerifyPassword_NoPassword_ThrowsMissingPassword()
        {
            using (var service = CreateService())
            {
                var request = new VerifyPasswordRequest()
                {
                    Email = "valid@valid.com",
                    Password = ""
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.VerifyPassword(request));
                Assert.Equal(FirebaseAuthMessageType.MissingPassword, exception.Error?.MessageType);
            }
        }

        [Fact]
        public async Task VerifyPassword_NoEmail_ThrowsInvalidEmail()
        {
            using (var service = CreateService())
            {
                var request = new VerifyPasswordRequest()
                {
                    Email = "",
                    Password = "asdfasdfasdfasdf"
                };

                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.VerifyPassword(request));
                Assert.Equal(FirebaseAuthMessageType.InvalidEmail, exception.Error?.MessageType);
            }
        }

        [Fact]
        public async Task VerifyPassword_DisabledCredentials_ThrowsUserDisabled()
        {
            using (var service = CreateService())
            {
                var request = new VerifyPasswordRequest()
                {
                    Email = knownDisabledEmail,
                    Password = knownDisabledPassword
                };
                //NOTE: Make sure the user is disabled!
                var exception = await Assert.ThrowsAsync<FirebaseAuthException>(async () => await service.VerifyPassword(request));
                Assert.Equal(FirebaseAuthMessageType.UserDisabled, exception.Error?.MessageType);
            }
        }

        #endregion

        #region Helpers

        public static string GenerateValidEmail()
        {
            return $"{DateTime.UtcNow.Ticks}@validdomain.com";
        }

        public FirebaseAuthService CreateService()
        {
            return new FirebaseAuthService(new FirebaseAuthOptions(webApiKey));
        }

        #endregion
    }
}
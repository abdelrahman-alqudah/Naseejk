//using Google.Apis.Auth;
//using Microsoft.Extensions.Logging;
//using Moq;
//using Naseejk.core.Common;
//using Naseejk.core.DTOs;
//using Naseejk.core.Entities;
//using Naseejk.core.Interfaces;
//using Naseejk.core.Repositories;
//using Naseejk.core.Specifications;
//using Naseejk.Infrastructure.Services;
//namespace Naseejk.Tests.Services_Tests
//{
//    public class AccountServiceTests
//    {
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//        private readonly Mock<IGoogleAuthService> _googleAuthMock;
//        private readonly Mock<ISnowflakeIdGenerator> _idGeneratorMock;
//        private readonly Mock<ILogger<AccountService>> _loggerMock;
//        private readonly AccountService _service;

//        public AccountServiceTests()
//        {
//            _unitOfWorkMock = new Mock<IUnitOfWork>();
//            _googleAuthMock = new Mock<IGoogleAuthService>();
//            _idGeneratorMock = new Mock<ISnowflakeIdGenerator>();
//            _loggerMock = new Mock<ILogger<AccountService>>();

//            _service = new AccountService(
//                _unitOfWorkMock.Object,
//                _loggerMock.Object,
//                _googleAuthMock.Object,
//                _idGeneratorMock.Object);
//        }

//        [Fact]
//        public async Task ProvisionUserAsync_WhenUserDoesNotExist_ShouldCreateNewUser()
//        {
//            var dto = new UserProvisioningDto
//            {
//                IdToken = "valid_id_token",
//                Username = "testuser",
//                Email = "test@gmail.com",
//                Provider = "Google",
//                ProviderKey = "google_provider_key",
//                PhoneNumber = "1234567890"
//            };

//            var googlePayload = new GoogleJsonWebSignature.Payload
//            {
//                Email = dto.Email,
//                Name = dto.Username,
//                Subject = dto.ProviderKey
//            };

//            _googleAuthMock.Setup(x => x.VerifyTokenAsync(dto.IdToken))
//                       .ReturnsAsync(googlePayload);

//            _unitOfWorkMock.Setup(x => x.Repository<User>().GetEntityWithSpec(It.IsAny<ISpecification<User>>()))
//                       .ReturnsAsync((User)null!);

//            var expectedId = 40371798545534976L;
//            _idGeneratorMock.Setup(x => x.CreateId()).Returns(expectedId);

//            var result = await _service.ProvisionUserAsync(dto);

//            Assert.True(result.Succeeded);
//            Assert.Equal(expectedId, result.Data!.Id);
//            Assert.Equal("test@gmail.com", result.Data!.Email);

//            _unitOfWorkMock.Verify(x => x.Repository<User>().AddAsync(It.IsAny<User>()), Times.Once);
//            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Once);
//        }
//        [Fact]

//        public async Task ProvisionUserAsync_WhenUserExist_ShouldUpdate()
//        {
//            var dto = new UserProvisioningDto
//            {
//                IdToken = "valid_id_token",
//                Username = "new_username",
//                Email = "test@gmail.com",
//                Provider = "Google",
//                ProviderKey = "google_provider_key",
//                PhoneNumber = "1234567890"
//            };

//            var googlePayload = new GoogleJsonWebSignature.Payload
//            {
//                Email = dto.Email,
//                Name = dto.Username,
//                Subject = dto.ProviderKey
//            };

//            _googleAuthMock.Setup(x => x.VerifyTokenAsync(dto.IdToken))
//                       .ReturnsAsync(googlePayload);

//            var existingUser = new User
//            {
//                Id = 40371798545534976L,
//                Email = dto.Email,
//                Username = "old_username",
//                IdentityId = dto.ProviderKey
//            };

//            _unitOfWorkMock.Setup(x => x.Repository<User>().GetEntityWithSpec(It.IsAny<ISpecification<User>>()))
//                       .ReturnsAsync(existingUser);

//            var result = await _service.ProvisionUserAsync(dto);

//            Assert.True(result.Succeeded);
//            Assert.Equal(existingUser.Id, result.Data!.Id);
//            Assert.Equal("new_username", result.Data.Username);

//            _unitOfWorkMock.Verify(x => x.Repository<User>().AddAsync(It.IsAny<User>()), Times.Never);
//            _unitOfWorkMock.Verify(x => x.CommitAsync(), Times.Never);
//            _unitOfWorkMock.Verify(x => x.CompleteAsync(), Times.Once);

//        }

//        [Fact]
//        public async Task ProvisionUserAsync_WhenGoogleTokenIsInvalid_ShouldThrowUnauthorizedAccessException()
//        {
//            var dto = new UserProvisioningDto
//            {
//                IdToken = "invalid_id_token",
//                Username = "testuser",
//                Email = "test@gmail.com",
//                Provider = "Google",
//                ProviderKey = "google_provider_key",
//                PhoneNumber = "1234567890"
//            };
//            var googlePayLoad = new GoogleJsonWebSignature.Payload
//            {
//                Email = dto.Email,
//                Name = dto.Username,
//                Subject = dto.ProviderKey
//            };

//            _googleAuthMock.Setup(x => x.VerifyTokenAsync(dto.IdToken))
//                       .ReturnsAsync((GoogleJsonWebSignature.Payload)null!);

//            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
//                 _service.ProvisionUserAsync(dto));

//            Assert.Equal("Invalid Google Token", exception.Message);
//            _unitOfWorkMock.Verify(x => x.Repository<User>().GetEntityWithSpec(It.IsAny<ISpecification<User>>()), Times.Never);

//        }
//    }
//}

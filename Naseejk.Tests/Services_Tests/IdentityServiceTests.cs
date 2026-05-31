//using Moq;
//using Naseejk.core.Interfaces;
//using Naseejk.core.Repositories;
//using Naseejk.core.Specifications;
//using Naseejk.core.Entities;
//using Naseejk.Infrastructure.Repositories;
//using Naseejk.Infrastructure.Services;
//using Naseejk.core.DTOs;

//public class IdentityServiceTests
//{
//    private readonly Mock<IGoogleAuthService> _googleAuthMock;
//    private readonly Mock<IAccountService> _accountServiceMock;
//    private readonly Mock<ITokenService> _tokenServiceMock;
//    private readonly Mock<IUnitOfWork> _uowMock;
//    private readonly IdentityService _service;

//    public IdentityServiceTests()
//    {
//        _googleAuthMock = new Mock<IGoogleAuthService>();
//        _accountServiceMock = new Mock<IAccountService>();
//        _tokenServiceMock = new Mock<ITokenService>();
//        _uowMock = new Mock<IUnitOfWork>();

//        //_service = new IdentityService(
//            _googleAuthMock.Object,
//            _accountServiceMock.Object,
//            _tokenServiceMock.Object,
//            _uowMock.Object);
//    }

//    [Fact]
//    public async Task LoginWithGoogleAsync_WhenAppNotRegistered_ShouldReturnFailure()
//    {
//        // 1. Arrange
//        var googleIdToken = "valid_id_token_but_unregistered_app";
//        var clientId = "unregistered_client_id";

//        var googlePayload = new Google.Apis.Auth.GoogleJsonWebSignature.Payload
//        {
//            Audience = clientId,
//            Email = "test@gmail.com",
//            Name = "Test User",
//            Subject = "google_sub_123"
//        };

//        _googleAuthMock.Setup(x => x.VerifyTokenAsync(googleIdToken))
//                       .ReturnsAsync(googlePayload);

//        _uowMock.Setup(x => x.Repository<Application>().GetEntityWithSpec(It.IsAny<ISpecification<Application>>()))
//                .ReturnsAsync((Application)null!);

//        // 2. Act
//        var result = await _service.LoginWithGoogleAsync(googleIdToken);

//        // 3. Assert
//        Assert.False(result.Succeeded);
//        Assert.Contains("This application is not registered in Naseejk.", result.Errors);

//        _accountServiceMock.Verify(x => x.ProvisionUserAsync(It.IsAny<UserProvisioningDto>()), Times.Never);
//        _tokenServiceMock.Verify(x => x.CreateToken(It.IsAny<User>()), Times.Never);
//    }

//    [Fact]
//    public async Task LoginWithGoogleAsync_WhenAppRegistered_ShouldReturnSuccess()
//    {
//        // 1. Arrange
//        var googleIdToken = "valid_google_token";
//        var clientId = "registered_client_id";
//        var fakeToken = "eyJuYXNlZWprIjp0cnVlfQ.fake_token";
//        var googlePayload = new Google.Apis.Auth.GoogleJsonWebSignature.Payload
//        {
//            Audience = clientId,
//            Email = "test@gmail.com",
//            Name = "Test User",
//            Subject = "google_sub_123"
//        };
//        var user = new User { Id = 1, Email = googlePayload.Email, Username = googlePayload.Name };

//        // 2. Act

//        _googleAuthMock.Setup(x => x.VerifyTokenAsync(googleIdToken))
//                       .ReturnsAsync(googlePayload);

//        _uowMock.Setup(x => x.Repository<Application>().GetEntityWithSpec(It.IsAny<ISpecification<Application>>()))
//                .ReturnsAsync(new Application { ClientIdentifier = clientId });

//        _accountServiceMock.Setup(x => x.ProvisionUserAsync(It.IsAny<UserProvisioningDto>()))
//                       .ReturnsAsync(Naseejk.core.Common.Result<User>.Success(user));

//        _tokenServiceMock.Setup(x => x.CreateToken(user))
//                         .Returns(fakeToken);
//        var loginResult = await _service.LoginWithGoogleAsync(googleIdToken);

//        // 3. Assert
//        Assert.True(loginResult.Succeeded);
//        Assert.Equal(fakeToken, loginResult.Data!.Token);

//        _accountServiceMock.Verify(x => x.ProvisionUserAsync(It.IsAny<UserProvisioningDto>()), Times.Once);
//        _tokenServiceMock.Verify(x => x.CreateToken(user), Times.Once);
//    }
//}

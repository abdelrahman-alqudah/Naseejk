//using Microsoft.Extensions.Configuration;
//using Moq;
//using Naseejk.core.Entities;
//using Naseejk.Infrastructure.Services;

//namespace Naseejk.Tests.Services_Tests
//{
//    public class TokenServiceTests
//    {
//        private readonly TokenService _tokenService;
//        private readonly Mock<IConfiguration> _configMock;




//        public TokenServiceTests()
//        {
//            _configMock = new Mock<IConfiguration>();

//            _configMock.Setup(x => x["Jwt:Key"]).Returns("Naseejk_Super_Secret_Key_For_Testing_Purpose_1234567890_Longer_Than_64_Chars");
//            _configMock.Setup(x => x["Jwt:Issuer"]).Returns("NaseejkBackend");
//            _configMock.Setup(x => x["Jwt:Audience"]).Returns("NaseejkClients");

//            _tokenService = new TokenService(_configMock.Object);
//        }

//        [Fact]
//        public void Test_TokenService_Should_Return_Valid_Token()
//        {
//            // Arrange
//            var user = new User
//            {
//                Id = 40371798545534976L,
//                Email = "TestUser@gmail.com",
//                UserStatus = "Active",
//                Username = "TestUser",
//                UserAppRoles = new List<UserAppRole>()
//            };

//            user.UserAppRoles.Add(new UserAppRole
//            {
//                Role = new Role { Name = "Admin" }
//            });

//            // Act
//            var token = _tokenService.CreateToken(user);
//            // Assert
//            Assert.NotNull(token);
//            Assert.NotEmpty(token);

//            //var parts = token.Split('.');
//            //Assert.Equal(3, parts.Length);
//        }
//    }
//}

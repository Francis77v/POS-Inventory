using Backend.DTOs.ManageUserDTO;
using Backend.Services.UserService;
using Moq;
using Backend.Repository.UserRepository;
namespace Backend.Tests.Services.UserService;

public class ManageUserServiceTest
{

    [Fact]
    public async Task AddUserService_PasswordMismatch_ReturnsError()
    {
        // ---------- Arrange ----------
        var repoMock = new Mock<IManageUserRepository>();
        var service = new ManageUserService(repoMock.Object);

        var dto = new AddUserDTO
        {
            username = "testuser",
            email = "test@email.com",
            password = "123456",
            confirmPassword = "different",
            firstName = "Test",
            surName = "User"
        };

        // ---------- Act ----------
        var result = await service.AddUserService(dto);

        // ---------- Assert ----------
        Assert.False(result.success);
        Assert.Equal(403, result.StatusCode);
        Assert.Contains("Password mismatched", result.Errors![0]);
    }
}
using Backend.DTOs.ManageUserDTO;
using Backend.Model;
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

    [Fact]
    public async Task GetUserService_UserExists_Returns_User()
    {
        //Arrange
        var repoMock = new Mock<IManageUserRepository>();
        repoMock.Setup(r => r.GetUserRepository())
            .ReturnsAsync(new List<GetUserDTO>()
            {
                new GetUserDTO()
                {
                    Id = "1",
                    firstName = "Francis",
                    middleName = "Joseph",
                    surName = "Flores",
                    username = "francis66617"
                },
                new GetUserDTO()
                {
                    Id = "2",
                    firstName = "Norman",
                    middleName = "Joe",
                    surName = "Flores",
                    username = "norman66617"
                }
                
            });
        var service = new ManageUserService(repoMock.Object);
        
        
        //act
        var result = await service.GetUserService();
        
        //assert
        Assert.Equal("Francis", result.data[0].firstName);
        Assert.Equal(200, result.StatusCode);
    }
}
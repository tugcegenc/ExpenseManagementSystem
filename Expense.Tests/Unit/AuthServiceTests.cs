using System.Text;
using Moq;
using FluentAssertions;
using Expense.Application.Services.Implementations;
using Expense.Application.Services.Interfaces.Infrastucture;
using Expense.Application.Services.Interfaces.Repositories;
using Expense.Domain.Entities;
using Expense.Schema.Requests;
using MockQueryable;

public class AuthServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockTokenService = new Mock<ITokenService>();
        _authService = new AuthService(_mockTokenService.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenUserIsInactive()
    {
        // ---------- Arrange ----------
        var request = new LoginRequest
        {
            UserNameOrEmail = "user@example.com",
            Password = "123456"
        };

        CreatePasswordHash("123456", out string hash, out string salt);

        var user = new User
        {
            Id = 1,
            Email = "user@example.com",
            UserName = "user",
            PasswordHash = hash,
            PasswordSalt = salt,
            IsActive = false 
        };

        var userList = new List<User> { user }.AsQueryable().BuildMock();
        var mockUserRepo = new Mock<IGenericRepository<User>>();
        mockUserRepo.Setup(x => x.AsQueryable()).Returns(userList);

        var mockRefreshTokenRepo = new Mock<IGenericRepository<RefreshToken>>();
        mockRefreshTokenRepo.Setup(x => x.AsQueryable()).Returns(new List<RefreshToken>().AsQueryable().BuildMock());

        _mockUnitOfWork.Setup(x => x.GetRepository<User>()).Returns(mockUserRepo.Object);
        _mockUnitOfWork.Setup(x => x.GetRepository<RefreshToken>()).Returns(mockRefreshTokenRepo.Object);

        // ---------- Act ----------
        var result = await _authService.LoginAsync(request);

        // ---------- Assert ----------
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("Invalid credentials");
    }

    private void CreatePasswordHash(string password, out string hash, out string salt)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA256();
        salt = Convert.ToBase64String(hmac.Key);
        hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
}

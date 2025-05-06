using AutoMapper;
using Expense.Application.Services.Implementations;
using Expense.Application.Services.Interfaces.Infrastucture;
using Expense.Application.Services.Interfaces.Sessions;
using Expense.Domain.Entities;
using Expense.Domain.Enums;
using Expense.Schema.Requests;
using Expense.Schema.Responses;
using Microsoft.AspNetCore.Http;
using Moq;
using MockQueryable.Moq;
using Xunit;
using MockQueryable;
using Expense.Application.Services.Interfaces.Services;

namespace Expense.Tests.Unit;

public class ExpenseClaimServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IFileService> _mockFileService;
    private readonly Mock<IAppSession> _mockAppSession;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    private readonly ExpenseClaimService _service;

    public ExpenseClaimServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockFileService = new Mock<IFileService>();
        _mockAppSession = new Mock<IAppSession>();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        _service = new ExpenseClaimService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockAppSession.Object,
            _mockFileService.Object,
            _mockHttpContextAccessor.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsSuccess()
    {
        // ---------- Arrange ----------

        var request = new ExpenseClaimRequest
        {
            Amount = 100,
            Description = "Ulaşım",
            ExpenseCategoryId = 1,
            Location = "İstanbul",
            PaymentMethod = PaymentMethod.CreditCard
        };

        var entity = new ExpenseClaim { Id = 1, Amount = 100, UserId = 1 };
        var response = new ExpenseClaimResponse { Amount = 100 };

        _mockAppSession.Setup(a => a.UserId).Returns(1);

        _mockMapper.Setup(m => m.Map<ExpenseClaim>(request)).Returns(entity);
        _mockMapper.Setup(m => m.Map<ExpenseClaimResponse>(It.IsAny<ExpenseClaim>())).Returns(response);

        var claimList = new List<ExpenseClaim>().AsQueryable();
        var mockClaimQueryable = claimList.BuildMock().BuildMockDbSet();
        _mockUnitOfWork.Setup(u => u.GetRepository<ExpenseClaim>().AsQueryable())
            .Returns(mockClaimQueryable.Object);

        _mockUnitOfWork.Setup(u => u.GetRepository<ExpenseClaim>().AddAsync(It.IsAny<ExpenseClaim>()))
            .Returns(Task.CompletedTask);

        _mockUnitOfWork.Setup(u => u.CompleteAsync())
            .ReturnsAsync(1);

        // ---------- Arrange ----------
        var result = await _service.CreateAsync(request);

        // ---------- Assert ----------
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(100, result.Data.Amount);
    }
}

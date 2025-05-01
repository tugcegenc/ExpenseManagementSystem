using AutoMapper;
using Expense.Application.Services.Interfaces;
using Expense.Common.Helpers;
using Expense.Domain.Entities;
using Expense.Domain.Interfaces;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAppSession _appSession;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IAppSession appSession)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _appSession = appSession;
    }
    public async Task<List<UserResponse>> GetAllAsync()
    {
        var users = await _unitOfWork.GetRepository<User>().GetAllAsync();
        var activeUsers = users.Where(x => x.IsActive).ToList();
        
        return _mapper.Map<List<UserResponse>>(activeUsers);
    }
    public async Task<UserResponse> GetByIdAsync(long id)
    {
        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
        return _mapper.Map<UserResponse>(user);
    }
    public async Task<UserResponse> CreateAsync(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);

        var password = PasswordGenerator.GeneratePassword(8);
        var salt = PasswordGenerator.GeneratePassword(30);
        var passwordHash = PasswordGenerator.CreateSHA256(password, salt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;
        user.CreatedAt = DateTime.UtcNow;
        user.CreatedBy = _appSession.UserName ?? "Anonymous";

        await _unitOfWork.GetRepository<User>().AddAsync(user);
        await _unitOfWork.CompleteAsync();

    var log = $"Username: {user.UserName}, Password: {password}";
    await File.AppendAllTextAsync("GeneratedPasswords.txt", log + Environment.NewLine);
        var response = _mapper.Map<UserResponse>(user);
        return response;
           
    }
    public async Task UpdateAsync(long id, UpdateUserRequest request)
    {
        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found");

        _mapper.Map(request, user);
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _appSession.UserName ?? "Anonymous";

        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(id);
        if (user == null)
            throw new Exception("User not found");

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _appSession.UserName ?? "Anonymous";

        await _unitOfWork.CompleteAsync();
    }

}

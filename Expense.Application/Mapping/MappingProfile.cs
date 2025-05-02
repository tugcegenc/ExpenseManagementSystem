using AutoMapper;
using Expense.Domain.Entities;
using Expense.Domain.Enums;
using Expense.Schema.Requests;
using Expense.Schema.Responses;

namespace Expense.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
        CreateMap<CreateUserRequest, User>();
        CreateMap<UpdateUserRequest, User>();

        CreateMap<ExpenseCategory, ExpenseCategoryResponse>();
        CreateMap<ExpenseCategoryRequest, ExpenseCategory>();

        CreateMap<ExpenseClaim, ExpenseClaimResponse>()
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ExpenseCategory.Name));

        CreateMap<ExpenseClaimRequest, ExpenseClaim>()
            .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => ExpenseStatus.Pending));
        
        CreateMap<ExpenseClaimFilterRequest, ExpenseClaim>();
         
        CreateMap<EftSimulationLog, EftSimulationLogResponse>()
            .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => $"{src.ExpenseClaim.User.FirstName} {src.ExpenseClaim.User.LastName}"));
       
            
    }
}

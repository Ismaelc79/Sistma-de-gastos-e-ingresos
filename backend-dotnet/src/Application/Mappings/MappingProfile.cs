using AutoMapper;
using Application.DTOs.Auth;
using Application.DTOs.Categories;
using Application.DTOs.Transactions;
using Application.DTOs.Reports;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User Mappings
        CreateMap<User, UserDto>().ReverseMap();

        // Category Mappings
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CreateCategoryRequest, Category>();

        // Transaction Mappings
        CreateMap<Transaction, TransactionDto>().ReverseMap();
        CreateMap<CreateTransactionRequest, Transaction>();

        // RefreshToken Mappings
        CreateMap<RefreshToken, RefreshTokenDto>().ReverseMap();

        // UserVerification Mappings
        CreateMap<UserVerification, UserVerificationDto>().ReverseMap();

        // Report Mappings
        CreateMap<SummaryReportDto, SummaryReportDto>();
        CreateMap<CategorySummary, CategorySummary>();
    }
}

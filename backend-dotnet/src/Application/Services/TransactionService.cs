using Application.DTOs.Transactions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<TransactionDto> CreateTransactionAsync(CreateTransactionRequest request)
    {
        var transaction = _mapper.Map<Transaction>(request);

        var createdTransaction = await _unitOfWork.Transaction.AddAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TransactionDto>(createdTransaction);
    }

    public async Task<TransactionDto?> GetTransactionByIdAsync(int id)
    {
        var transaction = await _unitOfWork.Transaction.GetByIdAsync(id);
        return transaction != null ? _mapper.Map<TransactionDto>(transaction) : null;
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAsync(Ulid userId)
    {
        var transactions = await _unitOfWork.Transaction.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByUserIdAndDateRangeAsync(Ulid userId, DateTime startDate, DateTime endDate)
    {
        var transactions = await _unitOfWork.Transaction.GetByUserIdAndDateRangeAsync(userId, startDate, endDate);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsByCategoryIdAsync(int categoryId)
    {
        var transactions = await _unitOfWork.Transaction.GetByCategoryIdAsync(categoryId);
        return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto> UpdateTransactionAsync(int id, CreateTransactionRequest request)
    {
        var transaction = await _unitOfWork.Transaction.GetByIdAsync(id);
        if (transaction == null)
        {
            throw new KeyNotFoundException($"Transacción con ID {id} no encontrada");
        }

        transaction.Update( categoryId: request.CategoryId, name: request.Name, description: request.Description, amount: request.Amount );

        var updatedTransaction = await _unitOfWork.Transaction.UpdateAsync(transaction);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<TransactionDto>(updatedTransaction);
    }

    public async Task<bool> DeleteTransactionAsync(int id)
    {
        return await _unitOfWork.Transaction.DeleteAsync(id);
    }
}

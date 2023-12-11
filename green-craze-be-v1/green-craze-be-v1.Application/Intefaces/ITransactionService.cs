using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Statistic;
using green_craze_be_v1.Application.Model.Transaction;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface ITransactionService
    {
        Task<PaginatedResult<TransactionDto>> GetListTransaction(GetTransactionPagingRequest request);

        Task<TransactionDto> GetTransaction(long id);

        Task<List<StatisticTransactionResponse>> GetTop5TransactionLatest();
    }
}
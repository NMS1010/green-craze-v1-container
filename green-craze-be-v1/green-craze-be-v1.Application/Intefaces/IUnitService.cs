using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Unit;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IUnitService
    {
        Task<PaginatedResult<UnitDto>> GetListUnit(GetUnitPagingRequest request);

        Task<UnitDto> GetUnit(long id);

        Task<long> CreateUnit(CreateUnitRequest request);

        Task<bool> UpdateUnit(long id, UpdateUnitRequest request);

        Task<bool> DeleteUnit(long id);

        Task<bool> DeleteListUnit(List<long> ids);
    }
}
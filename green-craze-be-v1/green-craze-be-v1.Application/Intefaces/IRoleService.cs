using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IRoleService
    {
        Task<PaginatedResult<RoleDto>> GetListRole(GetRolePagingRequest request);

        Task<RoleDto> GetRole(string roleId);
    }
}
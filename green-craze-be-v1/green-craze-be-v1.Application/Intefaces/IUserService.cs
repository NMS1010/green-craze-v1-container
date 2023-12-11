using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace green_craze_be_v1.Application.Intefaces
{
    public interface IUserService
    {
        Task<string> CreateStaff(CreateStaffRequest request);

        Task<bool> UpdateUser(UpdateUserRequest request);

        Task<bool> UpdateStaff(UpdateStaffRequest request);

        Task<PaginatedResult<UserDto>> GetListUser(GetUserPagingRequest request);

        Task<PaginatedResult<StaffDto>> GetListStaff(GetUserPagingRequest request);

        Task<UserDto> GetUser(string userId);

        Task<StaffDto> GetStaff(long staffId);

        Task<bool> ToggleUserStatus(string userId);

        Task<bool> ToggleStaffStatus(long staffId);

        Task<bool> DisableListUserStatus(List<string> userIds);

        Task<bool> DisableListStaffStatus(List<long> staffIds);

        Task<bool> ChangePassword(ChangePasswordRequest request);
    }
}
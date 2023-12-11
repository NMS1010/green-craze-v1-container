using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Address;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ICurrentUserService _currentUserService;

        public AddressesController(IAddressService addressService, ICurrentUserService currentUserService)
        {
            _addressService = addressService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressRequest request)
        {
            request.UserId = _currentUserService.UserId;

            long addressId = await _addressService.CreateAddress(request);

            var url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/addresses/{addressId}";

            return Created(url, new APIResponse<object>(new { id = addressId }, StatusCodes.Status201Created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress([FromRoute] long id, [FromBody] UpdateAddressRequest request)
        {
            request.UserId = _currentUserService.UserId;
            request.Id = id;

            var res = await _addressService.UpdateAddress(request);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress([FromRoute] long id)
        {
            var res = await _addressService.GetAddress(id, _currentUserService.UserId);

            return Ok(new APIResponse<AddressDto>(res, StatusCodes.Status200OK));
        }

        [HttpGet("default")]
        public async Task<IActionResult> GetDefaultAddress()
        {
            var res = await _addressService.GetDefaultAddress(_currentUserService.UserId);

            return Ok(new APIResponse<AddressDto>(res, StatusCodes.Status200OK));
        }

        [HttpGet]
        public async Task<IActionResult> GetListAddress([FromQuery] GetAddressPagingRequest request)
        {
            request.UserId = _currentUserService.UserId;

            var res = await _addressService.GetListAddress(request);

            return Ok(new APIResponse<PaginatedResult<AddressDto>>(res, StatusCodes.Status200OK));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress([FromRoute] long id)
        {
            var res = await _addressService.DeleteAddress(id, _currentUserService.UserId);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status200OK));
        }

        [HttpPut("set-default/{id}")]
        public async Task<IActionResult> SetDefaultAddress([FromRoute] long id)
        {
            var res = await _addressService.SetAddressDefault(id, _currentUserService.UserId);

            return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
        }

        [HttpGet("p")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProvince()
        {
            var res = await _addressService.GetListProvince();

            return Ok(new APIResponse<List<ProvinceDto>>(res, StatusCodes.Status200OK));
        }

        [HttpGet("p/{provinceId}/d")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListDistrictByProvince([FromRoute] long provinceId)
        {
            var res = await _addressService.GetListDistrictByProvince(provinceId);

            return Ok(new APIResponse<List<DistrictDto>>(res, StatusCodes.Status200OK));
        }

        [HttpGet("p/d/{districtId}/w")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListWardByDistrict([FromRoute] long districtId)
        {
            var res = await _addressService.GetListWardByDistrict(districtId);

            return Ok(new APIResponse<List<WardDto>>(res, StatusCodes.Status200OK));
        }
    }
}
using AutoMapper;
using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Address;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.Address;
using green_craze_be_v1.Domain.Entities;

namespace green_craze_be_v1.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<long> CreateAddress(CreateAddressRequest request)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                var user = await _unitOfWork.Repository<AppUser>().GetById(request.UserId)
                    ?? throw new NotFoundException("Cannot find current user");

                var province = await _unitOfWork.Repository<Province>().GetById(request.ProvinceId)
                    ?? throw new InvalidRequestException("Unexpected provinceId");

                var district = await _unitOfWork.Repository<District>().GetEntityWithSpec(new DistrictSpecification(request.DistrictId))
                    ?? throw new InvalidRequestException("Unexpected districtId");

                var ward = await _unitOfWork.Repository<Ward>().GetEntityWithSpec(new WardSpecification(request.WardId))
                    ?? throw new InvalidRequestException("Unexpected wardId");

                if (ward.District.Id != district.Id || district.Province.Id != province.Id)
                    throw new InvalidRequestException("Cannot identify combined address, may be unexpected provinceId, districtId, wardId");

                var address = _mapper.Map<Address>(request);
                address.Province = province;
                address.District = district;
                address.Ward = ward;
                address.User = user;
                address.IsDefault = true;

                var addresses = await _unitOfWork.Repository<Address>().ListAsync(new AddressSpecification(user.Id, isDefault: true));

                foreach (var a in addresses)
                {
                    a.IsDefault = false;
                    _unitOfWork.Repository<Address>().Update(a);
                }

                await _unitOfWork.Repository<Address>().Insert(address);

                var isSuccess = await _unitOfWork.Save() > 0;
                await _unitOfWork.Commit();

                if (!isSuccess)
                {
                    throw new Exception("Cannot handle to insert address for user, an error has occured");
                }

                return address.Id;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteAddress(long id, string userId)
        {
            var address = await _unitOfWork.Repository<Address>().GetEntityWithSpec(new AddressSpecification(userId, id))
                ?? throw new InvalidRequestException("Unexpected addressId");
            if (address.IsDefault)
                throw new InvalidRequestException("Cannot handle to delete default address, please set another address to default and try again");

            address.Status = false;

            _unitOfWork.Repository<Address>().Update(address);

            var isSuccess = await _unitOfWork.Save() > 0;

            if (!isSuccess)
            {
                throw new Exception("Cannot handle to delete address, an error has occured");
            }

            return true;
        }

        public async Task<AddressDto> GetAddress(long id, string userId)
        {
            var address = await _unitOfWork.Repository<Address>().GetEntityWithSpec(new AddressSpecification(userId, id))
                ?? throw new InvalidRequestException("Unexpected addressId");

            return _mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto> GetDefaultAddress(string userId)
        {
            var address = await _unitOfWork.Repository<Address>().GetEntityWithSpec(new AddressSpecification(userId, true));
            if (address == null)
                return null;
            return _mapper.Map<AddressDto>(address);
        }

        public async Task<PaginatedResult<AddressDto>> GetListAddress(GetAddressPagingRequest request)
        {
            var spec = new AddressSpecification(request, isPaging: true);
            var countSpec = new AddressSpecification(request);

            var addresses = await _unitOfWork.Repository<Address>().ListAsync(spec);
            var count = await _unitOfWork.Repository<Address>().CountAsync(countSpec);
            //if (request.Status)
            //{
            //    addresses = addresses.Where(x => x.Status == true).ToList();
            //}
            var addressDtos = new List<AddressDto>();
            addresses.ForEach(x => addressDtos.Add(_mapper.Map<AddressDto>(x)));

            return new PaginatedResult<AddressDto>(addressDtos, request.PageIndex, count, request.PageSize);
        }

        public async Task<bool> SetAddressDefault(long id, string userId)
        {
            try
            {
                await _unitOfWork.CreateTransaction();
                var address = await _unitOfWork.Repository<Address>().GetEntityWithSpec(new AddressSpecification(userId, id))
                    ?? throw new InvalidRequestException("Unexpected addressId");

                address.IsDefault = true;

                var addresses = await _unitOfWork.Repository<Address>().ListAsync(new AddressSpecification(userId, isDefault: true));
                foreach (var a in addresses)
                {
                    a.IsDefault = false;
                    _unitOfWork.Repository<Address>().Update(a);
                }

                _unitOfWork.Repository<Address>().Update(address);

                var isSuccess = await _unitOfWork.Save() > 0;

                await _unitOfWork.Commit();

                if (!isSuccess)
                {
                    throw new Exception("Cannot handle to set default address, an error has occured");
                }

                return true;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<bool> UpdateAddress(UpdateAddressRequest request)
        {
            try
            {
                await _unitOfWork.CreateTransaction();

                var address = await _unitOfWork.Repository<Address>().GetEntityWithSpec(new AddressSpecification(request.UserId, request.Id))
                    ?? throw new InvalidRequestException("Unexpected addressId");

                var province = await _unitOfWork.Repository<Province>().GetById(request.ProvinceId)
                    ?? throw new InvalidRequestException("Unexpected provinceId");

                var district = await _unitOfWork.Repository<District>().GetEntityWithSpec(new DistrictSpecification(request.DistrictId))
                    ?? throw new InvalidRequestException("Unexpected districtId");

                var ward = await _unitOfWork.Repository<Ward>().GetEntityWithSpec(new WardSpecification(request.WardId))
                    ?? throw new InvalidRequestException("Unexpected wardId");

                if (ward.District.Id != district.Id || district.Province.Id != province.Id)
                    throw new InvalidRequestException("Cannot identify combined address, may be unexpected provinceId, districtId, wardId");
                var isDefault = address.IsDefault;
                _mapper.Map(request, address);

                address.Province = province;
                address.District = district;
                address.Ward = ward;
                address.IsDefault = isDefault;

                _unitOfWork.Repository<Address>().Update(address);

                var isSuccess = await _unitOfWork.Save() > 0;
                await _unitOfWork.Commit();

                if (!isSuccess)
                {
                    throw new Exception("Cannot handle to update address for current user, an error has occured");
                }

                return true;
            }
            catch
            {
                await _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<List<DistrictDto>> GetListDistrictByProvince(long provinceId)
        {
            var province = await _unitOfWork.Repository<Province>().GetEntityWithSpec(new ProvinceSpecification(provinceId))
                ?? throw new InvalidRequestException("Unexpected provinceId");

            var districtDtos = new List<DistrictDto>();

            province.Districts.ToList().ForEach(x => districtDtos.Add(_mapper.Map<DistrictDto>(x)));

            return districtDtos;
        }

        public async Task<List<ProvinceDto>> GetListProvince()
        {
            var provinces = (await _unitOfWork.Repository<Province>().GetAll()).ToList();

            var provinceDtos = new List<ProvinceDto>();

            provinces.ForEach(x => provinceDtos.Add(_mapper.Map<ProvinceDto>(x)));

            return provinceDtos;
        }

        public async Task<List<WardDto>> GetListWardByDistrict(long districtId)
        {
            var district = await _unitOfWork.Repository<District>().GetEntityWithSpec(new DistrictSpecification(districtId))
                ?? throw new InvalidRequestException("Unexpected districtId");

            var wardDtos = new List<WardDto>();

            district.Wards.ToList().ForEach(x => wardDtos.Add(_mapper.Map<WardDto>(x)));

            return wardDtos;
        }
    }
}
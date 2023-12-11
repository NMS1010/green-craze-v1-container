using AutoMapper;
using green_craze_be_v1.Application.Common.SignalR;
using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.Notification;
using green_craze_be_v1.Application.Model.Paging;
using green_craze_be_v1.Application.Specification.Notification;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace green_craze_be_v1.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<AppHub> _hub;
        private readonly ICurrentUserService _currentUserService;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IHubContext<AppHub> hub, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hub = hub;
            _currentUserService = currentUserService;
        }

        public async Task CreateOrderNotification(CreateNotificationRequest request)
        {
            var user = await _unitOfWork.Repository<AppUser>().GetById(request.UserId)
                ?? throw new Exception("User not login");

            var notification = _mapper.Map<Notification>(request);
            notification.User = user;
            await _unitOfWork.Repository<Notification>().Insert(notification);

            var res = await _unitOfWork.Save() > 0;
            if (res)
            {
                var count = await _unitOfWork.Repository<Notification>().CountAsync(new NotificationSpecification(request.UserId, false));
                await _hub.Clients.Group(user.Id).SendAsync("ReceiveNotification", _mapper.Map<NotificationDto>(notification), count);
            }
        }

        public async Task CreateSaleNotification(CreateNotificationRequest request)
        {
            var users = await _unitOfWork.Repository<AppUser>().GetAll();
            foreach (var user in users)
            {
                var notification = _mapper.Map<Notification>(request);
                notification.User = user;
                await _unitOfWork.Repository<Notification>().Insert(notification);
            }

            var res = await _unitOfWork.Save() > 0;
            if (res)
            {
                foreach (var user in users)
                {
                    var count = await _unitOfWork.Repository<Notification>().CountAsync(new NotificationSpecification(request.UserId, false));
                    await _hub.Clients.Group(user.Id).SendAsync("ReceiveNotification", _mapper.Map<NotificationDto>(_mapper.Map<Notification>(request)), count);
                }
            }
        }

        public async Task<PaginatedResult<NotificationDto>> GetListNotification(GetNotificationPagingRequest request)
        {
            var notifications = await _unitOfWork.Repository<Notification>().ListAsync(new NotificationSpecification(request, isPaging: true));
            var total = await _unitOfWork.Repository<Notification>().CountAsync(new NotificationSpecification(request));

            return new PaginatedResult<NotificationDto>(notifications
                .Select(x => _mapper.Map<NotificationDto>(x)).ToList(),
                request.PageIndex, total, request.PageSize);
        }

        public async Task<bool> UpdateAllNotification(string userId)
        {
            var notifications = await _unitOfWork.Repository<Notification>().ListAsync(new NotificationSpecification(userId));

            notifications.ForEach(x =>
            {
                x.Status = true;
                _unitOfWork.Repository<Notification>().Update(x);
            });

            return await _unitOfWork.Save() > 0;
        }

        public async Task<bool> UpdateNotification(UpdateNotificationRequest request)
        {
            var notification = await _unitOfWork.Repository<Notification>().GetEntityWithSpec(new NotificationSpecification(request.UserId, request.Id))
                ?? throw new Exception("Notification not found");

            notification.Status = true;

            _unitOfWork.Repository<Notification>().Update(notification);

            return await _unitOfWork.Save() > 0;
        }
    }
}
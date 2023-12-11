using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Model.Notification;
using green_craze_be_v1.Application.Model.Paging;

namespace green_craze_be_v1.Application.Intefaces
{
	public interface INotificationService
	{
		Task CreateOrderNotification(CreateNotificationRequest request);

		Task CreateSaleNotification(CreateNotificationRequest request);

		Task<bool> UpdateNotification(UpdateNotificationRequest request);

		Task<bool> UpdateAllNotification(string userId);

		Task<PaginatedResult<NotificationDto>> GetListNotification(GetNotificationPagingRequest request);
	}
}
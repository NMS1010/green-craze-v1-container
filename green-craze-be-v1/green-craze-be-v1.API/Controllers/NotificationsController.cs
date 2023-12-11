using green_craze_be_v1.Application.Dto;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Model.CustomAPI;
using green_craze_be_v1.Application.Model.Notification;
using green_craze_be_v1.Application.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace green_craze_be_v1.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class NotificationsController : ControllerBase
	{
		private readonly INotificationService _notificationService;
		private readonly ICurrentUserService _currentUserService;

		public NotificationsController(INotificationService notificationService, ICurrentUserService currentUserService)
		{
			_notificationService = notificationService;
			_currentUserService = currentUserService;
		}

		[HttpGet]
		public async Task<IActionResult> GetListNotification([FromQuery] GetNotificationPagingRequest request)
		{
			request.UserId = _currentUserService.UserId;
			var notifications = await _notificationService.GetListNotification(request);

			return Ok(new APIResponse<PaginatedResult<NotificationDto>>(notifications, StatusCodes.Status200OK));
		}

		[HttpPatch("{id}")]
		public async Task<IActionResult> UpdateNotification([FromRoute] long id)
		{
			var request = new UpdateNotificationRequest()
			{
				Id = id,
				UserId = _currentUserService.UserId
			};

			var res = await _notificationService.UpdateNotification(request);

			return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
		}

		[HttpPut("all")]
		public async Task<IActionResult> UpdateAllNotification()
		{
			var res = await _notificationService.UpdateAllNotification(_currentUserService.UserId);

			return Ok(new APIResponse<bool>(res, StatusCodes.Status204NoContent));
		}
	}
}
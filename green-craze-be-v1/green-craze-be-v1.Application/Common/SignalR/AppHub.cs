using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Intefaces;
using green_craze_be_v1.Application.Specification.Notification;
using green_craze_be_v1.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace green_craze_be_v1.Application.Common.SignalR
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppHub : Hub
    {
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private static Dictionary<string, int> clientsNotification = new();

        public AppHub(IJwtService jwtService, IUnitOfWork unitOfWork)
        {
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            var accessToken = Context.GetHttpContext().Request.Query["access_token"];
            var userPrincipal = _jwtService.ValidateExpiredJWT(accessToken)
                ?? throw new Exception("Invalid token");

            var userId = (userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value)
                ?? throw new NotFoundException("User not found");

            int count = 0;
            if (clientsNotification.TryGetValue(userId, out count))
                clientsNotification[userId] = count + 1;
            else
                clientsNotification.Add(userId, 1);

            if (clientsNotification[userId] == 1)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, Group.SALES);

            var countNotify = await _unitOfWork.Repository<Notification>().CountAsync(new NotificationSpecification(userId, false));
            await Clients.Group(userId).SendAsync("CountUnreadingNotification", countNotify);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var accessToken = Context.GetHttpContext().Request.Query["access_token"];
            var userPrincipal = _jwtService.ValidateExpiredJWT(accessToken)
                ?? throw new Exception("Invalid token");

            var userId = (userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value)
                ?? throw new NotFoundException("User not found");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Group.SALES);

            clientsNotification.Remove(userId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
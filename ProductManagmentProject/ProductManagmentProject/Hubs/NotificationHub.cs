using Microsoft.AspNetCore.SignalR;

namespace ProductManagmentProject.Hubs
{
    public class NotificationHub : Hub
    {
        // Lưu mapping giữa UserId và ConnectionId
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.GetHttpContext()?.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
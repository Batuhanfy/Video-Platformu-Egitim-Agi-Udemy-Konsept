using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using UdemyEgitimPlatformu.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OnlineUsersHub : Hub
{
    private readonly UserManager<ApplicationUser> _userManager;
    private static readonly HashSet<string> _onlineUsers = new HashSet<string>();

    public OnlineUsersHub(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public override Task OnConnectedAsync()
    {
        _onlineUsers.Add(Context.UserIdentifier);
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _onlineUsers.Remove(Context.UserIdentifier);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendOnlineUsers()
    {
        var onlineUserIds = _onlineUsers.ToList();
        var onlineUsers = new List<OnlineUserDto>();

        foreach (var userId in onlineUserIds)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                onlineUsers.Add(new OnlineUserDto
                {
                    UserId = user.Id,
                    UserName = user.FirstName+" "+user.LastName,
                    ProfileImage = user.ProfilePictureUrl // Profil fotoğrafı yolunu al
                });
            }
        }

        var onlineUsersCount = onlineUsers.Count;
        await Clients.All.SendAsync("ReceiveOnlineUsers", onlineUsers, onlineUsersCount);
    }
}

public class OnlineUserDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string ProfileImage { get; set; }
}

using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
namespace API.SignalR;
public class MessageHub(IMessageRepository messageRepository) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var otherUser = httpContext?.Request.Query["user"];
        if (Context.User == null || string.IsNullOrEmpty(otherUser)) 
            throw new Exception("Cannot join group");
        var groupName = GetGroupName(Context.User.GetUsername(), otherUser);
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        // var group = await AddToGroup(groupName);
        // await Clients.Group(groupName).SendAsync("UpdatedGroup", group);
        var messages = await messageRepository.GetMessageThread(Context.User.GetUsername(), otherUser!);
        // await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // var group = await RemoveFromMessageGroup();
        // await Clients.Group(group.Name).SendAsync("UpdatedGroup", group);
       return base.OnDisconnectedAsync(exception);
    }
    private string GetGroupName(string caller, string? other) 
    {
        var stringCompare = string.CompareOrdinal(caller, other) < 0;
        return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
    }
}
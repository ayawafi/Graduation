using Clinic_DbModel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly clinic_dbContext _dbContext;

    public ChatHub(clinic_dbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SendMessage(string receiverUserId, string message)
    {
        string senderUserId = Context.UserIdentifier;

        await Clients.User(receiverUserId).SendAsync("ReceiveMessage", senderUserId, message);
    }



}






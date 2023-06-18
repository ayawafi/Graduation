using Clinic_DbModel.Models;
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

    //public async Task SendMessage(string receiverUserId, string message)
    //{
    //    string senderUserId = Context.UserIdentifier;

    //    // Broadcast the message to the receiver
    //    await Clients.User(receiverUserId).SendAsync("ReceiveMessage", senderUserId, message);
    //}

    //public async Task GetMessages(string userId)
    //{
    //    var sendMessages = _dbContext.ChatMessages.Include("Doctor.User").Where(x => x.SenderUserId == userId).OrderBy(x => x.SentAt).ToList();
    //    var receivedMessages = _dbContext.ChatMessages.Where(x => x.ReceiverUserId == userId).OrderBy(x => x.SentAt).ToList();
    //}
}






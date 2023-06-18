using Clinic_DbModel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using Clinic_ModelView;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using MySqlX.XDevAPI;
using System.Linq;

namespace Clinic.Controllers
{
    [Authorize]
    [ApiController]
    public class ChatController : BaseController
    {
        private readonly clinic_dbContext _dbContext;
        private readonly IHubContext<ChatHub> _chatHub;
        private readonly IHttpContextAccessor __httpContextAccessor;
        public ChatController(clinic_dbContext dbContext, IHubContext<ChatHub> chatHub, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _dbContext = dbContext;
            _chatHub = chatHub;
            __httpContextAccessor = httpContextAccessor;

        }

        [HttpPost("chat/sendmessage")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageVM messageVM)
        {
            var chatMessage = new ChatMessage
            {
                SenderUserId = _DoctorId,
                ReceiverUserId = messageVM.ReceiverUserId,
                Message = messageVM.Message,
                SentAt = DateTime.UtcNow
            };

            _dbContext.ChatMessages.Add(chatMessage);
            var res = _dbContext.SaveChanges();

            await _chatHub.Clients.User(messageVM.ReceiverUserId).SendAsync("ReceiveMessage", chatMessage);

            await _chatHub.Clients.User(messageVM.ReceiverUserId).SendAsync("ReceiveNotification", "You have a new massage");


            return Ok(res);
        }

    

        [HttpGet("chat/GetListChat")]
        public IActionResult GetListChat()
        {
            var chatMessages = _dbContext.ChatMessages
                .Include(x => x.SenderUser)
                .Include(x => x.ReceiverUser)
                .Where(x => x.SenderUserId == _DoctorId || x.ReceiverUserId == _DoctorId)
                .ToList(); 

            var listofmsg = chatMessages
                .GroupBy(x => x.SenderUserId == _DoctorId ? x.ReceiverUserId : x.SenderUserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    LastMessage = g.OrderByDescending(x => x.SentAt).FirstOrDefault()
                })
                .OrderByDescending(x => x.LastMessage.SentAt)
                .Select(x => new
                {
                    User = new
                    {
                        Image = x.UserId == _DoctorId ? x.LastMessage.SenderUser?.Image : x.LastMessage.ReceiverUser?.Image,
                        Name = x.UserId == _DoctorId ? $"{x.LastMessage.SenderUser?.FirstName} {x.LastMessage.SenderUser?.LastName}" : $"{x.LastMessage.ReceiverUser?.FirstName} {x.LastMessage.ReceiverUser?.LastName}",
                        Id = x.UserId
                    },
                    CreatedDate = x.LastMessage.SentAt,
                    LastMessage = x.LastMessage.Message
                })
                .ToList();

            if (!listofmsg.Any())
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "You don't have any chat",
                    Data = null
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Successfully",
                    Data = listofmsg
                };
                return Ok(response);
            }

        }
        [HttpGet("chat/GetChatMassage")]
        public IActionResult GetChatMassage(string uId)
        {
            var chatMsg = _dbContext.ChatMessages
    .Where(x => (x.SenderUserId == uId && x.ReceiverUserId == _DoctorId) ||
                (x.SenderUserId == _DoctorId && x.ReceiverUserId == uId))
    .Join(_dbContext.Users,
        message => message.SenderUserId,
        sender => sender.Id,
        (message, sender) => new { Message = message, SenderName = sender.FirstName + " " + sender.LastName })
    .Join(_dbContext.Users,
        message => message.Message.ReceiverUserId,
        receiver => receiver.Id,
        (message, receiver) => new
        {
            message.Message.SenderUserId,
            message.Message.ReceiverUserId,
            SenderName = message.SenderName,
            ReceiverName = receiver.FirstName + " " + receiver.LastName,
            Message = message.Message.Message,
            SentAt = message.Message.SentAt
        })
    .ToList()
    .OrderBy(x => x.SentAt.ToString("h:mm:ss tt d/M/yyyy"))
    .Select(x => new
    {
        x.SenderUserId,
        x.ReceiverUserId,
        x.SenderName,
        x.ReceiverName,
        x.Message,
        SentAt = x.SentAt.ToString("h:mm:ss tt d/M/yyyy")
    })
    .ToList();



            if (!chatMsg.Any())
            {
                var response = new ResponseApi
                {
                    IsSuccess = false,
                    Message = "You don't have any massage",
                    Data = null
                };
                return Ok(response);
                
            }
            else
            {
                var response = new ResponseApi
                {
                    IsSuccess = true,
                    Message = "Successfully",
                    Data = chatMsg
                };
                return Ok(response);
               
            }
        }

    }
}
       
   
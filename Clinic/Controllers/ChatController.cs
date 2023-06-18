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
        public ChatController(clinic_dbContext dbContext,IHubContext<ChatHub> chatHub, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _dbContext = dbContext;
            _chatHub = chatHub;
            __httpContextAccessor = httpContextAccessor;

        }

        //[HttpPost("chat/sendmessage")]
        //public async Task<IActionResult> SendMessage([FromBody] ChatMessageVM messageVM)
        //{
        //    var chatMessage = new ChatMessage
        //    {
        //        SenderUserId = _DoctorId,
        //        ReceiverUserId = messageVM.ReceiverUserId,
        //        Message = messageVM.Message,
        //        SentAt = DateTime.UtcNow
        //    };

        //    _dbContext.ChatMessages.Add(chatMessage);
        //   var res=  _dbContext.SaveChanges();

        //    await _chatHub.Clients.User(messageVM.ReceiverUserId).SendAsync("ReceiveMessage", chatMessage);


        //    return Ok(res);
        //}

        //[HttpGet("chat/Getlistchat")]
        //public async Task<IActionResult> GetListChat()
        //{


        //    var res = _dbContext.ChatMessages.Where(x => x.SenderUserId == _DoctorId || x.ReceiverUserId == _DoctorId).ToList();
           

            


        //    return Ok(res);
        //}

        //[HttpGet("chat/getlistmessage")]
        //public async Task<IActionResult> GetListMessage()
        //{


        //    var res = _dbContext.ChatMessages.Where(x => x.SenderUserId == _DoctorId || x.ReceiverUserId == _DoctorId).






        //    return Ok(res);
        //}


    }
}

       
   
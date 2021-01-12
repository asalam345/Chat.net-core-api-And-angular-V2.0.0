using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using chat_server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using DAL;
using Newtonsoft.Json;
using Models.interfaces;

namespace chat_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : GenericController<MessageVM>
    {
        private IGenericService<MessageVM> _genericService;
        public ChatController(IGenericService<MessageVM> genericService) : base(genericService)
        {
            _genericService = genericService;
        }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Newtonsoft.Json;

namespace chat_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginStatusController : ControllerBase
	{
        private DA_LogInStatus _objAuthtication = null;
        public LoginStatusController()
        {
            this._objAuthtication = new DA_LogInStatus();
        }

        [HttpPost("[action]")]
        public async Task<object> addLoginStatus([FromBody] ReqLoginStatus model)
        {
            object result = null; object resdata = null;

            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                resdata = await _objAuthtication.addLoginStatus(model);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            result = new
            {
                resdata
            };

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.interfaces;

namespace chat_server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : GenericController<UserVM>
	{
		private IGenericService<UserVM> _genericService;
		public UsersController(IGenericService<UserVM> genericService) :base(genericService)
		{
			_genericService = genericService;
		}
		[HttpGet("welcome")]
		public string Welcome()
		{
			return "Welcome to our chat application";
		}

		[HttpPost("login")]
		public async Task<object> login([FromBody] UserVM model)
		{
			Result result = null; 

			try
			{
				if (model == null)
				{
					return BadRequest();
				}
				model.ForLogin = true;
				result = await _genericService.Get(model);

			}
			catch (Exception ex)
			{
				ex.ToString();
			}

			//result = new Result()
			//{
			//	Message = resdata.
			//};

			return result;
		}
	}
}

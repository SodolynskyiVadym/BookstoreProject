﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{

    [ApiController]
    public class AuthController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
    }
}

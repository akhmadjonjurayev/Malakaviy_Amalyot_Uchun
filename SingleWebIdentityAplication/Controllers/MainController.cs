using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SingleWebIdentityAplication.Models;
using SingleWebIdentityAplication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingleWebIdentityAplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("default")]
    public class MainController : ControllerBase
    {
        private readonly ITokenService _service;

        public MainController(ITokenService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult GetPage()
        {
            return Ok(new { message = "Hello World" });
        }
        [HttpPost("login")]
        public async Task<IActionResult> GenerateTokenForLogin(LoginViewModel login)
        {
            return Ok(await _service.LogInToken(login));
        }
        [HttpPost("signIn")]
        public async Task<IActionResult> SignInUser(SignInViewModel model)
        {
            return Ok(await _service.SignInToken(model));
        }
        [HttpGet("getAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            return Ok(await _service.GetAllProduct());
        }
        [HttpPost("createProducts")]
        public async Task<IActionResult> AddRangeProducts([FromBody] IEnumerable<Product> products)
        {
            return Ok(await _service.AddRangeProduct(products));
        }
        [HttpGet("getSingleProduct")]
        public async Task<IActionResult> GetSingleProduct(int ProductId)
        {
            return Ok(await _service.GetSingleProduct(ProductId));
        }
        [HttpGet("userpassword")]
        public IActionResult GetAllPassword()
        {
            return Ok(_service.GetAllPassword());
        }
        [HttpPost("addpassword")]
        public async Task<IActionResult> AddPasswords([FromBody] UsersModel user)
        {
            return Ok(await _service.AddUsersPassword(user));
        }
    }
}

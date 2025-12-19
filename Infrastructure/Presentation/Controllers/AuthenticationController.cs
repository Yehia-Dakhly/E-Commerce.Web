using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager _serviceManager) : ApiBaseController
    {
        // Login
        [HttpPost("Login")] // POST BaseUrl/api/Authentication/Login
        public async Task<ActionResult<UserDTo>> Login(LoginDTo loginDTo)
        {
            var User = await _serviceManager.AuthenticationService.LoginAsync(loginDTo);
            return Ok(User);
        }
        // Register
        [HttpPost("Register")] // POST BaseUrl/api/Authentication/Register
        public async Task<ActionResult<UserDTo>> Register(RegisterDTo registerDTo)
        {
            var User = await _serviceManager.AuthenticationService.RegisterAsync(registerDTo);
            return Ok(User);
        }

        // Check Email
        [HttpGet("emailexists")] // GET BaseUrl/api/Authentication/CheckEmail
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            return Ok(await _serviceManager.AuthenticationService.CheckEmailAsync(email));
        }
        // Get Currrents User
        [Authorize]
        [HttpGet("GetUser")] // GET BaseUrl/api/Authentication/GetUser
        public async Task<ActionResult<UserDTo>> GetUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.AuthenticationService.GetUserAsync(Email!)  );
        }
        // Get Current User Address
        [Authorize]
        [HttpGet("Address")] // GET BaseUrl/api/Authentication/UserAddress
        public async Task<ActionResult<AddressDTo>> GetUserAddress() 
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.AuthenticationService.GetUserAddressAsync(Email!));
        }
        // Update Current User Address
        [Authorize]
        [HttpPut("Address")] // PUT BaseUrl/api/Authentication/UserAddress
        public async Task<ActionResult<AddressDTo>> UpdateUserAddress(AddressDTo addressDTo)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.AuthenticationService.UpdateUserAddressAsync(Email!, addressDTo));
        }
    }
}

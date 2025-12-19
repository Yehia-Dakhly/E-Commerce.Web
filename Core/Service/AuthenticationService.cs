using AutoMapper;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransferObjects.IdentityDTos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager, IMapper _mapper, IConfiguration _configuration) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            return User is not null;
        }

        public async Task<UserDTo> GetUserAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email) ?? throw new UserNotFoundException($"{email}");
            var CurrentUser = _mapper.Map<UserDTo>(User);
            CurrentUser.Token = await CreateTokenAsync(User);
            return CurrentUser;
        }

        public async Task<AddressDTo> GetUserAddressAsync(string email)
        {
            var User = await _userManager.Users
                .Include(U => U.Address)
                .FirstOrDefaultAsync(U => U.Email == email) ?? throw new UserNotFoundException($"{email}");
            //if (User.Address is not null)
            //{
                return _mapper.Map<AddressDTo>(User.Address);
            //}
            //throw new AddressNotFoundException(User.UserName!);
        }
        public async Task<AddressDTo> UpdateUserAddressAsync(string email, AddressDTo newAddress)
        {
            var User = await _userManager.Users
                .Include(U => U.Address)
                .FirstOrDefaultAsync(U => U.Email == email) ?? throw new UserNotFoundException($"{email}");
            if (User.Address is not null) // Update
            {
                _mapper.Map(newAddress, User.Address); // Update inside the same object - Change Object State
            }
            else // Add New Address
            {
                User.Address = _mapper.Map<Address>(newAddress);
            }
            await _userManager.UpdateAsync(User);
            return _mapper.Map<AddressDTo>(User.Address);
        }
        public async Task<UserDTo> LoginAsync(LoginDTo loginDTo)
        {
            // Check if Email is Exists or not?!
            var User = await _userManager.FindByEmailAsync(loginDTo.Email) ?? throw new UserNotFoundException(loginDTo.Email);
            // CheckPassword
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDTo.Password);
            if (IsPasswordValid)
            {
                return new UserDTo()
                {
                    DisplayName = User.DisplayName,
                    Email = loginDTo.Email,
                    Token = await CreateTokenAsync(User)
                };
            }
            else
                throw new UnauthorizedException("Wrong Password!");
        }

        public async Task<UserDTo> RegisterAsync(RegisterDTo registerDTo)
        {
            var User = _mapper.Map<ApplicationUser>(registerDTo);
            // Create Application USer - Mapping From RegisterDTo - > Application User
            var Result = await _userManager.CreateAsync(User, registerDTo.Password);
            if (Result.Succeeded)
            {
                return new UserDTo()
                {
                    DisplayName = User.DisplayName,
                    Email = registerDTo.Email,
                    Token = await CreateTokenAsync(User)
                };
                // Return UserDTo
            }
            else
            {
                var Errors = Result.Errors.Select(E => E.Description).ToList();
                throw new BadRequestException(Errors);
                // Throw BadRequest Exception
            }
        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var Claims = new List<Claim>()
            {
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.UserName!),
                new(ClaimTypes.NameIdentifier, user.Id!)
            };
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var item in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, item));
            }
            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey!));
            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken
                (
                    issuer: _configuration["JWTOptions:Issuer"],
                    audience: _configuration["JWTOptions:Audience"],
                    claims: Claims,
                    expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["JWTOptions:Expires"])),
                    signingCredentials: Creds
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}

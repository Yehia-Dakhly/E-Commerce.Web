using Shared.DataTransferObjects.IdentityDTos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        // Login
        // Take Email and Password, Return Token and DisplayName
        Task<UserDTo> LoginAsync(LoginDTo loginDTo);
        // Register
        // Take Email, Password, Username, Displayname and PhoneNumber Then Return Token, Email, DisplayName
        Task<UserDTo> RegisterAsync(RegisterDTo registerDTo);

        // Check Email
        // Take string Email and Return bool boolean
        Task<bool> CheckEmailAsync(string email);

        // Get Current User Address
        // Take string Email Then Return AddressDTo
        Task<AddressDTo> GetUserAddressAsync(string email);

        // Update Current User Address
        // Take Updated AddressDTo and Email Then Return AddressDTo After Update

        Task<AddressDTo> UpdateUserAddressAsync(string email, AddressDTo newAddress);
        // Get Current User
        // Take Email Return (UserDTo) Token, Email and Display Name
        Task<UserDTo> GetUserAsync(string email);
    }
}

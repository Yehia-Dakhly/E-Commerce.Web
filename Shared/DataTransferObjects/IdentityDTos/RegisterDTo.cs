using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.IdentityDTos
{
    public class RegisterDTo
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? UserName { get; set; } = "JohnDakhly";
        public string DisplayName { get; set; } = null!;
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}

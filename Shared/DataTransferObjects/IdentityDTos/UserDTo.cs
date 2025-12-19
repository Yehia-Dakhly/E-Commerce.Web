using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DataTransferObjects.IdentityDTos
{
    public class UserDTo
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
    }
}

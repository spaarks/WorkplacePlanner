using System;
using System.Collections.Generic;
using System.Text;

namespace WorkPlacePlanner.Domain.Dtos.Account
{
    public class AuthToken
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}

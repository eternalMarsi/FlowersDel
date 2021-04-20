using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.IIS.Core;
using Newtonsoft.Json.Linq;

namespace IdentityServiceAPI.Utils
{
    public static class RoleType
    {
        public const string User = "USER";
        public const string Manager = "MANAGER";
        public const string Admin = "ADMIN";
    }
}

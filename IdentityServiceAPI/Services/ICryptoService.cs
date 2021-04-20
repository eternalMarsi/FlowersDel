using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServiceAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace IdentityServiceAPI.Services
{
    public interface ICryptoService
    {
        public Task<bool> VerifyPasswordAsync(byte[] expectedPassword, string passwordString, byte[] localSaltBytes);
        public byte[] GetLocalSaltBytes();

        public byte[] GetSHA256Hash(byte[] bytes);
        public byte[] GetPasswordHash(string passwordString, byte[] localHashBytes);

        public string CreateToken(User user);

        /// <summary>
        /// Verify JWT token using specific scheme
        /// </summary>
        /// <param name="token">JWT-token string</param>
        /// <param name="fullName">Full name of person separated by empty space</param>
        /// <returns>If the operation has been successful, returns the role id. Otherwise -1
        ///</returns>
        public int VerifyToken(string token, string fullName);
    }
}

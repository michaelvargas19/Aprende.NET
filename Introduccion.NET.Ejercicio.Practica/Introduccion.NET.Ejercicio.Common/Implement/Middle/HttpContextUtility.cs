using Introduccion.NET.Ejercicio.Common.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Introduccion.NET.Ejercicio.Common.Implement.Middle
{
    public static class HttpContextUtility
    {
        public static string GetUserLog(this IHttpContextAccessor accessor)
        {
            var claim = accessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.UserData);
            if (claim == null)
            {
                return string.Empty;
            }
            else
            {
                if (claim.IsEmpty() || !claim.Any()) return string.Empty;

                UserData data = JsonSerializer.Deserialize<UserData>(claim.First().Value);
                return data.Username;
            }
        }

        public static string GetRol(this IHttpContextAccessor accessor)
        {
            var claim = accessor.HttpContext?.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return string.Empty;
            }
            else
            {
                if (claim.IsEmpty()) return string.Empty;
                return claim.Value;
            }
        }

        public static string GetEmail(this IHttpContextAccessor accessor)
        {
            var claim = accessor.HttpContext?.User.Claims.First(c => c.Type == ClaimTypes.Email);
            if (claim == null)
            {
                return string.Empty;
            }
            else
            {
                if (claim.IsEmpty()) return string.Empty;
                return claim.Value;
            }
        }

        public static Guid GetUserId(this IHttpContextAccessor accessor)
        {
            var claim = accessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid);
            if (claim == null) return Guid.Parse("00000000-0000-0000-0000-000000000000");
            return Guid.Parse(claim.Value);
        }

        public static string GetToken(this IHttpContextAccessor context)
        {
            var token = context.HttpContext == null ? "" : context.HttpContext.Request.Headers["Authorization"].ToString();
            return !token.IsEmpty() ? token : "";
        }

        public static string CreateAutomaticToken(string key, string userName, string rolName)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] tokenKey = Encoding.ASCII.GetBytes(key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, rolName),
                    new Claim(ClaimTypes.Authentication, key)
                }),
                Expires = DateTime.UtcNow.AddSeconds(600),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenSerialized = tokenHandler.WriteToken(token);
            return tokenSerialized;
        }
    }
}

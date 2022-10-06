using KantanMitsumori.Helper.Settings;
using KantanMitsumori.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KantanMitsumori.Helper.CommonFuncs
{
    public static class HelperToken
    {           
        public static LogToken? EncodingToken(JwtSettings settings, string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token)) return null;
                var tokenHandler = new JwtSecurityTokenHandler();            
                var key = Encoding.UTF8.GetBytes(settings.Key);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,               
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var genderStr = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
                if (!string.IsNullOrWhiteSpace(genderStr))
                {
                    var model = JsonConvert.DeserializeObject<LogToken>(genderStr);
                    return model;
                }
                return null;
            }
            catch 
            {
                return null;
            }
        }
        public static string GenerateJsonToken(JwtSettings settings, LogToken model)
        {           
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            string genderStr = JsonConvert.SerializeObject(model);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,genderStr),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var currentDate = DateTime.Now;
            var RefreshExpires = settings.AccessExpires;
            TimeSpan time = TimeSpan.Parse(RefreshExpires);
            var token = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Issuer,
                claims,
                notBefore: currentDate,
                expires: currentDate.Add(time),
                signingCredentials: credentials
             );
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }
    }
}

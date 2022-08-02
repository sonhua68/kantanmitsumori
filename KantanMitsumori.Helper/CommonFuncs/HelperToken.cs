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
        public static IConfiguration _config;
        public static void Configure(IConfiguration config)
        {
            _config = config;


        }

        public static LogToken EncodingToken(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token)) return null;
                var tokenHandler = new JwtSecurityTokenHandler();
                if (_config == null) return null;
                var key = Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var genderStr = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
                if (!string.IsNullOrWhiteSpace(genderStr))
                {
                    LogToken model = JsonConvert.DeserializeObject<LogToken>(genderStr);
                    return model;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
        public static string GenerateJsonToken(LogToken model)
        {
            var jwtKey = _config["JwtSettings:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            string genderStr = JsonConvert.SerializeObject(model);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,genderStr),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var currentDate = DateTime.Now;
            var RefreshExpires = _config["JwtSettings:RefreshExpires"];
            TimeSpan time = TimeSpan.Parse(RefreshExpires);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Issuer"],
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

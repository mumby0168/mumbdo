using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using Mumbdo.Shared.Dtos;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Mumbdo.Web.Tests.Tools
{
    public static class JwtHelper
    {
        private static char base64PadCharacter = '=';
        private static char base64Character62 = '+';
        private static char base64Character63 = '/';
        private static char base64UrlCharacter62 = '-';
        private static char _base64UrlCharacter63 = '_';
        private static readonly DateTime DateFrom = new DateTime (1970, 1, 1);
        public static JwtTokenDto CreateFakeToken(Guid id, string email, string role, DateTime expiry, string refresh = null)
        {
            var claims = new Dictionary<string, string>();
            claims.Add("nameid", id.ToString());
            claims.Add("email", email);
            claims.Add("role", role);
            var diff = expiry - DateFrom;
            claims.Add("exp", diff.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            var json = JsonSerializer.Serialize(claims);
            return new JwtTokenDto($@"sdfdfgsdf.{Encode(Encoding.UTF8.GetBytes(json))}.224r243534s", refresh);
        }
        
        private static string Encode(byte[] inArray)
        {
            string s = Convert.ToBase64String(inArray, 0, inArray.Length);
            s = s.Split(base64PadCharacter)[0]; // Remove any trailing padding
            s = s.Replace(base64Character62, base64UrlCharacter62);  // 62nd char of encoding
            s = s.Replace(base64Character63, _base64UrlCharacter63);  // 63rd char of encoding

            return s;
        }
    }
}
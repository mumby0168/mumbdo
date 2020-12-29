using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Mumbdo.Shared
{
    public record SignedInUser
    {
        private readonly DateTime DateFrom = new DateTime (1970, 1, 1);
        
        public SignedInUser(string token)
        {
            Token = token;
            string payload = token.Split ('.') [1];
            var bytes = ParseBase64WithoutPadding (payload);
            var json = Encoding.ASCII.GetString (bytes);
            var dictionary = JsonSerializer.Deserialize<Dictionary<string, object>> (json);
            if (dictionary is null)
            {
                throw new InvalidOperationException("Could not process jwt token");
            }
            if (dictionary.TryGetValue ("role", out object role)) {
                Role = role.ToString ();
            }
            if (dictionary.TryGetValue ("email", out object email)) {
                Email = email.ToString ();
            }
            if (dictionary.TryGetValue ("exp", out object expiryInSeconds)) {
                Expiry = DateFrom.AddSeconds (double.Parse(expiryInSeconds.ToString()));
            }
            if (dictionary.TryGetValue("nameid", out object id))
            {
                Id = Guid.Parse(id.ToString());
            }
        }

        public SignedInUser()
        {
            
        }

        private byte[] ParseBase64WithoutPadding (string base64) {
            switch (base64.Length % 4) {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }
            return Convert.FromBase64String (base64);
        }

        public DateTime Expiry
        {
            get;
            init;
        }

        public string Token { get; }
        
        public string Email { get; }
        
        public string Role { get; }
        
        public Guid Id { get; }
    }
}
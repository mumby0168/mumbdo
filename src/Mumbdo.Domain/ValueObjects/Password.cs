namespace Mumbdo.Domain.ValueObjects
{
    public record Password
    {
        public string Salt { get; }
        
        public string Hash { get; }
        
        public string Iterations { get; }

        public Password(string hash, string salt, string iterations)
        {
            Salt = salt;
            Hash = hash;    
            Iterations = iterations;
        }

        public string Pack()
        {
            return Hash + Salt + Iterations;
        }

        public static Password UnPack(string packed)
        {
            
            string base64Hash = packed.Substring(0, PasswordSettings.Base64HashLength);
            string base64NumberOfIterations = packed.Substring(packed.Length - PasswordSettings.Base64IterationsLength, PasswordSettings.Base64IterationsLength);
            string base64Salt = packed.Substring(PasswordSettings.Base64HashLength, packed.Length - PasswordSettings.Base64HashLength - PasswordSettings.Base64IterationsLength);
            return new Password(base64Hash, base64Salt, base64NumberOfIterations);
        }
        
        
    }
}
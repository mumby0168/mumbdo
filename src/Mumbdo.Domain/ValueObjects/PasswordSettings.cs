namespace Mumbdo.Domain.ValueObjects
{
    public static class PasswordSettings
    {
        public const int HashLength = 32;
        public const int Base64HashLength = ((HashLength * 4) / 3) + (HashLength % 3);
        public const int SaltLength = 32;
        public const int Base64IterationsLength = 8;   
    }
}
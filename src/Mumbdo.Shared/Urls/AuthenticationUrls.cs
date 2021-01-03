namespace Mumbdo.Shared.Urls
{
    public static class AuthenticationUrls
    {
        public const string SignIn = "/api/users/signin";

        public const string SignUp = "/api/users/signup";

        public static string Refresh(string token, string email) => $"/api/refresh?token={token}&email={email}";
    }
}
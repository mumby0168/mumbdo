namespace Mumbdo.Shared.Urls
{
    public static class GroupUrls
    {
        public const string CreateGroupUrl = "/api/groups";

        public static string GetGroupsUrl(bool includeTasks) => $"/api/groups/{includeTasks}";
    }
}
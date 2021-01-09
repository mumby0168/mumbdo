using System;

namespace Mumbdo.Shared.Urls
{
    public static class GroupUrls
    {
        public const string CreateGroupUrl = "/api/groups";

        public static string GetGroupsUrl(bool includeTasks) => $"/api/groups/{includeTasks}";

        public static string GetGroupUrl(Guid groupId, bool includeTasks = true) =>
            $@"/api/groups?includeTasks={includeTasks}&groupId={groupId}";
    }
}
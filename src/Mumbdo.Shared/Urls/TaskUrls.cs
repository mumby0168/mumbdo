using System;

namespace Mumbdo.Shared.Urls
{
    public static class TaskUrls
    {
        public const string CreateTaskUrl = "/api/tasks";
        
        public const string UpdateTaskUrl = "/api/tasks";

        public static string DeleteTaskUrl(Guid id) => $"/api/tasks/{id}";

        public static string UngroupedTasksUrl(bool completedTasks) => $"/api/tasks/ungrouped/{completedTasks}";
    }
}
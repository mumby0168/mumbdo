using System;

namespace Mumbdo.Shared.Dtos
{
    public record CreateTaskDto(string Name, Guid? GroupId, DateTime? Deadline);

    public record UpdateTaskDto(Guid Id, string Name, bool IsComplete, Guid? GroupId, DateTime? Deadline);
}
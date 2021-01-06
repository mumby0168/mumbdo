using System;

namespace Mumbdo.Shared.Dtos
{
    public record CreateTaskDto(string Name, Guid? GroupId, DateTime? Deadline);
}
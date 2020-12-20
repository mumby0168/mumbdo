
using System;
using System.Collections.Generic;

namespace Mumbdo.Shared
{
    public record ItemGroupDto(Guid Id, string Name, string Description, string Image, List<TaskDto> Tasks = null);

    public record TaskDto(string Name, DateTime Created, bool IsComplete, DateTime? Deadline = null);
}
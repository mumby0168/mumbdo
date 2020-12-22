using System;
using System.Collections.Generic;

namespace Mumbdo.Shared.Dtos
{
    public record ItemGroupDto(Guid Id, string Name, string Description, string Image, List<TaskDto> Tasks);

    public record TaskDto(string Name, DateTime Created, bool IsComplete, DateTime? Deadline = null);

    public record CreateItemGroupDto(string Name, string Description, string Image);
}
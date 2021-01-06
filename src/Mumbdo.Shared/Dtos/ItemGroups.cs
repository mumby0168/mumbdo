using System;
using System.Collections.Generic;

namespace Mumbdo.Shared.Dtos
{
    public record ItemGroupDto(Guid Id, string Name, string Description, string Image, List<TaskDto> Tasks);

    public record TaskDto(Guid Id, string Name, DateTime Created, bool IsComplete, Guid? GroupId, DateTime? Deadline = null);

    public record CreateItemGroupDto(string Name, string Description, string Image);
}
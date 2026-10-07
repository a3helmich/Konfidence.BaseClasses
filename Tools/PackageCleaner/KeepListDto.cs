using System.Collections.Generic;

namespace PackageCleaner;

internal sealed class KeepListDto
{
    public Dictionary<string, List<int>> Keep { get; set; } = new();
}

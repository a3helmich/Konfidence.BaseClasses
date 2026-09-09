namespace Konfidence.Logging.Inspection;

internal sealed class FileSinkOptions
{
    public required string Path { get; init; }

    public required string OutputTemplate { get; init; }
}

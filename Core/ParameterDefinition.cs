namespace ImageProcessor.Core
{
    public readonly record struct ParameterDefinition(
    string Name,
    TypeCode Type,
    bool Required = false,
    object? DefaultValue = null,
    string? Description = null);
}
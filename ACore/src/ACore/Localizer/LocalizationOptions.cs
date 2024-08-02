namespace ACore.Localizer;

public class LocalizationOptions
{
    /// <summary>
    /// For true = name of key
    /// For false = contextId + name of key
    /// </summary>
    public bool ReturnOnlyKeyIfNotFound { get; set; } = true;
}
namespace JMCore.Localizer
{
    /// <summary>
    /// Scope for retrieve localization data from db.
    /// </summary>
    [Flags]
    public enum LocalizationScopeEnum
    {
        Server = 1,
        Client = 2
    }
}

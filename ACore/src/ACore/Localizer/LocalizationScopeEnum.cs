namespace ACore.Localizer
{
  /// <summary>
  /// Scope for retrieve localization data from db.
  /// </summary>
  [Flags]
  public enum LocalizationScopeEnum
  {
    Server = 1 << 0,
    Client = 1 << 1
  }
}
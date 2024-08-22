using System.Globalization;
using System.Text.Json;
using ACore.Server.Modules.AuditModule.Models;
using Microsoft.Extensions.Logging;

// ReSharper disable MemberCanBePrivate.Global

namespace ACore.Server.Modules.AuditModule.Storage.SQL.Models;

internal class AuditSqlValueItem(string columName, Type dataType)
{
  public const int MaxStringSize = 10000;
  public string AuditColumnName { get; } = columName;

  public Type AuditColumnDataType { get; } = dataType;
  public string? OldValueString { get; set; }
  public string? NewValueString { get; set; }
  public int? OldValueInt { get; set; }
  public int? NewValueInt { get; set; }
  public long? OldValueLong { get; set; }
  public long? NewValueLong { get; set; }
  public bool? OldValueBool { get; set; }
  public bool? NewValueBool { get; set; }
  public Guid? OldValueGuid { get; set; }
  public Guid? NewValueGuid { get; set; }

  public static AuditSqlValueItem? CreateValue(ILogger logger, AuditEntryColumnItem coll)
  {
    var value = new AuditSqlValueItem(coll.ColumnName, coll.DataType);

    if (coll.OldValue != null)
    {
      switch (coll.OldValue)
      {
        case byte b:
          value.OldValueInt = b;
          break;
        case short s:
          value.OldValueInt = s;
          break;
        case int i:
          value.OldValueInt = i;
          break;
        case long l:
          value.OldValueLong = l;
          break;
        case bool bl:
          value.OldValueBool = bl;
          break;
        case Guid g:
          value.OldValueGuid = g;
          break;
        case DateTime dt:
          value.OldValueLong = dt.Ticks;
          break;
        case TimeSpan span:
          value.OldValueLong = span.Ticks;
          break;
        case string st:
          value.OldValueString = st;
          break;
        case decimal dec:
          value.OldValueString = dec.ToString(CultureInfo.InvariantCulture);
          break;
        default:
          value.OldValueString = ToValueString(logger, coll.OldValue);
          break;
      }
    }

    if (coll.NewValue != null)
    {
      switch (coll.NewValue)
      {
        case byte b:
          value.NewValueInt = b;
          break;
        case short s:
          value.NewValueInt = s;
          break;
        case int i:
          value.NewValueInt = i;
          break;
        case long l:
          value.NewValueLong = l;
          break;
        case bool bl:
          value.NewValueBool = bl;
          break;
        case Guid g:
          value.NewValueGuid = g;
          break;
        case DateTime dt:
          value.NewValueLong = dt.Ticks;
          break;
        case TimeSpan span:
          value.NewValueLong = span.Ticks;
          break;
        case string st:
          value.NewValueString = st;
          break;
        case decimal dec:
          value.NewValueString = dec.ToString(CultureInfo.InvariantCulture);
          break;
        default:
          value.NewValueString = ToValueString(logger, coll.NewValue);
          break;
      }
    }

    if (value.OldValueInt == value.NewValueInt &&
        value.OldValueLong == value.NewValueLong &&
        value.OldValueBool == value.NewValueBool &&
        value.OldValueString == value.NewValueString &&
        value.OldValueGuid == value.NewValueGuid)
      return null;

    return value;
  }

  private static string ToValueString(ILogger logger, object value)
  {
    var valueString = JsonSerializer.Serialize(value);

    switch (value)
    {
      case byte[]:
        break;
      default:
        throw new Exception($"Unknown type for audit. Type: {value.GetType()}; Value:{valueString}");
    }

    if (valueString.Length > MaxStringSize)
      // This message is used in unit test.
      logger.LogError("The value exceeded the maximum character length '{MaxStringSize}'. Value:{Value}", MaxStringSize, valueString);

    return valueString;
  }
}
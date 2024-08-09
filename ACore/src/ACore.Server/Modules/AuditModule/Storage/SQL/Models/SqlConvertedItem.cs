using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using ACore.Extensions;
using ACore.Server.Storages.Models.SaveInfo;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

// ReSharper disable MemberCanBePrivate.Global

namespace ACore.Server.Modules.AuditModule.Storage.SQL.Models;

internal class SqlConvertedItem(string propName, string columName, bool isChanged, string dataType)
{
  public const int MaxStringSize = 10000;
  public string PropName => propName;
  public string ColumnName => columName;
  public string DataType => dataType;
  public bool IsChanged => isChanged;
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

  public static SqlConvertedItem CreateValue(ILogger logger, SaveInfoColumnItem coll)
  {
    var value = new SqlConvertedItem(coll.PropName, coll.ColumnName, coll.IsChanged, coll.DataType);

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

    if (coll.IsChanged && coll.NewValue != null)
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

    return value;
  }

  public static string ToValueString(ILogger logger, object value)
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

  public static object? ConvertObjectToDataType(string dataType, object? value)
  {
    if (string.IsNullOrEmpty(dataType))
      throw new ArgumentNullException($"Data type is null.");

    if (value == null)
      return null;

    if (dataType == typeof(ObjectId).ACoreTypeName())
      return new ObjectId(value.ToString());

    if (dataType == typeof(DateTime).ACoreTypeName())
      return new DateTime(Convert.ToInt64(value), DateTimeKind.Utc);

    var type = Type.GetType(dataType);

    if (dataType == typeof(byte[]).ACoreTypeName() && type != null)
      return JsonSerializer.Deserialize(value.ToString() ?? throw new NullReferenceException(), type);
    
    if (type == null)
      throw new Exception($"Cannot create data type '{dataType}'.");

    var c = ChangeType(value, type);
    return c;
  }


  public static object ChangeType(object value, Type conversionType)
  {
    if (conversionType == null)
    {
      throw new ArgumentNullException("conversionType");
    }

    if (conversionType.IsGenericType &&
        conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
    {
      if (value == null)
      {
        return null;
      }

      NullableConverter nullableConverter = new NullableConverter(conversionType);
      conversionType = nullableConverter.UnderlyingType;
    }

    return Convert.ChangeType(value, conversionType);
  }
}
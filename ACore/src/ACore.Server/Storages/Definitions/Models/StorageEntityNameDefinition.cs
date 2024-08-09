using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace ACore.Server.Storages.Definitions.Models;

public class StorageEntityNameDefinition(string tableName, IDictionary columns)
{
  private readonly IDictionary? _columns = columns;
  private Dictionary<string, string>? _columnNames;

  public string TableName { get; } = tableName;

  public Dictionary<string, string>? ColumnNames
  {
    get
    {
      if (_columns == null)
        return null;

      InitColumnNames();
      return _columnNames;
    }
  }

  public Dictionary<Expression<Func<T, object>>, string> GetColumns<T>()
  {
    return _columns as Dictionary<Expression<Func<T, object>>, string> ?? throw new InvalidOperationException($"Columns are not defined for '{typeof(T).Name}' entity.");
  }

  private static object GetPropValue(object src, string propName)
  {
    return src.GetType().GetProperty(propName)?
      .GetValue(src, null) ?? throw new InvalidOperationException($"Prop {propName} doesn't exist.");
  }

  private static string GetPropertyName(object body)
  {
    var memberExpression = body switch
    {
      UnaryExpression ue => (MemberExpression)(ue.Operand),
      MemberExpression me => me,
      _ => throw new Exception($"Cannot get {nameof(MemberExpression)} from body")
    };
    return ((PropertyInfo)memberExpression.Member).Name;
  }

  private void InitColumnNames()
  {
    if (_columns == null)
      return;
    
    if (_columnNames != null)
      return;

    _columnNames = new Dictionary<string, string>();
    foreach (var columnDef in _columns)
    {
      if (columnDef == null)
        throw new Exception("");
      var key = GetPropValue(columnDef, "Key");
      var expressionBody = GetPropValue(key, "Body");
      var expressionPropertyName = GetPropertyName(expressionBody);
      _columnNames.Add(expressionPropertyName, GetPropValue(columnDef, "Value").ToString() ?? throw new InvalidOperationException($"Prop {expressionPropertyName} doesn't exist."));
    }
  }
}
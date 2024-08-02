using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace JMCore.Server.Storages.Models;

public class StorageEntityNameDefinition
{
  private readonly IDictionary _columns;
  private Dictionary<string, string>? _columnNames;
  
  public string TableName { get; }
  public Dictionary<string, string> ColumnNames
  {
    get
    {
      InitColumnNames();
      return _columnNames!;
    }
  }
  
  public Dictionary<Expression<Func<T, object>>, string> GetColumns<T>()
  {
    return _columns as Dictionary<Expression<Func<T, object>>, string> ?? throw new InvalidOperationException($"Columns are not defined for '{typeof(T).Name}' entity.");
  }
  
  public StorageEntityNameDefinition(string tableName, IDictionary columns)
  {
    TableName = tableName;
    _columns = columns;
  }


  private static object GetPropValue(object src, string propName)
  {
    return src.GetType().GetProperty(propName)?.GetValue(src, null) ?? throw new InvalidOperationException($"Prop {propName} doesn't exist.");
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
      _columnNames.Add(expressionPropertyName, GetPropValue(columnDef, "Value").ToString()!);
    }
  }
}
using System.Collections.ObjectModel;
using ACore.Base.CQRS.Models.Results;
using ACore.Base.CQRS.Models.Results.Error;
using ACore.Extensions;
using ACore.Server.Storages.Models;
using ACore.Server.Storages.Models.PK;

namespace ACore.Server.Storages.CQRS;

public class DbSaveResultData(object pk, string? hash = null)
{
  public object PK => pk;
  public string? Hash => hash;
}

public class DbSaveResult : Result
{
  public ReadOnlyDictionary<StorageTypeEnum, DbSaveResultData> ReturnedValues { get; }

  private static DbSaveResult SuccessWithValues(Dictionary<StorageTypeEnum, DbSaveResultData> pkValues) => new(pkValues);

  public static DbSaveResult SuccessWithData(IEnumerable<SaveHandlerData> data, string saltForHash = "")
  {
    return DbSaveResult.SuccessWithValues(data.ToDictionary(
      k => k.Storage.StorageDefinition.Type,
      v => new DbSaveResultData(
        v.Entity.PropertyValue(nameof(PKEntity<int>.Id)) ?? throw new Exception($"{nameof(PKEntity<int>.Id)} is null."),
        v.WithHash ? v.Entity.HashObject(saltForHash) : null
      )));
  }

  public static DbSaveResult SuccessWithData<T>(IEnumerable<SaveHandlerData<T>> data, string saltForHash = "") where T : class
  {
    return SuccessWithValues(data.ToDictionary(
      k => k.Storage.StorageDefinition.Type,
      v => new DbSaveResultData(
        v.Entity.PropertyValue(nameof(PKEntity<int>.Id)) ?? throw new Exception($"{nameof(PKEntity<int>.Id)} is null."),
        v.WithHash ? v.Entity.HashObject(saltForHash) : null
      )));
  }

  private DbSaveResult(IDictionary<StorageTypeEnum, DbSaveResultData> pkValues) : base(true, Error.None)
  {
    ReturnedValues = pkValues.AsReadOnly();
  }

  /// <summary>
  /// Return the first PK value. Value must exist.
  /// </summary>
  public T PrimaryKeySingle<T>()
  {
    if (ReturnedValues.Count != 1)
      throw new Exception($"No suitable {nameof(ReturnedValues)} is available. Count of items is {ReturnedValues.Count}.");

    return (T)ReturnedValues.First().Value.PK;
  }

  public string? HashSingle()
  {
    if (ReturnedValues.Count != 1)
      throw new Exception($"No suitable {nameof(ReturnedValues)} is available. Count of items is {ReturnedValues.Count}.");

    return ReturnedValues.First().Value.Hash;
  }
}
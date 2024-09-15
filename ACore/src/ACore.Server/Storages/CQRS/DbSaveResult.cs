using System.Collections.ObjectModel;
using ACore.Base.CQRS.Models;
using ACore.Server.Storages.Models;

namespace ACore.Server.Storages.CQRS;

public class DbSaveResult : Result
{
  public ReadOnlyDictionary<StorageTypeEnum, object> PKValues { get; }

  public static DbSaveResult SuccessWithPkValues(Dictionary<StorageTypeEnum, object>  pkValues) => new(pkValues);

  private DbSaveResult(Dictionary<StorageTypeEnum, object>  pkValues) : base(true, Error.None)
  {
    PKValues = pkValues.AsReadOnly();
  }
}